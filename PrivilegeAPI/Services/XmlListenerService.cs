using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Net;
using System.Text;
using System.Xml.Linq;
using File = PrivilegeAPI.Models.File;

namespace PrivilegeAPI.Services
{
    public class XmlListenerService : BackgroundService
    {
        private readonly HttpListener _listener;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FtpService _ftpService;
        private readonly IHubContext<XmlProcessingHub> _hubContext;
        private readonly ILogger<XmlListenerService> _logger;
        private readonly AnswerService _answerService;
        private readonly PortalService _portalService;
        private readonly string _url = "http://+:5059/";
        private bool _isDisposing;

        public XmlListenerService(IServiceScopeFactory scopeFactory, FtpService ftpService, IHubContext<XmlProcessingHub> hubContext, ILogger<XmlListenerService> logger, AnswerService answerService, PortalService portalService)
        {
            _listener = new HttpListener();
            _scopeFactory = scopeFactory;
            _ftpService = ftpService;
            _hubContext = hubContext;
            _logger = logger;
            _answerService = answerService;
            _portalService = portalService;
            _isDisposing = false;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Attempting to register URL prefix: {Url}", _url);
                _listener.Prefixes.Add(_url);
                _logger.LogInformation("Starting HttpListener on {Url}", _url);
                
                _listener.Start();
                await Task.Run(() => MonitorFtpFolderAsync("Finali", stoppingToken), stoppingToken);

                _logger.LogInformation("HttpListener IsSuccessfully started on {Url}", _url);

                while (!stoppingToken.IsCancellationRequested && !_isDisposing)
                {
                    try
                    {
                        var context = await _listener.GetContextAsync().ConfigureAwait(false);
                        if (!_isDisposing)
                        {
                            _ = Task.Run(() => ProcessRequestAsync(context, stoppingToken), stoppingToken);
                        }
                    }
                    catch (HttpListenerException ex) when (ex.ErrorCode == 995 || ex.Message.Contains("The I/O operation has been aborted"))
                    {
                        _logger.LogInformation("HttpListener stopped due to cancellation or shutdown");
                        break;
                    }
                    catch (ObjectDisposedException)
                    {
                        _logger.LogInformation("HttpListener was disposed, exiting loop");
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error processing HttpListener request");
                    }
                }
            }
            catch (HttpListenerException ex)
            {
                _logger.LogError(ex, "Failed to start HttpListener on {Url}", _url);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error starting HttpListener on {Url}", _url);
                throw;
            }
            finally
            {
                if (_listener.IsListening)
                {
                    _logger.LogInformation("Stopping HttpListener");
                    _listener.Stop();
                }
                _listener.Close();
                _isDisposing = true;
                _logger.LogInformation("HttpListener stopped");
            }
        }

        private async Task ProcessRequestAsync(HttpListenerContext context, CancellationToken cancellationToken)
        {
            if (_isDisposing || cancellationToken.IsCancellationRequested)
                return;

            try
            {
                var request = context.Request;
                var clientIp = request.RemoteEndPoint?.Address.ToString();
                _logger.LogInformation("Received request from {ClientIp}", clientIp);

                if (request.HttpMethod == "POST" && (request.ContentType == "multipart/form-data" || request.ContentType == "application/xml" || request.ContentType == "application/json" || request.ContentType == "text/plain"))
                {
                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    var body = await reader.ReadToEndAsync(cancellationToken);

                    string xmlContent;

                    if (request.ContentType == "application/json")
                    {
                        var json = System.Text.Json.JsonDocument.Parse(body);
                        if (!json.RootElement.TryGetProperty("base64Content", out var base64Element))
                        {
                            throw new Exception("Поле base64Content не найдено в JSON.");
                        }

                        var base64 = base64Element.GetString();
                        if (string.IsNullOrWhiteSpace(base64))
                        {
                            throw new Exception("base64Content пустой.");
                        }

                        byte[] xmlBytes = Convert.FromBase64String(base64);
                        xmlContent = Encoding.UTF8.GetString(xmlBytes);
                    }
                    else
                    {
                        xmlContent = body;
                    }

                    _logger.LogDebug("Received XML: {XmlContent}", xmlContent);

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    string fileName = $"Application_{DateTime.Now:yyyyMMddHHmmss}.xml";
                    string virtualPath = $"Applications/New/{fileName}";

                    var file = new File
                    {
                        Name = fileName,
                        Path = virtualPath
                    };

                    var signedDoc = await _answerService.SendToMedAsync(xmlContent);
                    if (signedDoc != null)
                    {
                        var application = ParseXml(signedDoc.ToString(), file);
                        application.FileId = file.Id;

                        dbContext.Files.Add(file);
                        dbContext.Applications.Add(application);
                        await dbContext.SaveChangesAsync(cancellationToken);
                    }

                    await _ftpService.SaveFileAsync(virtualPath, xmlContent);

                    dbContext.Files.Add(file);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    //bool answer = await _answerService.SendAnswerDeliveredAsync(application);

                    //if (!answer)
                    //{
                    //    throw new Exception("Failed to send reply.");
                    //}

                    await _hubContext.Clients.All.SendAsync(
                        "ReceiveMessage",
                        $"XML processed from {clientIp}.",
                        cancellationToken
                    );

                    var response = context.Response;
                    var responseString = $"{{\"Message\":\"XML processed and saved to database.\"}}";
                    var buffer = Encoding.UTF8.GetBytes(responseString);
                    response.ContentLength64 = buffer.Length;
                    response.ContentType = "application/json";
                    await response.OutputStream.WriteAsync(buffer, 0, buffer.Length, cancellationToken);
                    response.StatusCode = 200;
                }
                else
                {
                    context.Response.StatusCode = 415;
                    var error = Encoding.UTF8.GetBytes("{\"Error\":\"Unsupported Media Type. Expected text/xml or application/xml.\"}");
                    context.Response.ContentLength64 = error.Length;
                    context.Response.ContentType = "application/json";
                    await context.Response.OutputStream.WriteAsync(error, 0, error.Length, cancellationToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing XML request from {ClientIp}", context.Request.RemoteEndPoint?.Address.ToString());
                context.Response.StatusCode = 400;
                var error = Encoding.UTF8.GetBytes("{\"Error\":\"Error processing XML: " + ex.Message + "\"}");
                context.Response.ContentLength64 = error.Length;
                context.Response.ContentType = "application/json";
                await context.Response.OutputStream.WriteAsync(error, 0, error.Length, cancellationToken);
            }
            finally
            {
                context.Response.Close();
            }
        }

        private async Task MonitorFtpFolderAsync(string ftpFolder, CancellationToken cancellationToken)
        {
            var knownFiles = new HashSet<string>();

            while (!cancellationToken.IsCancellationRequested && !_isDisposing)
            {
                try
                {
                    await _ftpService.ConnectAsync();

                    var files = await _portalService.ListDirectoryAsync(ftpFolder);
                    _logger.LogInformation("FTP folder {FolderPath} checked at {Time}", ftpFolder, DateTime.Now);

                    foreach (var file in files)
                    {
                        var fileName = Path.GetFileName(file);

                        if (!knownFiles.Contains(file) && fileName.StartsWith("Confirmation", StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogInformation("Найден новый Confirmation файл: {File}", fileName);

                            await _ftpService.ConnectAsync();

                            var localFolder = Path.Combine(AppContext.BaseDirectory, "Applications");
                            Directory.CreateDirectory(localFolder);

                            var localPath = Path.Combine(localFolder, fileName);

                            await _ftpService.DownloadFileAsync(fileName, localPath);

                            var xdoc = XDocument.Load(localPath);

                            var id = xdoc.Root?.Element("id")?.Value;
                            var kodorg = xdoc.Root?.Element("kodorg")?.Value;
                            var kodorgout = xdoc.Root?.Element("kodorgout")?.Value;

                            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(kodorg) || string.IsNullOrWhiteSpace(kodorgout))
                            {
                                _logger.LogWarning("Confirmation file {File} is missing required elements: id, kodorg, kodorgout", fileName);
                                await _ftpService.DeleteFileAsync(fileName);
                                continue;
                            }

                            var result = await _answerService.ProcessFileAsync(id, kodorg, kodorgout);  

                            await _ftpService.DeleteFileAsync(fileName);
                            
                            _logger.LogInformation("Confirmation: id={Id}, kodorg={Kodorg}, kodorgout={Kodorgout}", id, kodorg, kodorgout);

                            knownFiles.Add(fileName);
                        }
                    }

                    await _ftpService.DisconnectAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при проверке FTP папки {FolderPath}", ftpFolder);
                }

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }

        private Application ParseXml(string xmlContent, File file)
        {
            try
            {
                xmlContent = $"<root>{xmlContent}</root>";
                XDocument doc = XDocument.Parse(xmlContent);

                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var htmlx = doc.Root?.Element(ns + "htmlx")
                            ?? throw new Exception("Missing <htmlx> element");

                var body2 = htmlx.Element(ns + "body2")
                            ?? throw new Exception("Missing <body2> element");

                var container = body2.Element(ns + "container")
                                ?? throw new Exception("Missing <container> element");

                string fullName = container.Element(ns + "nameru")?.Value
                                  ?? throw new Exception("Missing <nameru> element");

                var reg = container.Element(ns + "reg")
                          ?? throw new Exception("Missing <reg> element");

                string dateString = reg.Element(ns + "datareg")?.Value;
                if (!DateTime.TryParse(dateString, out DateTime applicationDate))
                {
                    throw new Exception("Invalid or missing registration date");
                }

                string regNumber = reg.Element(ns + "regnumber")?.Value;
                string uslugNumber = reg.Element(ns + "uslugnumber")?.Value;

                string kodorg = container.Element(ns + "kodorg")?.Value;

                var destination = container
                    .Element(ns + "destinations")
                    ?.Element(ns + "destination")
                    ?.Element(ns + "legalentity");

                string kodorgout = destination?.Element(ns + "kodorgout")?.Value;
                string nameorg = destination?.Element(ns + "nameorg")?.Value;

                var servInfo = body2.Element(ns + "servinfo")
                               ?? throw new Exception("Missing <servinfo> element");

                string idgosuslug = servInfo.Element(ns + "idgosuslug")?.Value;

                return new Application
                {
                    Status = StatusEnum.Delivered,
                    DateAdd = applicationDate,
                    DateEdit = DateTime.Now,
                    File = file,
                    Idgosuslug = idgosuslug,
                    Org = kodorg,
                    Orgout = kodorgout,
                    Orgnumber = regNumber,
                    Uslugnumber = uslugNumber
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse XML: {ex.Message}", ex);
            }
        }

        //private Application ParseXml(string xmlContent, File file)
        //{
        //    try
        //    {
        //        xmlContent = $"<root>{xmlContent}</root>";
        //        XDocument doc = XDocument.Parse(xmlContent);

        //        XNamespace ns = "http://www.w3.org/1999/xhtml";

        //        var htmlx = doc.Root?.Element(ns + "htmlx")
        //                    ?? throw new Exception("Missing <htmlx> element");

        //        var body2 = htmlx.Element(ns + "body2")
        //                    ?? throw new Exception("Missing <body2> element");

        //        var container = body2.Element(ns + "container")
        //                        ?? throw new Exception("Missing <container> element");

        //        var persData = container
        //            .Element(ns + "topheader")
        //            ?.Element(ns + "tophead")
        //            ?.Element(ns + "pers_data")
        //            ?? throw new Exception("Missing <pers_data> element");

        //        string fam = persData.Element(ns + "fam")?.Value ?? "";
        //        string im = persData.Element(ns + "im")?.Value ?? "";
        //        string ot = persData.Element(ns + "ot")?.Value ?? "";

        //        string fullName = $"{fam} {im} {ot}".Trim();
        //        if (string.IsNullOrWhiteSpace(fullName))
        //            throw new Exception("FullName is required");

        //        string dateString = container.Element(ns + "dateblank")?.Value;
        //        if (!DateTime.TryParse(dateString, out DateTime applicationDate))
        //        {
        //            throw new Exception("Invalid or missing ApplicationDate");
        //        }

        //        return new Application
        //        {
        //            Name = fullName,
        //            Status = StatusEnum.Delivered,
        //            DateAdd = applicationDate,
        //            DateEdit = DateTime.Now,
        //            File = file
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Failed to parse XML: {ex.Message}", ex);
        //    }
        //}

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _isDisposing = true;
            if (_listener.IsListening)
            {
                _logger.LogInformation("Stopping HttpListener during shutdown");
                _listener.Stop();
            }
            _listener.Close();
            _logger.LogInformation("HttpListener service stopped during application shutdown");
            await base.StopAsync(cancellationToken);
        }
    }
}

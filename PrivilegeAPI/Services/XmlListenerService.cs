using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.ComponentModel;
using System.Data.Entity;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
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
                await Task.Run(() => MonitorFtpFoldersAsync(stoppingToken), stoppingToken);

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

                        _logger.LogInformation("Application from {ClientIp} saved to database with ID {ApplicationId}", clientIp, application.Idgosuslug);
                    }

                    await _ftpService.SaveFileAsync(virtualPath, xmlContent);

                    dbContext.Files.Add(file);
                    await dbContext.SaveChangesAsync(cancellationToken);

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

        private async Task MonitorFtpFoldersAsync(CancellationToken cancellationToken)
        {
            var knownFiles = await LoadKnownFilesAsync();
            var knownFinals = await LoadKnownFinalsAsync();

            while (!cancellationToken.IsCancellationRequested && !_isDisposing)
            {
                try
                {
                    await _portalService.ConnectAsync();

                    var userLogsFiles = await _portalService.ListDirectoryAsync("UserLog");

                    foreach (var file in userLogsFiles)
                    {
                        var fileName = Path.GetFileName(file);

                        if (!fileName.Equals("log.htm", StringComparison.OrdinalIgnoreCase))
                            continue;

                        var localFolder = Path.Combine(AppContext.BaseDirectory, "Applications", "UserLog");
                        Directory.CreateDirectory(localFolder);
                        var localPath = Path.Combine(localFolder, fileName);

                        await _portalService.DownloadFileAsync($"UserLog/{fileName}", localPath);

                        var doc = XDocument.Load(localPath);
                        var rows = doc.Descendants("tr")
                                      .Where(x => x.Attribute("Type") != null)
                                      .ToList();

                        var fileLogEvents = new List<FileLogEvent>();

                        foreach (var row in rows)
                        {
                            var type = (string)row.Attribute("Type")!;
                            var idStr = row.Attribute("ID")?.Value ?? "";
                            var logFile = row.Attribute("File")?.Value ?? "";
                            var number = row.Attribute("Number")?.Value ?? "";

                            if (string.IsNullOrEmpty(idStr))
                                continue;

                            int id = int.Parse(idStr);

                            if (await ExistsInDatabaseAsync(id, type))
                                continue;

                            var fileLog = new FileLogEvent
                            {
                                Id = id,
                                File = logFile,
                                Number = number,
                                Type = type
                            };

                            await SaveFileLogEventAsync(fileLog.Id, fileLog.File, fileLog.Number, fileLog.Type);

                            fileLogEvents.Add(fileLog);
                        }

                        var completedIds = await CheckFileLogEventsAsync(fileLogEvents);

                        foreach (var appId in completedIds)
                        {
                            await _answerService.SendAnswerDeliveredAsync(appId.ToString());
                        }

                        await _ftpService.DeleteFileAsync(fileName);
                    }

                    var otherFolders = new[] { "Finali" };

                    foreach (var ftpFolder in otherFolders)
                    {
                        var files = await _portalService.ListDirectoryAsync(ftpFolder);

                        if (!knownFiles.TryGetValue(ftpFolder, out var knownSet))
                        {
                            knownSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                            knownFiles[ftpFolder] = knownSet;
                        }

                        foreach (var file in files)
                        {
                            var fileName = Path.GetFileName(file);

                            if (knownFinals.Contains(fileName) || knownSet.Contains(fileName))
                            {
                                continue;
                            }

                            _logger.LogInformation("Обработка файла {File}", fileName);

                            var localFolder = Path.Combine(AppContext.BaseDirectory, "Applications", ftpFolder);
                            Directory.CreateDirectory(localFolder);
                            var localPath = Path.Combine(localFolder, fileName);

                            await _portalService.DownloadFileAsync($"Finali/{fileName}", localPath);

                            var fileDb = new File
                            {
                                Name = fileName,
                                Path = localPath
                            };

                            var xdoc = XDocument.Load(localPath);
                            XNamespace ns = "http://www.w3.org/1999/xhtml";
                            var container = xdoc.Descendants(ns + "container").FirstOrDefault();

                            if (container == null)
                            {
                                _logger.LogWarning("Элемент <container> не найден в {File}", fileName);
                                await _ftpService.DeleteFileAsync(fileName);
                                continue;
                            }

                            var reg = container.Element(ns + "reg");
                            if (reg == null)
                            {
                                _logger.LogWarning("Элемент <reg> не найден в {File}", fileName);
                                await _ftpService.DeleteFileAsync(fileName);
                                continue;
                            }

                            // Пример стрктуры тэга <reg>:
                            //<reg>
                            //    <datareg> 29.08.2025 </datareg>
                            //    <regnumber> 01-14/07-5 </regnumber>
                            //    <vhodregnumber> 20250829143539 </vhodregnumber>
                            //    <vhodregdate> 29.08.2025 </vhodregdate>
                            //</reg>

                            var dataReg = reg.Element(ns + "datareg")?.Value;
                            var regNumber = reg.Element(ns + "regnumber")?.Value;
                            var vhodRegDate = reg.Element(ns + "vhodregdate")?.Value;
                            var vhodRegNumber = reg.Element(ns + "vhodregnumber")?.Value;

                            if (string.IsNullOrWhiteSpace(dataReg) ||
                                string.IsNullOrWhiteSpace(regNumber) ||
                                string.IsNullOrWhiteSpace(vhodRegDate) ||
                                string.IsNullOrEmpty(vhodRegNumber))
                            {
                                _logger.LogWarning("Ответ {File} отклонен (нет обязательных элементов)", fileName);
                                await _answerService.ProcessAnswerFileAsync(xdoc, fileDb);
                                await _ftpService.DeleteFileAsync(fileName);
                                continue;
                            }

                            var result = await _answerService.ProcessAnswerFileAsync(
                                xdoc, fileDb, xdoc, dataReg, regNumber, vhodRegNumber.Trim(), vhodRegDate);
                            if (!result)
                            {
                                _logger.LogWarning("Ответ {File} отклонен (ошибка обработки)", fileName);
                                await _ftpService.DeleteFileAsync(fileName);
                                continue;
                            }

                            await _ftpService.DeleteFileAsync(fileName);
                            knownSet.Add(fileName);
                            await SaveFinalEventAsync(fileName);
                        }
                    }

                    await _portalService.DisconnectAsync();

                    await _hubContext.Clients.All.SendAsync(
                        "ReceiveMessage",
                        $"All folders have been checked.",
                        cancellationToken
                    );
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Ошибка при проверке FTP");
                }

                await Task.Delay(TimeSpan.FromMinutes(5), cancellationToken);
            }
        }

        public async Task<List<int>> CheckFileLogEventsAsync(List<FileLogEvent> fileLogEvents)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var allLogs = fileLogEvents;

            var completedNumbers = new HashSet<string>();

            foreach (var givingLog in allLogs.Where(l => l.Type == "Giving" && !string.IsNullOrEmpty(l.Number)))
            {
                var receivingLog = allLogs.FirstOrDefault(l => l.Id == givingLog.Id && l.Type == "Receiving");
                completedNumbers.Add(givingLog.Number);
            }

            if (!completedNumbers.Any())
            {
                return new List<int>();
            }

            var applications = dbContext.Applications
                .Where(a => completedNumbers.Contains(a.Orgnumber))
                .ToList();

            var completedIds = applications.Select(a => a.Id).ToList();

            return completedIds;
        }

        private async Task<Dictionary<string, HashSet<string>>> LoadKnownFilesAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var processedFiles = dbContext.FileLogEvents
                .Select(f => new { f.Type, f.Number })
                .ToList();

            var knownFiles = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);

            foreach (var file in processedFiles)
            {
                if (!knownFiles.TryGetValue(file.Type, out var set))
                {
                    set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    knownFiles[file.Type] = set;
                }
                set.Add(file.Number);
            }

            return knownFiles;
        }

        private async Task<HashSet<string>> LoadKnownFinalsAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var processedFinals = dbContext.Finals
                .Select(f => f.Id)
                .ToList();

            return new HashSet<string>(processedFinals);
        }

        private async Task SaveFileLogEventAsync(int id, string file, string number, string type)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var logEvent = new FileLogEvent
            {
                Id = id,
                File = file,
                Number = number,
                Type = type
            };

            dbContext.FileLogEvents.Add(logEvent);
            await dbContext.SaveChangesAsync();
        }

        private async Task SaveFinalEventAsync(string id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var final = new Finals
            {
                Id = id
            };

            dbContext.Finals.Add(final);
            await dbContext.SaveChangesAsync();
        }

        public async Task<bool> ExistsInDatabaseAsync(int id, string type)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            return dbContext.FileLogEvents
                .Any(f => f.Id == id && f.Type == type);
        }

        private Application ParseXml(string xmlContent, File file)
        {
            try
            {
                XDocument doc = XDocument.Parse(xmlContent);

                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var htmlx = doc.Root
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

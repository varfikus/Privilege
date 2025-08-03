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
        private readonly IHubContext<XmlProcessingHub> _hubContext;
        private readonly ILogger<XmlListenerService> _logger;
        private readonly string _url = "http://+:5059/";
        private bool _isDisposing;

        public XmlListenerService(IServiceScopeFactory scopeFactory, IHubContext<XmlProcessingHub> hubContext, ILogger<XmlListenerService> logger)
        {
            _listener = new HttpListener();
            _scopeFactory = scopeFactory;
            _hubContext = hubContext;
            _logger = logger;
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
                _logger.LogInformation("HttpListener successfully started on {Url}", _url);

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

                if (request.HttpMethod == "POST" &&
                    (request.ContentType == "text/xml" || request.ContentType == "application/xml"))
                {
                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    var xmlContent = await reader.ReadToEndAsync(cancellationToken);
                    _logger.LogDebug("Received XML: {XmlContent}", xmlContent);

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    // Создаем запись о файле (с временным именем и путём)
                    string fileName = $"Application_{DateTime.Now:yyyyMMddHHmmss}.xml";
                    string virtualPath = $"Applications/{fileName}";

                    var file = new File
                    {
                        Name = fileName,
                        Path = virtualPath
                    };

                    dbContext.Files.Add(file);
                    await dbContext.SaveChangesAsync(cancellationToken); // получаем file.Id

                    var application = ParseXml(xmlContent, file);
                    application.FileId = file.Id;

                    dbContext.Applications.Add(application);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    // Сохраняем XML на FTP
                    await FtpService.SaveFileFtpAsync(
                        XDocument.Parse($"<root>{xmlContent}</root>"),
                        virtualPath
                    );

                    await _hubContext.Clients.All.SendAsync(
                        "ReceiveMessage",
                        $"XML processed from {clientIp}. ApplicationId: {application.Id}",
                        cancellationToken
                    );

                    // Ответ клиенту
                    var response = context.Response;
                    var responseString = $"{{\"Message\":\"XML processed and saved to database.\",\"ApplicationId\":{application.Id}}}";
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

                var persData = container
                    .Element(ns + "topheader")
                    ?.Element(ns + "tophead")
                    ?.Element(ns + "pers_data")
                    ?? throw new Exception("Missing <pers_data> element");

                string fam = persData.Element(ns + "fam")?.Value ?? "";
                string im = persData.Element(ns + "im")?.Value ?? "";
                string ot = persData.Element(ns + "ot")?.Value ?? "";

                string fullName = $"{fam} {im} {ot}".Trim();
                if (string.IsNullOrWhiteSpace(fullName))
                    throw new Exception("FullName is required");

                string benefitCategory = container
                    .Element(ns + "content")
                    ?.Element(ns + "lgota_text")
                    ?.Value ?? "";
                if (string.IsNullOrWhiteSpace(benefitCategory))
                    throw new Exception("BenefitCategory is required");

                string dateString = container.Element(ns + "dateblank")?.Value;
                if (!DateTime.TryParse(dateString, out DateTime applicationDate))
                {
                    throw new Exception("Invalid or missing ApplicationDate");
                }

                return new Application
                {
                    Name = fullName,
                    Status = benefitCategory,
                    DateAdd = applicationDate.ToString("yyyy-MM-dd"),
                    DateEdit = DateTime.Now.ToString("yyyy-MM-dd"),
                    File = file
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

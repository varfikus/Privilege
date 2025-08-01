using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Net;
using System.Text;
using System.Xml.Linq;

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
                _logger.LogError(ex, "Unexpected error starting HttpListener on {Url}", _url);
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
            {
                return;
            }

            try
            {
                var request = context.Request;
                var clientIp = context.Request.RemoteEndPoint?.Address.ToString();
                _logger.LogInformation("Received request from {ClientIp}", clientIp);

                // Optional: Restrict to specific IP
                // if (clientIp != "192.168.1.100")
                // {
                //     context.Response.StatusCode = 401;
                //     var error = Encoding.UTF8.GetBytes("{\"Error\":\"Unauthorized IP address\"}");
                //     context.Response.ContentLength64 = error.Length;
                //     context.Response.ContentType = "application/json";
                //     await context.Response.OutputStream.WriteAsync(error, 0, error.Length, cancellationToken);
                //     context.Response.Close();
                //     return;
                // }

                if (request.HttpMethod == "POST" && (request.ContentType == "text/xml" || request.ContentType == "application/xml"))
                {
                    using var reader = new System.IO.StreamReader(request.InputStream, request.ContentEncoding);
                    var xmlContent = await reader.ReadToEndAsync(cancellationToken);
                    _logger.LogDebug("Received XML: {XmlContent}", xmlContent);

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var application = ParseXml(xmlContent);
                    dbContext.Applications.Add(application);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"XML processed from {clientIp}. ApplicationId: {application.Id}", cancellationToken);

                    var response = context.Response;
                    var responseString = "{\"Message\":\"XML processed and saved to database.\",\"ApplicationId\":" + application.Id + "}";
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

        private Application ParseXml(string xmlContent)
        {
            XDocument doc = XDocument.Parse(xmlContent);
            XElement root = doc.Root;

            return new Application
            {
                Organization = root.Element("Organization")?.Value ?? "",
                FullName = root.Element("FullName")?.Value ?? "",
                Address = root.Element("Address")?.Value ?? "",
                PassportSeries = root.Element("PassportSeries")?.Value ?? "",
                PassportNumber = root.Element("PassportNumber")?.Value ?? "",
                IssuedBy = root.Element("IssuedBy")?.Value ?? "",
                ContactPhone = root.Element("ContactPhone")?.Value ?? "",
                Email = root.Element("Email")?.Value ?? "",
                CardNumber = root.Element("CardNumber")?.Value ?? "",
                BenefitCategory = root.Element("BenefitCategory")?.Value ?? "",
                ApplicationDate = DateTime.Parse(root.Element("ApplicationDate")?.Value ?? DateTime.Now.ToString()),
                ServiceId = root.Element("ServiceId")?.Value ?? ""
            };
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

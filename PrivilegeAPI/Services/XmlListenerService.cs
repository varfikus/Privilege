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
            {
                return;
            }

            try
            {
                var request = context.Request;
                var clientIp = context.Request.RemoteEndPoint?.Address.ToString();
                _logger.LogInformation("Received request from {ClientIp}", clientIp);

                if (request.HttpMethod == "POST" && (request.ContentType == "text/xml" || request.ContentType == "application/xml"))
                {
                    using var reader = new StreamReader(request.InputStream, request.ContentEncoding);
                    var xmlContent = await reader.ReadToEndAsync(cancellationToken);
                    _logger.LogDebug("Received XML: {XmlContent}", xmlContent);

                    using var scope = _scopeFactory.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var application = ParseXml(xmlContent);
                    dbContext.Applications.Add(application);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    // Save PDFs to FTP
                    await SaveXmlToFtp(xmlContent, application.Id);

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
            try
            {
                _logger.LogDebug("Parsing XML content: {XmlContent}", xmlContent);
                XDocument doc;
                try
                {
                    doc = XDocument.Parse(xmlContent);
                }
                catch (Exception ex)
                {
                    xmlContent = $"<root>{xmlContent}</root>";
                    doc = XDocument.Parse(xmlContent);
                    _logger.LogWarning("XML parsed as fragment with added root element");
                }

                XNamespace ns = "http://www.w3.org/1999/xhtml";
                XElement htmlx = doc.Element(ns + "htmlx");
                if (htmlx == null && doc.Element("root") != null)
                {
                    htmlx = doc.Element("root")?.Element(ns + "htmlx");
                }
                if (htmlx == null) throw new Exception("Missing htmlx element");

                XElement body2 = htmlx.Element(ns + "body2");
                if (body2 == null) throw new Exception("Missing body2 element");

                XElement container = body2.Element(ns + "container");
                if (container == null) throw new Exception("Missing container element");

                XElement topheader = container.Element(ns + "topheader")?.Element(ns + "tophead");
                XElement cardData = container.Element(ns + "card_data");

                XElement persData = topheader?.Element(ns + "pers_data");
                string fam = persData?.Element(ns + "fam")?.Value ?? "";
                string im = persData?.Element(ns + "im")?.Value ?? "";
                string ot = persData?.Element(ns + "ot")?.Value ?? "";
                string fullName = $"{fam} {im} {ot}".Trim();
                if (string.IsNullOrEmpty(fullName)) throw new Exception("FullName is required");

                string benefitCategory = container.Element(ns + "lgota_text")?.Value ?? "";
                if (string.IsNullOrEmpty(benefitCategory)) throw new Exception("BenefitCategory is required");

                string cardNumber = cardData?.Element(ns + "last_4_digits")?.Value ?? "";

                string dateString = container.Element(ns + "dateblank")?.Value;
                if (!DateTime.TryParse(dateString, out DateTime applicationDate))
                {
                    _logger.LogWarning("Invalid or missing ApplicationDate, using current date");
                    applicationDate = DateTime.Now;
                }

                XElement servinfo = body2.Element(ns + "servinfo");
                if (servinfo == null) throw new Exception("At least one servinfo element is required");

                string serviceId = servinfo.Element(ns + "idservice")?.Value ?? "";
                string serviceName = servinfo.Element(ns + "nameservice")?.Value ?? "";
                if (string.IsNullOrEmpty(serviceName)) throw new Exception("ServiceName is required");

                _logger.LogInformation("Parsed XML: FullName={FullName}, ServiceName={ServiceName}, ApplicationDate={ApplicationDate}, BenefitCategory={BenefitCategory}, CardNumber={CardNumber}, ServiceId={ServiceId}",
                    fullName, serviceName, applicationDate, benefitCategory, cardNumber, serviceId);

                return new Application
                {
                    FullName = fullName,
                    ServiceName = serviceName,
                    ApplicationDate = applicationDate,
                    BenefitCategory = benefitCategory,
                    CardNumber = cardNumber,
                    ServiceId = serviceId
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to parse XML");
                throw new Exception($"Failed to parse XML: {ex.Message}", ex);
            }
        }

        private async Task SaveXmlToFtp(string xmlContent, int applicationId)
        {
            try
            {
                _logger.LogInformation("Saving XML for ApplicationId {ApplicationId} to FTP", applicationId);
                string ftpServer = "ftp://192.168.1.100:21";
                string ftpUser = "ftp_user";
                string ftpPassword = "ftp_password";
                string ftpDirectory = "/Applications";
                string ftpPath = $"{ftpServer}{ftpDirectory}/{DateTime.Now:yyyyMMddHHmmss}_Application_{applicationId}.xml";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.UseBinary = true;

                byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlContent);
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(xmlBytes, 0, xmlBytes.Length);
                }

                using (FtpWebResponse response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    _logger.LogInformation("Uploaded XML for ApplicationId {ApplicationId} to FTP: {Status}", applicationId, response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save XML for ApplicationId {ApplicationId} to FTP", applicationId);
                throw;
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

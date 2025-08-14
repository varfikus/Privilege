using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using PrivilegeAPI.Services;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace PrivilegeAPI.Controllers
{
    [Route("api/xml-listener")]
    [ApiController]
    public class XmlListenerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<XmlProcessingHub> _hubContext;
        private readonly FtpService _ftpService;
        private readonly AnswerService _answerService;

        public XmlListenerController(ApplicationDbContext context, IHubContext<XmlProcessingHub> hubContext, FtpService ftpService, AnswerService answerService)
        {
            _context = context;
            _hubContext = hubContext;
            _ftpService = ftpService;
            _answerService = answerService;
        }

        //[HttpPost]
        //[Consumes("text/xml", "application/xml")]
        //public async Task<IActionResult> ReceiveXml([FromBody] string xmlContent)
        //{
        //    var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        //    // if (clientIp != "192.168.1.100")
        //    // {
        //    //     return Unauthorized(new { Error = "Request from unauthorized IP address." });
        //    // }

        //    try
        //    {
        //        var application = ParseXml(xmlContent);
        //        _context.Applications.Add(application);
        //        await _context.SaveChangesAsync();

        //        await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"XML processed from {clientIp}. ApplicationId: {application.Id}");

        //        return Ok(new { Message = "XML processed and saved to database.", ApplicationId = application.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = $"Error processing XML: {ex.Message}" });
        //    }
        //}

        [HttpPost]
        [Consumes("application/json", "text/plain")]
        public async Task<IActionResult> ReceiveXml([FromBody] Base64XmlRequest request)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(request.Base64Content))
            {
                return BadRequest(new { Error = "Данные не были переданы или пустые." });
            }

            try
            {
                byte[] xmlBytes = Convert.FromBase64String(request.Base64Content);
                string xmlContent = Encoding.UTF8.GetString(xmlBytes);

                var fileName = $"Application_{DateTime.Now:yyyyMMddHHmmss}.xml";
                var filePath = $"Applications/New/{fileName}";

                var file = new Models.File
                {
                    Name = fileName,
                    Path = filePath
                };

                await _ftpService.SaveFileAsync(file.Path, xmlContent);
                _context.Files.Add(file);
                await _context.SaveChangesAsync();

                var application = ParseXml(xmlContent, file);
                application.FileId = file.Id;

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                bool answer = _answerService.SendAnswerDeliveredAsync(application).Result;

                if (!answer)
                {
                    throw new Exception("Failed to send reply.");
                }

                await _hubContext.Clients.All.SendAsync("ReceiveMessage",
                    $"XML processed from {clientIp}. ApplicationId: {application.Id}");

                return Ok(new
                {
                    Message = "XML processed and saved to database.",
                    ApplicationId = application.Id
                });
            }
            catch (FormatException)
            {
                return BadRequest(new { Error = "Передана некорректная base64-строка." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = $"Ошибка обработки XML: {ex.Message}" });
            }
        }

        private Application ParseXml(string xmlContent, Models.File file)
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
                    Status = StatusEnum.Delivered,
                    DateAdd = applicationDate,
                    DateEdit = DateTime.Now,
                    File = file
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to parse XML: {ex.Message}", ex);
            }
        }
    }
}

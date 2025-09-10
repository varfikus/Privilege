using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Helpers;
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

                var signedDoc = await _answerService.SendToMedAsync(xmlContent);
                if (signedDoc == null)
                {
                    throw new Exception("Ошибка при формировании документа для MED.");
                }

                var application = ParseXml(signedDoc, file);
                application.FileId = file.Id;

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Received XML {application.Idgosuslug}");

                await _hubContext.Clients.All.SendAsync("ReceiveMessage",
                    $"XML processed from {clientIp}.");

                return Ok(new
                {
                    Status = "Ok"
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
    }
}

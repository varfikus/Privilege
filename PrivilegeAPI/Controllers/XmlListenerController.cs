using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Xml.Linq;

namespace PrivilegeAPI.Controllers
{
    [Route("api/xml-listener")]
    [ApiController]
    public class XmlListenerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<XmlProcessingHub> _hubContext;

        public XmlListenerController(ApplicationDbContext context, IHubContext<XmlProcessingHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
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
        [Consumes("text/xml", "application/xml")]
        public async Task<IActionResult> ReceiveXml([FromBody] string xmlContent)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            try
            {
                var file = new Models.File
                {
                    Name = $"file_{DateTime.Now:yyyyMMdd_HHmmss}.xml",
                    Path = "/virtual/path",
                };

                _context.Files.Add(file); 
                await _context.SaveChangesAsync();

                var application = ParseXml(xmlContent, file);
                application.FileId = file.Id;

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"XML processed from {clientIp}. ApplicationId: {application.Id}");

                return Ok(new
                {
                    Message = "XML processed and saved to database.",
                    ApplicationId = application.Id
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = $"Error processing XML: {ex.Message}" });
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

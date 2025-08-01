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

        [HttpPost]
        [Consumes("text/xml", "application/xml")]
        public async Task<IActionResult> ReceiveXml([FromBody] string xmlContent)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            // Optional: Restrict to specific IP
            // if (clientIp != "192.168.1.100")
            // {
            //     return Unauthorized(new { Error = "Request from unauthorized IP address." });
            // }

            try
            {
                var application = ParseXml(xmlContent);
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"XML processed from {clientIp}. ApplicationId: {application.Id}");

                return Ok(new { Message = "XML processed and saved to database.", ApplicationId = application.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = $"Error processing XML: {ex.Message}" });
            }
        }

        private Application ParseXml(string xmlContent)
        {
            try
            {
                XDocument doc;
                try
                {
                    doc = XDocument.Parse(xmlContent);
                }
                catch (Exception ex)
                {
                    xmlContent = $"<root>{xmlContent}</root>";
                    doc = XDocument.Parse(xmlContent);
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
                    applicationDate = DateTime.Now;
                }

                XElement servinfo = body2.Element(ns + "servinfo");
                if (servinfo == null) throw new Exception("At least one servinfo element is required");

                string serviceId = servinfo.Element(ns + "idservice")?.Value ?? "";
                string serviceName = servinfo.Element(ns + "nameservice")?.Value ?? "";
                if (string.IsNullOrEmpty(serviceName)) throw new Exception("ServiceName is required");

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
                throw new Exception($"Failed to parse XML: {ex.Message}", ex);
            }
        }
    }
}

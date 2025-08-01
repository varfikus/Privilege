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
    }
}

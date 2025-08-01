using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Data.Entity;
using System.Xml;
using System.Xml.Linq;

namespace PrivilegeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<XmlProcessingHub> _hubContext;

        public ApplicationsController(ApplicationDbContext context, IHubContext<XmlProcessingHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        [HttpPost]
        [Consumes("text/xml", "application/xml")]
        public async Task<IActionResult> SubmitApplication([FromBody] string xmlContent)
        {
            try
            {
                var application = ParseXml(xmlContent);
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"XML processed and saved to database. ApplicationId: {application.Id}");

                return Ok(new { Message = "XML processed and saved to database.", ApplicationId = application.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = $"Error processing XML: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Application>>> GetApplications()
        {
            try
            {
                return _context.Applications.ToList();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
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
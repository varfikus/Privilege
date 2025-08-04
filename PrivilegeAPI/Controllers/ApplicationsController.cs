using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Data.Entity;
using System.Net;
using System.Text;
using System.Threading.Channels;
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

        // GET: api/Applications
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

        // GET: api/Applications/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Application>> GetApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
                return NotFound();

            return application;
        }

        // POST: api/Applications
        [HttpPost]
        public async Task<ActionResult<Application>> CreateApplication(Application application)
        {
            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetApplication), new { id = application.Id }, application);
        }

        // PUT: api/Applications/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateApplication([FromBody] ApplicationDto updatedApplication)
        {
            try
            {
                var existingApplication = _context.Applications.FirstOrDefault(a => a.Id == updatedApplication.Id);
                if (updatedApplication == null || !ApplicationExists(updatedApplication.Id))
                {
                    return NotFound();
                }

                existingApplication.Status = (StatusEnum)updatedApplication.Status;
                existingApplication.DateEdit = DateTime.Now;

                await _context.SaveChangesAsync();
                await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"Application {updatedApplication.Id} updated");

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
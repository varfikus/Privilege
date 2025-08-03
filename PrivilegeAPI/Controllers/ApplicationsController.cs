using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivilegeAPI.Context;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Data.Entity;
using System.Net;
using System.Text;
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

        // POST: api/Applications
        //[HttpPost]
        //[Consumes("text/xml", "application/xml")]
        //public async Task<IActionResult> ReceiveXml([FromBody] string xmlContent)
        //{
        //    var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

        //    try
        //    {
        //        var application = ParseXml(xmlContent);
        //        _context.Applications.Add(application);
        //        await _context.SaveChangesAsync();

        //        //await SaveXmlToFtp(xmlContent, application.Id);

        //        await _hubContext.Clients.All.SendAsync("ReceiveMessage", $"XML processed from {clientIp}. ApplicationId: {application.Id}");
        //        return Ok(new { Message = "XML processed and saved to database.", ApplicationId = application.Id });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { Error = $"Error processing XML: {ex.Message}" });
        //    }
        //}

        //private Application ParseXml(string xmlContent, File file)
        //{
        //    try
        //    {
        //        xmlContent = $"<root>{xmlContent}</root>";
        //        XDocument doc = XDocument.Parse(xmlContent);

        //        XNamespace ns = "http://www.w3.org/1999/xhtml";

        //        var htmlx = doc.Root?.Element(ns + "htmlx")
        //                    ?? throw new Exception("Missing <htmlx> element");

        //        var body2 = htmlx.Element(ns + "body2")
        //                    ?? throw new Exception("Missing <body2> element");

        //        var container = body2.Element(ns + "container")
        //                        ?? throw new Exception("Missing <container> element");

        //        var persData = container
        //            .Element(ns + "topheader")
        //            ?.Element(ns + "tophead")
        //            ?.Element(ns + "pers_data")
        //            ?? throw new Exception("Missing <pers_data> element");

        //        string fam = persData.Element(ns + "fam")?.Value ?? "";
        //        string im = persData.Element(ns + "im")?.Value ?? "";
        //        string ot = persData.Element(ns + "ot")?.Value ?? "";

        //        string fullName = $"{fam} {im} {ot}".Trim();
        //        if (string.IsNullOrWhiteSpace(fullName))
        //            throw new Exception("FullName is required");

        //        string benefitCategory = container
        //            .Element(ns + "content")
        //            ?.Element(ns + "lgota_text")
        //            ?.Value ?? "";

        //        string cardNumber = container
        //            .Element(ns + "content")
        //            ?.Element(ns + "card_data")
        //            ?.Element(ns + "last_4_digits")
        //            ?.Value ?? "";

        //        string dateString = container.Element(ns + "dateblank")?.Value;
        //        if (!DateTime.TryParse(dateString, out DateTime applicationDate))
        //        {
        //            throw new Exception("Invalid or missing ApplicationDate");
        //        }

        //        var servinfo = body2.Element(ns + "servinfo")
        //                      ?? throw new Exception("Missing <servinfo> element");

        //        string serviceId = servinfo.Element(ns + "idservice")?.Value ?? "";
        //        string serviceName = servinfo.Element(ns + "nameservice")?.Value ?? "";

        //        if (string.IsNullOrWhiteSpace(serviceName))
        //            throw new Exception("ServiceName is required");

        //        // Создаем Application с привязкой к File
        //        return new Application
        //        {
        //            Name = fullName,
        //            Status = benefitCategory,
        //            DateAdd = applicationDate.ToString("yyyy-MM-dd"),
        //            DateEdit = DateTime.Now.ToString("yyyy-MM-dd"),
        //            File = file
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Failed to parse XML: {ex.Message}", ex);
        //    }
        //}


        private async Task SaveXmlToFtp(string xmlContent, int applicationId)
        {
            try
            {
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

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save XML to FTP: {ex.Message}", ex);
            }
        }
    }
}
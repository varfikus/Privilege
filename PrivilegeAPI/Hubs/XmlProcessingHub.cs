using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Context;
using PrivilegeAPI.Models;
using System.Xml.Linq;

namespace PrivilegeAPI.Hubs
{
    public class XmlProcessingHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public XmlProcessingHub(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task SendXml(string xmlContent)
        {
            try
            {
                var application = ParseXml(xmlContent);
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();
                await Clients.All.SendAsync("ReceiveMessage", "XML processed and saved to database.");
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("ReceiveMessage", $"Error processing XML: {ex.Message}");
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

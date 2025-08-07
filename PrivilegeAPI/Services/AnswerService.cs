using Microsoft.AspNetCore.SignalR;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using System.Net;
using System.Xml.Linq;
using File = System.IO.File;

namespace PrivilegeAPI.Services
{
    public class AnswerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FtpService _ftpService;

        public AnswerService(IServiceScopeFactory scopeFactory, FtpService ftpService)
        {
            _scopeFactory = scopeFactory;
            _ftpService = ftpService;
        }

        public async Task<bool> SendAnswerDeliveredAsync(Application app)
        {
            try
            {
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "template.xml");
                if (!File.Exists(templatePath))
                {
                    return false;
                }

                XDocument xdoc = XDocument.Load(templatePath);
                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var body2 = xdoc.Descendants(ns + "body2").FirstOrDefault();
                if (body2 == null)
                {
                    return false;
                }

                var container = body2.Element(ns + "container");
                if (container == null)
                {
                    return false;
                }

                var content = container.Element(ns + "content");
                if (content != null)
                {
                    content.RemoveAll();

                    var styleAttr = new XAttribute("style", "text-indent: 0cm;");

                    content.Add(
                            new XElement(ns + "p",
                                "Уважаемый(ая) " + app.Name + ". Ваша заявка была успешно доставлена в систему для дальнейшего рассмотрения.")
                            );
                }

                var executor = container.Element(ns + "executor");
                executor?.Element(ns + "executordate")?.SetValue(DateTime.Now.ToShortDateString());

                var docstatus = container.Element(ns + "docstatus");

                var reg = container.Element(ns + "reg");

                var servinfo = body2.Element(ns + "servinfo");
                if (servinfo != null)
                {
                    servinfo.Element(ns + "signaturesxml")?.SetValue(string.Empty);
                    servinfo.Element(ns + "timestampout")?.SetValue(DateTime.Now.ToString("o"));
                }

                string ftpFilePath = $"Applications/Reply/answer_{app.File.Name}";
                await _ftpService.SaveFileAsync(ftpFilePath, xdoc.ToString());
                await _ftpService.DisconnectAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }   
    }
}

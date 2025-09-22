using APB.CryptoLib;
using APB.CryptoLib.Asn1.Ocsp;
using FluentFTP.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Helpers;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using Serilog.Core;
using System.Buffers.Text;
using System.IO;
using System.Net;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Xml;
using System.Xml.Linq;
using File = System.IO.File;

namespace PrivilegeAPI.Services
{
    public class AnswerService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly PortalService _portalService;
        private readonly FtpService _ftpService;
        private readonly ILogger<AnswerService> _logger;

        public AnswerService(IServiceScopeFactory scopeFactory, PortalService portalService, FtpService ftpService, ILogger<AnswerService> logger)
        {
            _scopeFactory = scopeFactory;
            _portalService = portalService;
            _ftpService = ftpService;
            _logger = logger;
        }

        public async Task<bool> SendAnswerDeliveredAsync(Application app, XDocument xdoc)
        {
            try
            {
                string idgu = app.Idgosuslug;
                string param = "{\"serviceId\":\"" + idgu + "\", \"serviceResult\":1}";
                string signature = "";
                string filePath = "";

                List<MyCert> certs = Crypto.GelAllCertificates();
                if (certs == null || certs.Count == 0)
                {
                    _logger.LogError("Сертификаты не найдены");
                    return false;
                }

                foreach (var c in certs)
                {
                    if (c.ShortName == "Organizatsiya_1")
                    {
                        try
                        {
                            filePath = $"C:\\ftp\\dss\\{app.File.Name}";
                            File.Copy(app.File.Path, filePath, overwrite: true);
                        }
                        catch (Exception copyEx)
                        {
                            _logger.LogError(copyEx, "Ошибка при копировании файла");
                            return false;
                        }

                        string fileParam = PortalHelper.GetFileParam();

                        try
                        {
                            if (!string.IsNullOrEmpty(fileParam))
                            {
                                fileParam = fileParam.Replace("<container></container>", "<container>" + param + "</container>");

                                signature = PortalHelper.GetSignaturesFromStream(
                                    Crypto.FileSignCadesBes(PortalHelper.GenerateStreamFromString(fileParam), c));
                            }
                            else
                            {
                                using (Stream stream = PortalHelper.GenerateStreamFromString(param))
                                {
                                    signature = PortalHelper.GenerateStringFromStream(Crypto.FileSignCadesBes(stream, c));
                                }
                            }
                        }
                        catch (Exception signEx)
                        {
                            _logger.LogError(signEx, "Ошибка при подписании данных");
                            return false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(signature))
                {
                    _logger.LogError("Не удалось сформировать подпись. Отправка отменена.");
                    return false;
                }

                if (await PortalHelper.SendToPortalReply(param, signature, filePath))
                {
                    XmlDocument xdocS = PortalHelper.ToXmlDocument(xdoc);
                    await _ftpService.SaveFileAsync($"Applications/Final/Application_" + app.Orgnumber + ".xml}", xdocS.InnerXml);

                    return true;
                }
                else
                {
                    _logger.LogError("Ошибка при отправке сообщения. Параметры:\nparam={Param}\nsignatureLength={SignatureLen}\nfilePath={FilePath}",
                        param, signature?.Length, filePath);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке документа");
                return false;
            }
        }

        public async Task<bool> SendAnswerDeliveredAsync(string id)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var app = await dbContext.Applications
                    .Include(a => a.File)
                    .FirstOrDefaultAsync(a => a.Id == int.Parse(id));

                if (app == null)
                {
                    _logger.LogWarning($"Заявка с ID {id} не найдена");
                    return false;
                }

                string idgu = app.Idgosuslug;
                string param = "{\"serviceId\":\"" + idgu + "\", \"serviceResult\":1}";
                string signature = "";
                string filePath = "";
                XmlDocument xmlDocument = new XmlDocument();

                List<MyCert> cert = Crypto.GelAllCertificates();
                foreach (var c in cert)
                {
                    if (c.ShortName == "Organizatsiya_1")
                    {
                        xmlDocument.LoadXml(PortalHelper.DocGener());
                        xmlDocument = Crypto.FileSignCadesBesX(xmlDocument, c);
                        filePath = "C:\\ftp\\dss\\Уведомление_" + DateTime.Now.ToString("o").Replace(":", "") + ".xml";
                        PortalHelper.SaveStringToFile(xmlDocument.InnerXml, filePath);

                        string fileParam = PortalHelper.GetFileParam();
                        if (fileParam != "")
                        {
                            fileParam = fileParam.Replace("<container></container>", "<container>" + param + "</container>");
                            signature =
                                PortalHelper.GetSignaturesFromStream(
                                    Crypto.FileSignCadesBes(PortalHelper.GenerateStreamFromString(fileParam), c));
                        }
                        else
                        {
                            using (Stream stream = PortalHelper.GenerateStreamFromString(param))
                            {
                                signature = PortalHelper.GenerateStringFromStream(Crypto.FileSignCadesBes(stream, c));
                            }
                        }
                    }
                }

                if (await PortalHelper.SendToPortalNotification(param, signature, filePath))
                {
                    app.Status = StatusEnum.Delivered;
                    dbContext.Applications.Update(app);
                    await dbContext.SaveChangesAsync();

                    await _ftpService.SaveFileAsync($"Applications/Reply/Application_" + app.Orgnumber + ".xml}", xmlDocument.InnerXml);
                    await _ftpService.DisconnectAsync();

                    return true;
                }
                else
                {
                    _logger.LogWarning($"Ошибка при отправке сообщения");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при отправке ответа");
                return false;
            }
        }

        public async Task<string> SendToMedAsync(string xmlContent)
        {
            try
            {
                string fileName = $"{DateTime.Now:yyyyMMddHHmmss}";
                var filePath = $"C:\\ftp\\dss\\{fileName}.xml";

                XDocument xdoc = XDocument.Parse(xmlContent, LoadOptions.PreserveWhitespace);

                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var container = xdoc.Descendants(ns + "container").FirstOrDefault();
                if (container == null)
                {
                    _logger.LogWarning("Элемент <container> не найден");
                    return null;
                }

                var servInfo = xdoc.Descendants(ns + "servinfo").FirstOrDefault();
                var idgosuslug = servInfo?.Element(ns + "idgosuslug")?.Value;

                var reg = container.Element(ns + "reg");
                if (reg != null)
                {
                    reg.Element(ns + "datareg")?.SetValue(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                    reg.Element(ns + "regnumber")?.SetValue(fileName);

                    var uslugNumber = reg.Element(ns + "uslugnumber");
                    if (uslugNumber != null && !string.IsNullOrEmpty(idgosuslug))
                        uslugNumber.SetValue(idgosuslug);
                }

                if (container.Attribute("id") == null)
                    container.SetAttributeValue("id", "electronic-document");

                if (container.Attribute("style") == null)
                    container.SetAttributeValue("style", "display: none;");

                PortalHelper.SaveFile(xdoc, filePath);

                string signedXml = SignDocument(filePath);

                string ftpFilePath = $"/Ishodishie/{fileName}.xml";
                await _portalService.SaveFileAsync(ftpFilePath, signedXml);
                await _ftpService.SaveFileAsync($"Applications/Med/{fileName}.xml", signedXml);

                return signedXml;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при отправке документа в МЭД: {ex}");
                return null;
            }
        }

        public async Task<bool> SendToMedAsync(Application application)
        {
            try
            {
                string fileName = $"{DateTime.Now:yyyyMMddHHmmss}";
                var filePath = $"C:\\ftp\\dss\\{fileName}.xml";
                var xmlContent = PortalHelper.GetStringFromFile($"Applications/New/Application_{application.Orgnumber}.xml");

                XDocument xdoc = XDocument.Parse(xmlContent, LoadOptions.PreserveWhitespace);

                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var container = xdoc.Descendants(ns + "container").FirstOrDefault();
                if (container == null)
                {
                    _logger.LogWarning("Элемент <container> не найден");
                    return false;
                }

                var servInfo = xdoc.Descendants(ns + "servinfo").FirstOrDefault();
                var idgosuslug = servInfo?.Element(ns + "idgosuslug")?.Value;

                var reg = container.Element(ns + "reg");
                if (reg != null)
                {
                    reg.Element(ns + "datareg")?.SetValue(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                    reg.Element(ns + "regnumber")?.SetValue(fileName);

                    var uslugNumber = reg.Element(ns + "uslugnumber");
                    if (uslugNumber != null && !string.IsNullOrEmpty(idgosuslug))
                        uslugNumber.SetValue(idgosuslug);
                }

                if (container.Attribute("id") == null)
                    container.SetAttributeValue("id", "electronic-document");

                if (container.Attribute("style") == null)
                    container.SetAttributeValue("style", "display: none;");

                PortalHelper.SaveFile(xdoc, filePath);

                string signedXml = SignDocument(filePath);

                string ftpFilePath = $"/Ishodishie/{fileName}.xml";
                await _portalService.SaveFileAsync(ftpFilePath, signedXml);
                await _ftpService.SaveFileAsync($"Applications/Med/{fileName}.xml", signedXml);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при отправке документа в МЭД: {ex}");
                return false;
            }
        }

        public async Task<bool> ProcessAnswerFileAsync(XDocument xmlContent, Models.File file, XDocument xdoc, string dataReg, string regNumber, string vhodRegNumber, string vhodRegDate)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                var existingApp = await dbContext.Applications
                        .FirstOrDefaultAsync(a => a.Orgnumber == vhodRegNumber);

                dbContext.Files.Add(file);
                await dbContext.SaveChangesAsync();

                if (existingApp != null)
                {
                    existingApp.FileId = file.Id;
                    existingApp.File = file;
                    existingApp.DateEdit = DateTime.Now;
                    existingApp.Status = StatusEnum.Final;

                    var confirmationXml = FillConfirmationTemplate(existingApp);
                    XDocument signedDoc = XDocument.Parse(confirmationXml);

                    string ftpFilePath = $"/Ishodishie/Confirmation{existingApp.File.Name}";

                    if (await SendAnswerDeliveredAsync(existingApp, xdoc))
                    {
                        dbContext.Applications.Update(existingApp);
                        await dbContext.SaveChangesAsync();

                        if (await _portalService.SaveFileAsync(ftpFilePath, signedDoc.ToString()))
                        {
                            return true;
                        }
                        else
                        {
                            _logger.LogWarning($"Ошибка при отправке подтверждения в МЭД");
                            return false;
                        }
                    }
                    else
                    {
                        _logger.LogWarning($"Ошибка при отправке сообщения");
                        return false;
                    }
                }
                else
                {
                    _logger.LogWarning("Заявка с номером {VhodRegNumber} не найдена", vhodRegNumber);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке файла");
                return false;
            }
        }

        public async Task<bool> ProcessAnswerFileAsync(XDocument xmlContent, Models.File file)
        {
            try
            {
                var app = await SaveDeniedXml(xmlContent, file);
                if (app != null) 
                {
                    _logger.LogInformation("Обработан отказ {RegNumber}", app.Id);
                    
                }
                else if (app == null)
                {
                    _logger.LogInformation("Ошибка обработки отказа");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при обработке файла");
                return false;
            }
        }

        private async Task<DeniedApplication> SaveDeniedXml(XDocument xmlContent, Models.File file)
        {
            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var deniedApplication = new DeniedApplication
            {
                Status = DenyStatus.Add,
                DateEdit = DateTime.Now,
                Org = xmlContent.Descendants("kodorgout").FirstOrDefault()?.Value,
                Orgout = xmlContent.Descendants("kodorg").FirstOrDefault()?.Value,
                File = file,
            };

            dbContext.Files.Add(file);
            await dbContext.SaveChangesAsync();

            deniedApplication.FileId = file.Id;
            dbContext.DeniedApplications.Add(deniedApplication);
            await dbContext.SaveChangesAsync();

            var xdoc = XDocument.Parse(xmlContent.ToString());
            await _ftpService.SaveFileAsync($"DeniedApplications/Final/{file.Name}", xdoc.ToString());

            return deniedApplication;
        }

        public XmlDocument SignDocument(XmlDocument xmlDoc)
        {
            try
            {
                List<MyCert> certs = Crypto.GelAllCertificates();
                var cert = certs.FirstOrDefault(c => c.ShortName == "Organizatsiya_1");
                if (cert == null)
                    throw new Exception("Сертификат для подписи не найден");

                XmlDocument signedXml = Crypto.FileSignCadesBesX(xmlDoc, cert);

                return signedXml;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при подписании документа: {ex}");
                throw;
            }
        }

        public string SignDocument(string filepath)
        {
            try 
            {
                List<MyCert> certs = Crypto.GelAllCertificates();
                var cert = certs.FirstOrDefault(c => c.ShortName == "Organizatsiya_1");
                if (cert == null) throw new Exception("Сертификат не найден");

                XmlDocument xmlDoc = new XmlDocument { PreserveWhitespace = true };
                xmlDoc.Load(filepath);
                xmlDoc = Crypto.FileSignCadesBesX(xmlDoc, cert);

                var decl = xmlDoc.FirstChild as XmlDeclaration;
                if (decl == null)
                {
                    decl = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    xmlDoc.InsertBefore(decl, xmlDoc.DocumentElement);
                }

                using var ms = new MemoryStream();
                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = false
                };
                using (var writer = XmlWriter.Create(ms, settings))
                {
                    xmlDoc.Save(writer);
                }

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при подписании документа: {ex}");
                throw;
            }
        }

        //try
        //{
        //    List<MyCert> certs = Crypto.GelAllCertificates();
        //    var cert = certs.FirstOrDefault();

        //    if (cert == null)
        //    {
        //        _logger.LogWarning("Сертификат для подписи не найден");
        //        return document;
        //    }

        //    XmlDocument xmlDoc = new XmlDocument { PreserveWhitespace = true };
        //    using (var reader = document.CreateReader())
        //    {
        //        xmlDoc.Load(reader);
        //    }

        //    xmlDoc = Crypto.FileSignCadesBesX(xmlDoc, cert);

        //    using (var nodeReader = new XmlNodeReader(xmlDoc))
        //    {
        //        return XDocument.Load(nodeReader);
        //    }
        //}
        //catch (Exception ex)
        //{
        //    _logger.LogError($"Ошибка при подписании документа: {ex}");
        //    return document;
        //}
        //}

        private string FillConfirmationTemplate(Application app)
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "template", "confirmation.xml");
            var xdoc = XDocument.Load(templatePath);

            var root = xdoc.Element("html");
            if (root == null)
                throw new InvalidOperationException("Неверный шаблон подтверждения");

            root.Element("id")!.Value = Path.GetFileNameWithoutExtension(app.File.Name);
            root.Element("kodorg")!.Value = app.Orgout ?? string.Empty;
            root.Element("kodorgout")!.Value = app.Org ?? string.Empty;

            PortalHelper.SaveStringToFile(xdoc.ToString(), $"C:\\ftp\\dss\\Confirmation{Path.GetFileNameWithoutExtension(app.File.Name)}.xml");

            return xdoc.ToString();
        }

        public async Task<bool> SendDeny(DeniedApplicationDto application)
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, "template", "deny.xml");
            var xdoc = XDocument.Load(templatePath);

            var root = xdoc.Element("html");
            if (root == null)
                throw new InvalidOperationException("Неверный шаблон подтверждения");

            if (application.File?.Name == null || application.File.Name == "" || application.Orgout == null || application.Org == null)
            {
                _logger.LogWarning("Ошибка при отправке отказа: файл не содержит необходимых полей");
                return false;
            }

            root.Element("id")!.Value = Path.GetFileNameWithoutExtension(application.File.Name);
            root.Element("kodorg")!.Value = application.Orgout;
            root.Element("kodorgout")!.Value = application.Org;

            string ftpFilePath = $"/Ishodishie/Deny{application.File.Name}";

            return await _portalService.SaveFileAsync(ftpFilePath, xdoc.ToString());
        }
    }
}

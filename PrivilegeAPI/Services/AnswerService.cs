using FluentFTP.Helpers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using PrivilegeAPI.Context;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Hubs;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using Serilog.Core;
using System.IO;
using System.Net;
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

        public async Task<bool> SendAnswerDeliveredAsync(Application app)
        {
            try
            {
                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "template.xml");
                if (!File.Exists(templatePath))
                {
                    _logger.LogError($"Шаблон не найден: {templatePath}");
                    return false;
                }

                XDocument xdoc = XDocument.Load(templatePath);
                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var body2 = xdoc.Descendants(ns + "body2").FirstOrDefault();
                if (body2 == null)
                {
                    _logger.LogWarning("Элемент <body2> не найден в шаблоне");
                    return false;
                }

                var container = body2.Element(ns + "container");
                if (container == null)
                {
                    _logger.LogWarning("Элемент <container> не найден в шаблоне");
                    return false;
                }

                //var content = container.Element(ns + "content");
                //if (content != null)
                //{
                //    content.RemoveAll();
                //    content.Add(
                //        new XElement(ns + "p",
                //            "Уважаемый(ая) " + app.Name + ". Ваша заявка \"Получение льготы для АСОП\" была успешно доставлена в систему для дальнейшего рассмотрения.")
                //    );
                //}

                container.Element(ns + "executor")?.Element(ns + "executordate")?.SetValue(DateTime.Now.ToShortDateString());

                var servinfo = body2.Element(ns + "servinfo");
                if (servinfo != null)
                {
                    servinfo.Element(ns + "signaturesxml")?.SetValue(string.Empty);
                    servinfo.Element(ns + "timestampout")?.SetValue(string.Empty);
                }

                //XDocument signedDoc = SignDocument(xdoc);
                XDocument signedDoc = xdoc;

                string ftpFilePath = $"Applications/Reply/answer_{app.File.Name}";
                await _ftpService.SaveFileAsync(ftpFilePath, signedDoc.ToString());

                //await SendToPortalAsync("891", 1, "Уведомление доставлено", signedDoc.ToString(), signatureBytes, app.Id, app.File.Id);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при отправке документа: {ex}");
                return false;
            }
        }

        public async Task<XDocument> SendToMedAsync(string xmlContent)
        {
            try
            {
                string fileName = $"{DateTime.Now:yyyyMMddHHmmss}";
                XDocument xdoc = XDocument.Parse(xmlContent);
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
                    var dataReg = reg.Element(ns + "datareg");
                    if (dataReg != null) dataReg.Value = $"{DateTime.Now}";

                    var regNumber = reg.Element(ns + "regnumber");
                    if (regNumber != null) regNumber.Value = fileName;

                    var uslugNumber = reg.Element(ns + "uslugnumber");
                    if (uslugNumber != null && !string.IsNullOrEmpty(idgosuslug))
                    {
                        uslugNumber.Value = idgosuslug;
                    }
                }
                
                XDocument signedDoc = xdoc;

                string ftpFilePath = $"/Ishodishie/{fileName}.xml";
                await _portalService.SaveFileAsync(ftpFilePath, signedDoc.ToString());

                //await SendToPortalAsync("891", 1, "Уведомление доставлено", signedDoc.ToString(), signatureBytes, app.Id, app.File.Id);

                return xdoc; // возвращаем изменённый XML
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при отправке документа: {ex}");
                return null;
            }
        }
        //XDocument signedDoc = SignDocument(xdoc);
        
        public async Task SendToPortalAsync(string serviceId, int serviceResult, string message, string xmlContent, byte[] signatureBytes, int idDoc, int idFile)
        {
            string url = "https://uslugi.gospmr.org/serviceresponse.php";

            using var httpClient = new HttpClient();
            using var form = new MultipartFormDataContent();

            var paramsObj = new
            {
                serviceId = serviceId,
                serviceResult = serviceResult,
                message = message
            };
            string paramsJson = JsonSerializer.Serialize(paramsObj);
            form.Add(new StringContent(paramsJson, Encoding.UTF8, "application/json"), "params");

            form.Add(new ByteArrayContent(signatureBytes), "signature", "signature.p7s");

            var documentBytes = Encoding.UTF8.GetBytes(xmlContent);
            form.Add(new ByteArrayContent(documentBytes), "document", "Уведомление.xml");

            try
            {
                var response = await httpClient.PostAsync(url, form);
                string ans = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode && ans.Trim().ToLower() == "ok")
                {
                    string mes = $"ID документа:[{idDoc}] файл ID:[{idFile}] status: {ans}";
                    _logger.LogInformation(mes);
                }
                else
                {
                    string mes = $"ID документа:[{idDoc}] файл ID:[{idFile}] возникла ошибка: {ans}";
                    _logger.LogWarning(mes);
                }
            }
            catch (Exception ex)
            {
                string mes = $"ID документа:[{idDoc}] файл ID:[{idFile}] исключение при отправке: {ex.Message}";
                _logger.LogError(mes);
            }
        }

        public async Task<bool> ProcessFileAsync(string id, string kodorg, string kodorgout)
        {
            try
            {
                var xdoc = XDocument.Load("confirmation.xml");

                var root = xdoc.Root;
                if (root == null) return false;

                root.Element("id")?.SetValue(id);
                root.Element("kodorg")?.SetValue(kodorg);
                root.Element("kodorgout")?.SetValue(kodorgout);

                string fileName = $"Confirmation{id}.xml";
                string tempPath = Path.Combine(Path.GetTempPath(), fileName);
                xdoc.Save(tempPath);

                string ftpPath = $"/Ishodishie/{fileName}";
                await _portalService.SaveFileAsync(ftpPath, xdoc.ToString());

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке файла: {ex.Message}");
                return false;
            }
        }

        //public async Task ProcessFileAsync(string filePath)
        //{
        //    var httpClient = new HttpClient();
        //    //httpClient.BaseAddress = new Uri("http://192.168.69.236:5000");
        //    httpClient.BaseAddress = new Uri("http://localhost:5000");
        //    var apiClient = new MyHttpClient(httpClient);

        //    try
        //    {
        //        _logger.LogInformation("Начата обработка файла: {File}", filePath);

        //        var fileBytes = await File.ReadAllBytesAsync(filePath);
        //        var fileName = Path.GetFileName(filePath);

        //        if (Path.GetExtension(filePath).Equals(".xml", StringComparison.OrdinalIgnoreCase))
        //        {
        //            var xmlContent = await File.ReadAllTextAsync(filePath);
        //            var xdoc = XDocument.Parse(xmlContent);

        //            var privilegeElement = xdoc.Root?.Element("privilegeinfo");

        //            if (privilegeElement != null)
        //            {
        //                var idElement = privilegeElement.Element("id");
        //                if (idElement == null)
        //                {
        //                    throw new InvalidOperationException("В privilegeinfo отсутствует тег <id>");
        //                }

        //                if (!int.TryParse(idElement.Value, out var appId))
        //                {
        //                    throw new InvalidOperationException("Значение <id> некорректно: " + idElement.Value);
        //                }

        //                var virtualPath = Path.Combine("Applications", "Final", fileName).Replace("\\", "/");

        //                var file = new Models.File
        //                {
        //                    Name = fileName,
        //                    Path = virtualPath
        //                };

        //                await _portalService.SaveFileAsync(virtualPath, xmlContent);

        //                int fileId = await _portalService.AddToDbAsync(file);

        //                var applicationDto = new ApplicationDto
        //                {
        //                    Id = appId,
        //                    FileId = fileId,
        //                    Status = StatusEnum.Final,
        //                    DateEdit = DateTime.Now
        //                };

        //                var result = await apiClient.PutAsync<ApplicationDto, BaseResult<ApplicationDto>>("api/applications", applicationDto);

        //                var privId = appId;

        //                _logger.LogInformation("Отправка ответа в портал для приложения ID: {AppId}, файл ID: {FileId}", appId, fileId);
        //                //await SendToPortalAsync("891", 1, "Решение о предоставлении услуги", xmlContent, fileBytes, appId, privId);
        //            }
        //            else
        //            {
        //                var virtualPath = Path.Combine("DeniedApplications", "Final", fileName).Replace("\\", "/");

        //                var file = new Models.File
        //                {
        //                    Name = fileName,
        //                    Path = virtualPath
        //                };

        //                await _portalService.SaveFileAsync(virtualPath, xmlContent);

        //                int fileId = await _portalService.AddToDbAsync(file);

        //                var app = ParseXml(xmlContent, file);

        //                var applicationDto = new DeniedApplicationDto
        //                {
        //                    Status = DenyStatus.Add,
        //                    DateAdd = app.DateAdd,
        //                    DateEdit = DateTime.Now,
        //                    FileId = fileId,
        //                    File = file
        //                };

        //                var result = await apiClient.PostAsync<DeniedApplicationDto, BaseResult<DeniedApplicationDto>>("api/deniedapplications", applicationDto);
        //            }
        //        }
        //        else
        //        {
        //            _logger.LogWarning("Файл {File} не является XML, обработка пропущена", filePath);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Ошибка при обработке файла {File}", filePath);
        //        throw;
        //    }
        //}

        private DeniedApplication ParseXml(string xmlContent, Models.File file)
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

                return new DeniedApplication
                {
                    Status = DenyStatus.Add,
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

        public XDocument SignDocument(XDocument document)
        {
            try
            {
                List<MyCert> certs = Crypto.GelAllCertificates();
                var cert = certs.FirstOrDefault();

                if (cert == null)
                {
                    _logger.LogWarning("Сертификат для подписи не найден");
                    return document;
                }

                XmlDocument xmlDoc = new XmlDocument { PreserveWhitespace = true };
                using (var reader = document.CreateReader())
                {
                    xmlDoc.Load(reader);
                }

                xmlDoc = Crypto.FileSignCadesBesX(xmlDoc, cert);

                using (var nodeReader = new XmlNodeReader(xmlDoc))
                {
                    return XDocument.Load(nodeReader);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ошибка при подписании документа: {ex}");
                return document;
            }
        }
    }
}

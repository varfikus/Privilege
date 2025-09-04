using APB.CryptoLib.Asn1.Crmf;
using APB.CryptoLib.Crypto.Tls;
using FluentFTP;
using RestSharp;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PrivilegeAPI.Helpers
{
    public static class PortalHelper
    {
        /// <summary>
        /// Генерация XML-документа по шаблону с контентом.
        /// </summary>
        public static XmlDocument GenerateDocument(string templatePath, string content)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Шаблон не найден", templatePath);

            var cert = Crypto.GelAllCertificates().FirstOrDefault();
            bool canSign = cert != null;

            string file = File.ReadAllText(templatePath);

            file = file.Replace("<content>$$$</content>", $"<content>{content}</content>");
            file = file.Replace("<datedocexecutor>$$$</datedocexecutor>", $"<datedocexecutor>{DateTime.Now:g}</datedocexecutor>");
            file = file.Replace("<post>$$$</post>", canSign ? "<post></post>" : "<post>Документ не подписан</post>");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file);
            return xmlDoc;
        }

        /// <summary>
        /// Подписать XmlDocument и вернуть подпись как XElement.
        /// </summary>
        public static (XmlDocument signedDoc, XElement signature) SignDocument(XmlDocument doc, MyCert cert)
        {
            if (cert == null)
                throw new InvalidOperationException("Сертификат не найден");

            XmlDocument signedXml = Crypto.FileSignCadesBesX(doc, cert);

            var nsmgr = new XmlNamespaceManager(signedXml.NameTable);
            nsmgr.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");

            XmlNode signatureNode = signedXml.SelectSingleNode("//ds:Signature", nsmgr);
            if (signatureNode == null)
                throw new Exception("Подпись не найдена");

            XElement signatureElement = XElement.Parse(signatureNode.OuterXml);

            XmlNode signaturesNode = signedXml.SelectSingleNode("//servinfo/signaturesxml");
            if (signaturesNode == null)
            {
                signaturesNode = signedXml.GetElementsByTagName("signaturesxml")?.Cast<XmlNode>().FirstOrDefault();
                if (signaturesNode == null)
                    throw new Exception("Узел <servinfo><signaturesxml> не найден");
            }

            signaturesNode.InnerXml = signatureNode.OuterXml;

            signatureNode.ParentNode?.RemoveChild(signatureNode);

            return (signedXml, signatureElement);
        }

        /// <summary>
        /// Вставляет подпись в шаблон param.xml
        /// </summary>
        public static XDocument InsertSignatureToTemplate(string templatePath, XElement signature)
        {
            if (!File.Exists(templatePath))
                throw new FileNotFoundException("Шаблон param.xml не найден", templatePath);

            XDocument template = XDocument.Load(templatePath);
            XNamespace ns = "http://www.w3.org/1999/xhtml";

            var signaturesXml = template.Descendants().FirstOrDefault(e => e.Name.LocalName == "signaturesxml");
            if (signaturesXml == null)
                throw new InvalidOperationException("<signaturesxml> не найден в шаблоне");

            signaturesXml.RemoveAll();
            if (signature != null)
                signaturesXml.Add(signature);

            return template;
        }

        public static XDocument ToXDocument(XmlDocument xmlDoc)
        {
            using (var nodeReader = new XmlNodeReader(xmlDoc))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static XmlDocument ToXmlDocument(XDocument xDoc)
        {
            var xmlDoc = new XmlDocument();
            using (var reader = xDoc.CreateReader())
            {
                xmlDoc.Load(reader);
            }
            return xmlDoc;
        }

        /// <summary>
        /// Конвертирует XDocument или XmlDocument в байты UTF-8
        /// </summary>
        public static byte[] ToBytes(XmlDocument doc)
        {
            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms, new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = false });
            doc.Save(writer);
            return ms.ToArray();
        }

        public static byte[] ToBytes(XDocument doc)
        {
            using var ms = new MemoryStream();
            using var writer = XmlWriter.Create(ms, new XmlWriterSettings { Encoding = new UTF8Encoding(false), Indent = false });
            doc.Save(writer);
            return ms.ToArray();
        }

        /// <summary>
        /// Отправка документа на портал через form-urlencoded
        /// </summary>
        public static async Task<bool> SendToPortalAsync(string url, string param, string paramSignature, string file)
        {
            try
            {
                using var httpClient = new HttpClient();

                var values = new Dictionary<string, string>
            {
                { "params", param },
                { "signature", paramSignature },
                { "document", file }
            };

                using var content = new FormUrlEncodedContent(values);

                var response = await httpClient.PostAsync(url, content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Преобразование Stream в строку
        /// </summary>
        public static string GenerateStringFromStream(Stream stream)
        {
            var reader = new StreamReader(stream, Encoding.UTF8);
            try
            {
                return reader.ReadToEnd();
            }
            finally
            {
                reader.Dispose();
            }
        }

        /// <summary>
        /// Преобразование строки в Stream
        /// </summary>
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Чтение файла в строку
        /// </summary>
        public static string GetStringFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            string text = File.ReadAllText(path);
            return text;
        }

        /// <summary>
        /// Чтение файла в строку
        /// </summary>
        public static string GetStringFromFile(XDocument xdoc)
        {
            return xdoc.ToString();
        }

        /// <summary>
        /// Вырезка содержимого подписи из XML документа
        /// </summary>
        public static string GetSignaturesFromStream(Stream stream)
        {
            string doc = GenerateStringFromStream(stream);
            string startText = "<signaturesxml>";
            string endText = "</signaturesxml>";
            int startIndex = doc.IndexOf(startText);
            int endIndex = doc.IndexOf(endText);

            if (startIndex == -1 || endIndex == -1)
                return "";

            startIndex += startText.Length;
            return doc.Substring(startIndex, endIndex - startIndex);
        }

        public static string DocGener()
        {
            string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "template.xml");
            string file = GetStringFromFile(templatePath);

            string content = "<p>Ваша заявка \"Получение льготы для АСОП\" была успешно доставлена в систему для дальнейшего рассмотрения.</p>";
            string date = DateTime.Now.ToString("g");
            file = file.Replace("<content>$$$</content>", "<content>" + content + "</content>");
            file = file.Replace("<datedocexecutor>$$$</datedocexecutor>", $"<datedocexecutor>" + date + "</datedocexecutor>");

            return file;
        }

        public static string GetFileParam()
        {
            string paramPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "param.xml");
            if (!System.IO.File.Exists(paramPath))
                return "";
            return GetStringFromFile(paramPath);
        }

        public static async Task<bool> SendToPortalNotification(string param, string paramSignature, string path)
        {
            string url = "https://uslugi.gospmr.org/serviceresponse.php";
            string file = GetStringFromFile(path);
            string text = "Уведомление.xml";
            string ans = Post(url, param, paramSignature, file, text);
            Console.WriteLine(ans);
            if (ans == "ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static async Task<bool> SendToPortalReply(string param, string paramSignature, string path)
        {
            string url = "https://uslugi.gospmr.org/serviceresponse.php";
            string file = GetStringFromFile(path);
            string text = "Решение.xml";
            string ans = Post(url, param, paramSignature, file, text);
            Console.WriteLine(ans);
            if (ans == "ok")
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// POST запрос
        /// </summary>
        /// <param name="url">POST-запрос по следуюзему URL</param>
        /// <param name="_params">Параметры выполнения услуги (JSON строка)</param>
        /// <param name="signature">Электронная цифровая подпись</param>
        /// <param name="file">Документ, предоставляемый при подтверждении</param>
        /// <returns></returns>
        private static string Post(string url, string _params, string signature, string file, string text)
        {
            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "multipart/form-data");
                request.AddParameter("params", _params);
                request.AddParameter("signature", signature);
                request.AddFile("document", Encoding.UTF8.GetBytes(file), text, "text/xml");
                return client.Execute(request).Content;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Возникла ошибка при отправке документа: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Сохранить текст в XML-файл
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="path">Путь сохранения</param>
        /// <returns></returns>
        public static bool SaveStringToFile(string text, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                try
                {
                    sw.WriteLine(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static bool SaveFile(XDocument xdoc, string path)
        {
            try
            {
                string? directory = Path.GetDirectoryName(path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var writer = new StreamWriter(path, false, new UTF8Encoding(false)))
                {
                    xdoc.Save(writer, SaveOptions.DisableFormatting);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении файла: {ex.Message}");
                return false;
            }
        }
    }
}

using APB.CryptoLib.Cades.Upgraders;
using APB.CryptoLib.GSS;
using APB.CryptoLib.Xades;
using APB.CryptoLib.Xades.Crypto;
using APB.CryptoLib.Xades.Signature.Parameters;
using PrivilegeAPI.Services;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Xml;

namespace PrivilegeAPI
{
    public static class Crypto
    {
        private static readonly ILogger Logger;

        static Crypto()
        {
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            });
            Logger = loggerFactory.CreateLogger("CryptoLogger");
        }

        /// <summary>
        /// Позволяет получить все доступные сертифекаты, которые валидны на текущий момент
        /// </summary>
        /// <returns>Список сертифекатов</returns>
        public static List<MyCert> GelAllCertificates()
        {
            List<MyCert> spisok = new List<MyCert>();

            try
            {
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);

                try
                {
                    X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByTimeValid, DateTime.Now, false);

                    foreach (X509Certificate2 certificate in certCollection)
                    {
                        if (certificate.HasPrivateKey)
                        {
                            MyCert cert = new MyCert
                            {
                                Name = CertToFullString(certificate),
                                Thumbprint = certificate.Thumbprint,
                                Cert = certificate
                            };
                            cert.SetRootThumbprint(Get_Thumbprint(certificate));
                            if (certificate.Verify())
                                cert.IsValid = true;
                            else
                                cert.IsValid = false;
                            cert.ShortName = CertToShotString(certificate);
                            spisok.Add(cert);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Ошибка при перечислении сертификатов: " + ex.Message);
                }
                finally
                {
                    store.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Возникла ошибка при попытке обращения к хранилишу сертифекатов: " + ex.Message);
            }

            return spisok;
        }
        /// <summary>
        /// Позволяет выбрать из хранилища сертификат, соотвествующий выбранному сертификату
        /// </summary>
        /// <param name="seleCert">Выбранный сертифекат</param>
        /// <returns>Сертифекат их хранилища</returns>
        private static X509Certificate2 GetCertificate(MyCert seleCert)
        {
            try
            {
                X509Store store = new X509Store(StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly);
                try
                {
                    if (seleCert != null)
                    {
                        X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindByThumbprint, seleCert.Thumbprint, false);

                        if (certCollection.Count > 0)
                            return certCollection[0];
                    }

                    Logger.LogInformation("Выбранный сертифекат не найден");
                    return null;
                }
                catch (Exception ex)
                {
                    Logger.LogError("Возникла ошибка при попытке обращения к хранилишу сертифекатов: " + ex.Message);
                    return null;
                }
                finally
                {
                    store.Close();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Возникла ошибка при попытке обращения к хранилишу сертифекатов: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Подписыват данные из файла выбранным сертифекатом
        /// </summary>
        /// <param name="fileToSign">Ссылка на файл с данными для подписания</param>
        /// <param name="seleCert">Выбранный сертифекат</param>
        /// <param name="oldSignature">Старая подпись</param>
        /// <returns>Подпись к данным</returns>
        public static Stream FileSignCadesBes(Stream xmlDocument, MyCert seleCert, bool isFirst = true, bool isParallelSignature = false)
        {
            X509Certificate2 certificate = GetCertificate(seleCert);
            Stream signedDocument = new MemoryStream();
            SignatureFormat cSignatureFormat = SignatureFormat.CAdES_BES;

            try
            {
                if (!isParallelSignature)
                {
                    if (isFirst)
                    {
                        signedDocument = GssSign.SignXml(xmlDocument, certificate, DigestMethod.SHA1, cSignatureFormat, isParallelSignature, null);
                    }
                    else
                    {
                        signedDocument = GssSign.CoSignXml(xmlDocument, certificate, DigestMethod.SHA1, cSignatureFormat, null);
                    }
                }
                else
                {
                    signedDocument = GssSign.SignXml(xmlDocument, certificate, DigestMethod.SHA1, cSignatureFormat, isParallelSignature, null);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Возникла ошибка при попытке осуществить поддписание документа: " + ex.Message);
                xmlDocument.Seek(0, SeekOrigin.Begin);
                return xmlDocument;
            }

            return signedDocument;
        }

        /// <summary>
        /// Подписыват данные из файла выбранным сертифекатом
        /// </summary>
        /// <param name="fileToSign">Ссылка на файл с данными для подписания</param>
        /// <param name="seleCert">Выбранный сертифекат</param>
        /// <param name="oldSignature">Старая подпись</param>
        /// <returns>Подпись к данным</returns>
        public static XmlDocument FileSignCadesBesX(XmlDocument xmlDocument, MyCert cert, string TSPadres = null, string Packaging = null, string ID = null)
        {
            X509Certificate2 certificate = GetCertificate(cert);

            APB.CryptoLib.Xades.Signature.Parameters.SignaturePackaging signaturePackaging = APB.CryptoLib.Xades.Signature.Parameters.SignaturePackaging.ENVELOPED;

            APB.CryptoLib.Xades.Upgraders.SignatureFormat signatureFormat = APB.CryptoLib.Xades.Upgraders.SignatureFormat.XAdES_BES;
            SignatureMethod signatureMethod = SignatureMethod.RSAwithSHA256;

            TSPadres = "http://ca.agroprombank.com/tsa";
            if (TSPadres != null) signatureFormat = APB.CryptoLib.Xades.Upgraders.SignatureFormat.XAdES_T;

            string signatureID = "electronic-document";
            if (ID != null) signatureID = ID;

            try
            {
                XmlDocument signedDocument = XadesSign.SignXml(xmlDocument, certificate, signaturePackaging, signatureMethod, signatureFormat, TSPadres, signatureID);
                return signedDocument;
            }
            catch (Exception ex)
            {
                throw new Exception("Возникла ошибка при попытке осуществить поддписание документа: " + ex.Message);
            }
        }

        /// <summary>
        /// Получает короткую строку описывающую сертифекат
        /// </summary>
        /// <param name="myCert">Сертификат</param>
        /// <returns>Короткая строка, связанная с сертифекатом</returns>
        private static string CertToFullString(X509Certificate2 myCert)
        {
            try
            {
                string outString = "";
                string subname = myCert.SubjectName.Name;

                string O = Get_Value(subname, "O");
                string CN = Get_Value(subname, "CN");
                string V = myCert.NotAfter.ToString("dd.MM.yyyy hh:mm:ss");

                if (O != null) outString = CN + "; " + O;
                else outString = CN;

                outString = outString + " (Действует до: " + V + ")";

                return outString;
            }
            catch
            {
                return "Ошибка получения данных о сертифекате";
            }

        }
        /// <summary>
        /// Получает короткую строку описывающую сертифекат
        /// </summary>
        /// <param name="myCert">Сертификат</param>
        /// <returns>Короткая строка, связанная с сертифекатом</returns>
        private static string CertToShotString(X509Certificate2 myCert)
        {
            try
            {
                string subname = myCert.SubjectName.Name;
                return Get_Value(subname, "CN");
            }
            catch
            {
                return "Ошибка получения данных о сертифекате";
            }

        }
        /// <summary>
        /// Получает значение конкретного атрибута из списка атрибутов, хранящихся в свойстве сертифеката SubjectName
        /// </summary>
        /// <param name="allText">Все атрибуты, хранящиеся в свойстве SubjectName</param>
        /// <param name="paramName">Имя искомого атрибута (CN, T, OU, O, STREET, E, L, C, S, SN) </param>
        /// <returns>Значение атрибута</returns>
        public static string Get_Value(string allText, string paramName)
        {
            try
            {
                Regex regex = new Regex("( " + paramName + "|^" + paramName + ")" + "=\".*\"");
                MatchCollection matches = regex.Matches(allText);
                if (matches.Count > 0)
                    return matches[0].Value.Replace("\"", "").Split('=')[1];

                regex = new Regex("( " + paramName + "|^" + paramName + ")" + "=[^,]+");
                matches = regex.Matches(allText);
                if (matches.Count > 0)
                    return matches[0].Value.Split('=')[1];
            }
            catch
            {
                return "unknown";
            }

            return null;
        }

        /// <summary>
        /// Получает значение конкретного атрибута из списка атрибутов, хранящихся в свойстве сертифеката SubjectName
        /// </summary>
        /// <param name="allText">Все атрибуты, хранящиеся в свойстве SubjectName</param>
        /// <param name="paramName">Имя искомого атрибута (CN, T, OU, O, STREET, E, L, C, S, SN) </param>
        /// <returns>Значение атрибута</returns>
        public static string Get_Thumbprint(X509Certificate2 certificate)
        {
            try
            {
                X509Chain ch = new X509Chain();
                ch.Build(certificate);
                return ch.ChainElements[ch.ChainElements.Count - 1].Certificate.Thumbprint;
            }
            catch
            {
                return "";
            }
        }
    }
}

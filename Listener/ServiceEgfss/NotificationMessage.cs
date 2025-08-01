using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using FluentFTP;
using MySql.Data.MySqlClient;
using RestSharp;

namespace ServiceMinsoc
{
    class NotificationMessage
    {
        /// <summary>
        /// Уведомление на портал, что заявление пришло в систему
        /// </summary>
        public NotificationMessage()
        {
            Logger.InitLogger();
        }

        #region Переменные

        /// <summary>
        /// ID документа
        /// </summary>
        public int IdDoc { get; set; }
        /// <summary>
        /// ID госуслуги
        /// </summary>
        public string IdGosulug { get; set; }
        /// <summary>
        /// ID услуги
        /// </summary>
        public string IdService { get; set; }
        /// <summary>
        /// Наименование услуги
        /// </summary>
        public string NameUslug { get; set; }
        /// <summary>
        /// ФИО заявителя
        /// </summary>
        public string Fio { get; set; }
        /// <summary>
        /// ID файла уведомления
        /// </summary>
        public int IdFileNotification { get; private set; }
        /// <summary>
        /// Отправлен ли файл на портал?
        /// </summary>
        public bool IsPostEnable = false;

        private bool _fileSendPost = false;

        #endregion

        public void UpdateStatusDocument(string idDocument)
        {
            int fileId = SaveArrivedFile();
            IdFileNotification = fileId;
            UpdateArrivedMySql(idDocument, fileId);
        }

        /// <summary>
        /// Генерация и заполнение ответа, что документ был доставлен оператору
        /// </summary>
        /// <param name="nameUsl">Наименование услуги</param>
        /// <param name="fio">ФИО заявителя</param>
        private string DocGener(string nameUsl, string fio)
        {
            string template = "status1.xml";   //нужна ли проверка?
            string file = MyStatic.GetStringFromFile(template);

            string content = "<p>Уважаемый(ая) " + fio + "</p><p>Ваше заявление на предоставление услуги " + nameUsl +
                             " доставлено в информационную систему Министерства по социальной защите и труду " +
                             "Приднестровской Молдавской Республики для дальнейшей обработки.</p>";
            file = file.Replace("<content>$$$</content>", "<content>" + content + "</content>");
            file = file.Replace("<datedocexecutor>$$$</datedocexecutor>", "<datedocexecutor>" + DateTime.Now.ToString("g") + "</datedocexecutor>");
            file = file.Replace("<post>$$$</post>", Properties.Settings.Default.CryptoSert == "" ? "<post>Документ не подписан</post>" : "<post></post>");

            return file;
        }

        /// <summary>
        /// Формирование документа
        /// </summary>
        /// <returns></returns>
        private XDocument DocGenerXml()
        {
            XDocument xdoc = XDocument.Load(@"status1.xml");
            XNamespace ns = xdoc.Root?.GetDefaultNamespace();
            if (ns == null)
            {
                Logger.Log.Warn("Ошибка при формировании документа (Не удалось получить корневой элемент)");
                return null;
            }
            XElement body = xdoc.Root?.Element(ns + "body2");
            if (body != null)
            {
                XElement container = body.Element(ns + "container");
                if (container != null)
                {
                    #region Основная часть

                    XElement header = container.Element(ns + "header");
                    if (header != null)
                    {
                        header.Value = "";
                    }

                    XElement content = container.Element(ns + "content");
                    if (content != null)
                    {
                        #region content

                        content.Value = "";
                        switch (IdService)
                        {
                            case "297":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Присвоение правового статуса ребенку-сироте, " +
                                        "ребенку, оставшемуся без попечения родителей, лицу из числа детей-сирот и детей, " +
                                        "оставшихся без попечения родителей» доставлено в информационную систему Министерства " +
                                        "по социальной защите и труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "301":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Назначение граждан опекунами, попечителями " +
                                        "несовершеннолетних детей» доставлено в информационную систему Министерства по социальной " +
                                        "защите и труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "302":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача органом опеки и попечительства решения об " +
                                        "объявлении несовершеннолетнего, достигшего возраста 16(шестнадцати) лет, полностью " +
                                        "дееспособным» доставлено в информационную систему Министерства по социальной защите и " +
                                        "труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "304":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача органом опеки и попечительства " +
                                        "разрешения на заключение трудовых договоров с несовершеннолетними» доставлено в " +
                                        "информационную систему Министерства по социальной защите и труду Приднестровской " +
                                        "Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "305":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача разрешения органом опеки и " +
                                        "попечительства на изменение имени и фамилии ребенку (детям)» доставлено в " +
                                        "информационную систему Министерства по социальной защите и труду Приднестровской " +
                                        "Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "307":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Постановка на учет в качестве кандидатов " +
                                        "в усыновители, опекуны (попечители), граждан, выразивших желание принять ребенка " +
                                        "на воспитание в свою семью» доставлено в информационную систему Министерства по " +
                                        "социальной защите и труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "308":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача разрешения на посещение ребенка, оставшегося " +
                                        "без попечения родителей» доставлено в информационную систему Министерства по социальной " +
                                        "защите и труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "309":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Временная передача детей, находящихся в организациях " +
                                        "обеспечивающих содержание, образование и воспитание детей-сирот и детей, оставшихся без " +
                                        "попечения родителей, в семьи граждан, постоянно проживающих на территории Приднестровской " +
                                        "Молдавской Республики» доставлено в информационную систему Министерства по социальной защите " +
                                        "и труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "314":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Установление патронажа над совершеннолетними " +
                                        "дееспособными гражданами, которые по состоянию здоровья не могут самостоятельно " +
                                        "осуществлять и защищать свои права и исполнять свои обязанности» доставлено в " +
                                        "информационную систему Министерства по социальной защите и труду Приднестровской " +
                                        "Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "318":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача разрешений на выезд из Приднестровской " +
                                        "Молдавской Республики несовершеннолетних граждан Приднестровской Молдавской Республики, " +
                                        "оставшихся без попечения родителей, в том числе находящихся в организациях для " +
                                        "детей-сирот и детей, оставшихся без попечения родителей» доставлено в информационную " +
                                        "систему Министерства по социальной защите и труду Приднестровской Молдавской " +
                                        "Республики и ожидает приема в работу."));
                                break;
                            case "319":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача органом опеки и попечительства " +
                                        "разрешения (согласия) на совершение сделок, связанных с имуществом, принадлежащим " +
                                        "подопечному» доставлено в информационную систему Министерства по социальной " +
                                        "защите и труду Приднестровской Молдавской Республики и ожидает приема в работу."));
                                break;
                            case "321":
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + "."),
                                    new XElement(ns + "p",
                                        "Ваше заявление на предоставление услуги «Выдача согласия на приватизацию жилого " +
                                        "дома, жилого помещения, в которых проживают и зарегистрированы совершеннолетние " +
                                        "недееспособные или не полностью дееспособные граждане, несовершеннолетние " +
                                        "дети-сироты, дети, оставшиеся без попечения родителей» доставлено в информационную " +
                                        "систему Министерства по социальной защите и труду Приднестровской Молдавской " +
                                        "Республики и ожидает приема в работу."));
                                break;
                            default:
                                content.Add(
                                    new XElement(ns + "p",
                                        "Уважаемый(ая) " + Fio + ". Ваше заявление на предоставление услуги «" + NameUslug +
                                        "» доставлено в информационную систему Министерства по социальной защите и труду " +
                                        "Приднестровской Молдавской Республики для дальнейшей обработки."));
                                break;
                        }

                        #endregion
                    }

                    XElement executor = container.Element(ns + "executor");
                    if (executor != null)
                    {
                        XElement executorname = executor.Element(ns + "executorname");
                        if (executorname != null)
                            executorname.Value = "";
                        XElement executorphone = executor.Element(ns + "executorphone");
                        if (executorphone != null)
                            executorphone.Value = "";
                        XElement executordate = executor.Element(ns + "executordate");
                        if (executordate != null)
                            executordate.Value = ""; //DateTime.Now.ToString("g");
                    }
                    XElement docstatus = container.Element(ns + "docstatus");
                    if (docstatus != null)
                    {
                        XElement datedocexecutor = docstatus.Element(ns + "datedocexecutor");
                        if (datedocexecutor != null)
                        {
                            datedocexecutor.Value = "";
                            datedocexecutor.Add(DateTime.Now.ToString("g"));
                        }
                    }

                    #endregion
                }
                else
                {
                    Logger.Log.Warn("Ошибка при формировании документа (Не удалось получить тег контейнера)");
                    return null;
                }

                #region Servinfo

                XElement servinfo = body.Element(ns + "servinfo");
                if (servinfo != null)
                {
                    XElement idgosuslug = servinfo.Element(ns + "idgosuslug");
                    if (idgosuslug == null)
                    {
                        idgosuslug = new XElement(
                            ns + "idgosuslug",
                            IdGosulug,
                            new XAttribute("style", "display: none !important;"));
                        servinfo.Add(idgosuslug);
                    }
                    else
                    {
                        idgosuslug.Value = IdGosulug;
                        idgosuslug.Add(new XAttribute("style", "display: none !important;"));
                    }
                }
                else
                {
                    Logger.Log.Warn("Ошибка при формировании документа (Не удалось получить тег сервисной информации)");
                    return null;
                }

                #endregion
            }
            else
            {
                Logger.Log.Warn("Ошибка при формировании документа (Не удалось получить тег body2)");
                return null;
            }

            return xdoc;
        }

        private int SaveArrivedFile()
        {
            string idgu = IdGosulug;
            //string fileDoc = DocGener(NameUslug, Fio);
            string param = "{\"serviceId\":\"" + idgu + "\", \"serviceResult\":1}";
            string signature = "";
            string filePath = "";

            List<MyCert> cert = Cripto.GelAllCertificates();
            foreach (var c in cert)
            {
                if (c.ToString() == Properties.Settings.Default.CryptoSert)
                {
                    XmlDocument xmlDocument = ToXmlDocument(DocGenerXml());
                    //xmlDocument.Load(new System.Xml.XmlTextReader(new StringReader(fileDoc)));
                    xmlDocument = Cripto.FileSignCadesBesX(xmlDocument, c);
                    filePath = "files\\arrived_" + DateTime.Now.ToString("o").Replace(":", "") + ".xml";
                    MyStatic.SaveStringToFile(xmlDocument.InnerXml, filePath);
                    
                    string fileParam = GetFileParam();
                    if (fileParam != "")
                    {
                        fileParam = fileParam.Replace("<container></container>", "<container>" + param + "</container>");
                        signature =
                            MyStatic.GetSignaturesFromStream(
                                Cripto.FileSignCadesBes(MyStatic.GenerateStreamFromString(fileParam), c));
                    }
                    else
                    {
                        using (Stream stream = MyStatic.GenerateStreamFromString(param))
                        {
                            signature = MyStatic.GenerateStringFromStream(Cripto.FileSignCadesBes(stream, c));
                        }
                    }
                }
            }

            //todo: проведка подписи (сигнатуры)

            string filePathFtp = "/" + IdGosulug + "/arrived.xml";
            FtpClient client = new FtpClient(Properties.Settings.Default.Ftp_Url)
            {
                Credentials = new NetworkCredential(Properties.Settings.Default.Ftp_User, Properties.Settings.Default.Ftp_Pass),
                Port = Properties.Settings.Default.Ftp_Port
            };
            try
            {
                client.Connect();
                client.UploadFile(filePath, filePathFtp);
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Возникла при загрузке файла на FTP возникла ошибка: " + ex.InnerException);
                return 0;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
                if (File.Exists(filePath))
                    File.Delete(filePath);
            }

            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandText = "INSERT INTO files " +
                                  "( file,  service_result,  params,  param_signature) " +
                                  "VALUES " +
                                  "(@file, @service_result, @params, @param_signature)";

                //cmd.Parameters.AddWithValue("@file", MyBase64.Encode(fileDocSign));
                cmd.Parameters.AddWithValue("@file", filePathFtp);
                cmd.Parameters.AddWithValue("@service_result", 1);
                cmd.Parameters.AddWithValue("@params", param);
                cmd.Parameters.AddWithValue("@param_signature", signature);
                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText += "SELECT @@IDENTITY";    //Получение id
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                Logger.Log.Info("Файл сохранён в базе данных. ID: " + id);
                //_fileSendPost = SendToPortal(param, signature, fileDocSign, IdDoc, id);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Возникла при добавлении файла в БД: " + ex.Message);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Обновление информации в таблице документов
        /// </summary>
        /// <param name="documentId"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        private bool UpdateArrivedMySql(string documentId, int fileId)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandText = "UPDATE documents SET " +
                                  "status = @status, " +
                                  "file_arrived = @file_arrived, " +
                                  "flag_file_arrived = @flag_file_arrived " +
                                  "WHERE id = @id";

                cmd.Parameters.AddWithValue("@status", 1);
                cmd.Parameters.AddWithValue("file_arrived", fileId);
                cmd.Parameters.AddWithValue("@flag_file_arrived", 0);
                cmd.Parameters.AddWithValue("@id", documentId);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Возникла ошибка обновления записи: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Получение текста из файла с параметрами
        /// </summary>
        /// <returns></returns>
        private string GetFileParam()
        {
            if (!File.Exists("param.xml"))
                return "";
            return MyStatic.GetStringFromFile("param.xml");
        }

        /// <summary>
        /// POST запрос
        /// </summary>
        /// <param name="url">POST-запрос по следуюзему URL</param>
        /// <param name="_params">Параметры выполнения услуги (JSON строка)</param>
        /// <param name="signature">Электронная цифровая подпись</param>
        /// <param name="file">Документ, предоставляемый при подтверждении</param>
        /// <returns></returns>
        private string Post(string url, string _params, string signature, string file)
        {
            Logger.Log.Info("params: " + _params);
            if (signature == "")
                Logger.Log.Info("signature: Пусто");
            if (!IsPostEnable)
                return "ok";
            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "multipart/form-data");
                request.AddParameter("params", _params);
                request.AddParameter("signature", signature);
                request.AddFile("document", Encoding.UTF8.GetBytes(file), "Уведомление.xml", "text/xml");
                return client.Execute(request).Content;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Возникла ошибка при отправке документа: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Преобразовать XDocument в XmlDocument
        /// </summary>
        /// <param name="xDocument">Исходный документ</param>
        /// <returns></returns>
        private XmlDocument ToXmlDocument(XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }
    }
}

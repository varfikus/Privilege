using System;
using System.Data;
using System.IO;
using System.Net;
using System.Xml;
using FluentFTP;
using MySql.Data.MySqlClient;
using ServiceMinsoc.Properties;

namespace ServiceMinsoc
{
    public class MyReceiveFile
    {
        public MyReceiveFile()
        {
            _fileName = "files\\" + DateTime.Now.ToString("o").Replace(":", "") + ".xml";
        }


        #region Переменные

        /// <summary>
        /// Адрес FTP-сервера
        /// </summary>
        public string FtpUrl = Settings.Default.Ftp_Url;
        /// <summary>
        /// Пользователь FTP-сервера
        /// </summary>
        public string FtpUser = Settings.Default.Ftp_User;
        /// <summary>
        /// Пароль FTP-сервера
        /// </summary>
        public string FtpPassword = Settings.Default.Ftp_Pass;
        /// <summary>
        /// Порт FTP-сервера
        /// </summary>
        public int FtpPort = Settings.Default.Ftp_Port;

        /// <summary>
        /// Включен POST-запрос
        /// </summary>
        public bool EnablePost = true;

        /// <summary>
        /// ID услуги
        /// </summary>
        public string ServiceId;

        /// <summary>
        /// Возникшие ошибки при выполнении
        /// </summary>
        private string _error;

        /// <summary>
        /// Путь к файлу
        /// </summary>
        private readonly string _fileName;

        /// <summary>
        /// Рабочая информация
        /// </summary>
        public string Info { get; private set; }

        #endregion


        /// <summary>
        /// Добавить файл в базу данных
        /// </summary>
        /// <param name="uri">FTP-Адрес хранения файла</param>
        /// <returns></returns>
        private string AddFtp(Uri uri)
        {
            return AddFtp(uri.Host, uri.AbsolutePath);
        }

        /// <summary>
        /// Добавить файл в базу данных
        /// </summary>
        /// <param name="host">Хост FTP-сервера</param>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        private string AddFtp(string host, string path)
        {
            Info = "";
            _error = "";

            FtpClient client = new FtpClient(host);
            //client.Credentials = new NetworkCredential(User, Password);
            try
            {
                client.Connect();
            }
            catch (Exception ex)
            {
                return "Возникла ошибка при подключении к FTP-серверу: " + ex.Message;
            }

            try
            {
                client.DownloadFile(_fileName, path);
                client.Disconnect();
            }
            catch (Exception ex)
            {
                client.Disconnect();
                if (File.Exists(_fileName))
                    File.Delete(_fileName);
                return "Возникла ошибка при загрузки файла с FTP-сервера: " + ex.Message;
            }

            if (!AddDocumentMySql(""))
            {
                //return "Ошибка при записи в базу данных";
                return _error;
            }

            return "ok";
        }

        /// <summary>
        /// Добавить файл в базу данных, скачав файл через ссылку
        /// </summary>
        /// <param name="url">Ссылка на файл</param>
        /// <returns></returns>
        public string DownloadFile(string url)
        {
            Info = "";
            _error = "";

            if (GetFile(url))
            {
                XmlDocument document = new XmlDocument { PreserveWhitespace = true };
                document.Load(_fileName);
                string text = document.InnerXml;

                if (string.IsNullOrEmpty(ServiceId))
                    ServiceId = GetXmlTagFromString(text, "idgosuslug");

                if (ServiceId == "")
                {
                    return "В документе не найден тег idgosuslug";
                }
                if (CheckIdUslugi(ServiceId))
                {
                    if (!UpdateDocumentMySql())
                        return _error;
                }
                else if (!AddDocumentMySql(text))
                    return _error;

                return "ok";
            }
            else
            {
                return "Не удалось скачать файл";
            }
        }

        /// <summary>
        /// Добавить файл в базу данных
        /// </summary>
        /// <param name="base64String">Base64 строка</param>
        /// <returns></returns>
        public string AddBase64(string base64String)
        {
            Info = "";
            _error = "";

            MyBase64.SaveToFile(base64String, _fileName);

            XmlDocument document = new XmlDocument { PreserveWhitespace = true };
            document.Load(_fileName);
            string text = document.InnerXml;

            if (string.IsNullOrEmpty(ServiceId))
                ServiceId = GetXmlTagFromString(text, "idgosuslug");

            if (ServiceId == "")
            {
                return "В документе не найден тег idgosuslug";
            }
            if (CheckIdUslugi(ServiceId))
            {
                if (!UpdateDocumentMySql())
                    return _error;
            }
            else if (!AddDocumentMySql(text))
                return _error;

            return "ok";
        }


        /// <summary>
        /// Добавить информацию из файла в базу данных.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool AddDocumentMySql(string text)
        {
            //XmlDocument document = new XmlDocument { PreserveWhitespace = true };
            //document.Load(_fileName);
            //string text = document.InnerXml;

            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "INSERT INTO documents " +
                              "( fio,  birthday,  date_add,  address_reg,  status,  electronic,  date_ispoln,  id_uslugi,  name_usl,  service_id,  region,  office,  file_id,  opek_fio) " +
                              "VALUES " +
                              "(@fio, @birthday, @date_add, @address_reg, @status, @electronic, @date_ispoln, @id_uslugi, @name_usl, @service_id, @region, @office, @file_id, @opek_fio)";

            string fio = GetXmlTagFromString(text, "vfio");
            if (!string.IsNullOrEmpty(fio))
            {
                if (fio != "" && fio.Substring(0, 3) == "от ")
                    fio = fio.Substring(3);
            }
            else
            {
                fio = GetXmlTagFromString(text, "fam") + " " +
                      GetXmlTagFromString(text, "im") + " " +
                      GetXmlTagFromString(text, "ot");
            }
            cmd.Parameters.AddWithValue("@fio", fio);
            cmd.Parameters.AddWithValue("@date_add", DateTime.Now);
            cmd.Parameters.AddWithValue("@status", 0);
            cmd.Parameters.AddWithValue("@date_ispoln", DateTime.Now.AddDays(Settings.Default.NumApply));
            string idGosuslug;
            if (string.IsNullOrEmpty(ServiceId))
            {
                idGosuslug = GetXmlTagFromString(text, "idgosuslug");
                if (idGosuslug == "")
                {
                    cmd.Parameters.AddWithValue("@id_uslugi", 0);
                    _error = "В документе не найден тег idgosuslug";
                    conn.Close();
                    return false;
                }
                cmd.Parameters.AddWithValue("@id_uslugi", idGosuslug);
            }
            else
            {
                idGosuslug = ServiceId;
                cmd.Parameters.AddWithValue("@id_uslugi", idGosuslug);
            }
            string birthday = GetXmlTagFromString(text, "vdatar");
            DateTime dateBirthday;
            if (DateTime.TryParse(birthday, out dateBirthday))
                cmd.Parameters.AddWithValue("@birthday", dateBirthday);
            else
                cmd.Parameters.AddWithValue("@birthday", null);
            cmd.Parameters.AddWithValue("@address_reg", GetXmlTagFromString(text, "vadress"));
            string nameUsl = GetXmlTagFromString(text, "nameservice");
            cmd.Parameters.AddWithValue("@name_usl", nameUsl);
            string serviceId = GetXmlTagFromString(text, "idservice");
            cmd.Parameters.AddWithValue("@service_id", serviceId == "" ? 0.ToString() : serviceId);
            if (!CheckServiceId(serviceId))
                InsertService(serviceId, nameUsl);

            string region = GetXmlTagFromString(text, "region");
            //string office = GetXmlTagFromString(decodeString, "office");
            cmd.Parameters.AddWithValue("@office", region != "" ? region : "0");
            cmd.Parameters.AddWithValue("@region", GetRegionFromOfficeId(region));
            cmd.Parameters.AddWithValue("@opek_fio", GetXmlTagFromString(text, "opekfio"));
            string electronic = GetXmlTagFromString(text, "electronic");
            if (electronic == "true" || electronic == "1")
            {
                cmd.Parameters.AddWithValue("@electronic", 1);
            }
            else
            {
                cmd.Parameters.AddWithValue("@electronic", 0);
            }

            string filePath = UploadFileFtp(idGosuslug);
            if (filePath == "")
            {
                _error = "Возникла ошибка при загрузке файла на FTP-сервер";
                conn.Close();
                return false;
            }
            int fileId = AddFileMySql(filePath);
            if (fileId == 0)
            {
                DeleteFileFtp(filePath);
                _error = "Возникла ошибка при добавлении информации о файле в базу данных";
                conn.Close();
                return false;
            }
            cmd.Parameters.AddWithValue("@file_id", fileId);

            try
            {
                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText += "SELECT @@IDENTITY"; //Получение id
                int idDoc = Convert.ToInt32(cmd.ExecuteScalar());
                string mes = "Добавлена запись в базу данных. ID:" + idDoc + " file_id:" + fileId;
                Logger.Log.Info(mes);
                //AddDataGrid("info", mes);
                Info = mes;

                NotificationMessage notification = new NotificationMessage
                {
                    IdDoc = idDoc,
                    IdGosulug = idGosuslug,
                    NameUslug = nameUsl,
                    Fio = fio,
                    IsPostEnable = EnablePost
                };
                notification.UpdateStatusDocument(idDoc.ToString());
                return true;
            }
            catch (Exception ex)
            {
                string mes = "Ошибка при добавлении новой записи в базу данных: " + ex.Message;
                Logger.Log.Warn(mes);
                _error = mes;
                DeleteFileFtp(filePath);
                DeleteFileMySql(fileId);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }


        /// <summary>
        /// Удаление файла из базы данных
        /// </summary>
        /// <param name="id">ID файла</param>
        private void DeleteFileMySql(int id)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM files " +
                              "WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Ошибка удаления файла из базы данных: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Получить информацию из тега XML-документа
        /// </summary>
        /// <param name="xmlDoc">XML-документ в виде строки</param>
        /// <param name="tagName">Тег</param>
        /// <param name="index">Индекс (номер по порядку тега, если их несколько)</param>
        /// <returns></returns>
        private string GetXmlTagFromString(string xmlDoc, string tagName, int index = 0)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xmlDoc);
                XmlNode elem = document.GetElementsByTagName(tagName)[index];
                string xml = elem.InnerXml;
                return xml;
            }
            catch
            {
                Logger.Log.Warn("Возникла ошибка XML: Не удалось получить данные из документа с тегом: " + tagName);
                return "";
            }
        }

        /// <summary>
        /// Получение данных из XML документа
        /// </summary>
        /// <param name="fileNamePath">Путь к файлу</param>
        /// <param name="tagName">Тег</param>
        /// <param name="index">Номер элемента</param>
        /// <returns></returns>
        private string GetXmlTagFromFile(string fileNamePath, string tagName, int index = 0)
        {
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(fileNamePath);
                XmlNode elem = document.GetElementsByTagName(tagName)[index];
                string xml = elem.InnerXml;
                return xml;
            }
            catch
            {
                Logger.Log.Warn("Возникла ошибка XML: Не удалось получить данные из документа с тегом: " + tagName);
                return "";
            }
        }


        /// <summary>
        /// Обновить информацию в базе данных из файла
        /// </summary>
        /// <param name="fileText">Текст файла</param>
        /// <returns></returns>
        private bool UpdateDocumentMySql()
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE documents SET " +
                              "status = @status, " +
                              "file_incoming = @file_incoming " +
                              "WHERE id = @id";

            cmd.Parameters.AddWithValue("@status", 9); //статус "Ответ с Портала"

            string filePath = UploadFileFtp(ServiceId, "incoming.xml");
            if (filePath == "")
            {
                _error = "Возникла ошибка при загрузке файла на FTP-сервер";
                conn.Close();
                return false;
            }
            int fileId = AddFileMySql(filePath);
            if (fileId == 0)
            {
                DeleteFileFtp(filePath);
                _error = "Возникла ошибка при добавлении информации о файле в базу данных";
                conn.Close();
                return false;
            }

            cmd.Parameters.AddWithValue("@file_incoming", fileId);
            cmd.Parameters.AddWithValue("@id", GetIdUslugi(ServiceId));

            try
            {
                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText += "SELECT @@IDENTITY"; //Получение id
                int idDoc = Convert.ToInt32(cmd.ExecuteScalar());
                string mes = "Обновлена запись в базе данных. ID:" + idDoc + " file_id:" + fileId;
                //Logger.Log.Info(mes);
                Info = mes;
            }
            catch (Exception ex)
            {
                string mes = "Ошибка при изменении записи в базе данных: " + ex.Message;
                //Logger.Log.Warn(mes);
                _error = mes;
            }

            return true;
        }

        /// <summary>
        /// Проверить если ID госуслуги в базе данных (для услуги: акт сверки расчетов и платежей налогоплательщика в бюджеты различных уровней и государственные внебюджетные фонды)
        /// </summary>
        /// <param name="idUslugi">ID госуслуги</param>
        /// <returns>Наличие услуги в базе данных</returns>
        private bool CheckIdUslugi(string idUslugi)
        {
            string query = "SELECT id FROM documents WHERE id_uslugi = '" + idUslugi + "'";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    return true;
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Получить ID документа по ID услуги
        /// </summary>
        /// <param name="idUslugi">ID услуги</param>
        /// <returns>ID документа</returns>
        private int GetIdUslugi(string idUslugi)
        {
            string query = "SELECT id FROM documents WHERE id_uslugi = '" + idUslugi + "'";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int res;
                    if (int.TryParse(dt.Rows[0]["id"].ToString(), out res))
                        return res;
                    return 0;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Ошибка при получении ID документа с помощью ID услуги");
                return 0;
            }
        }


        private string GetRegionFromOfficeId(string idOffice)
        {
            string query = "select name " +
                           "from region " +
                           "where id in (select region_id from office where id = '" + idOffice + "')";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return "";
                return dt.Rows[0][0].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Получить ID услуги из базы данных
        /// </summary>
        /// <param name="idservice">ID услуги</param>
        /// <param name="serviceName">Наименование услуги</param>
        /// <returns>ID услуги (0 - не найдена)</returns>
        private int InsertService(string idservice, string serviceName)
        {
            int servId = CheckService(serviceName);
            if (servId != 0)
                return servId;

            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "INSERT INTO services " +
                              "( service_id,  name) " +
                              "VALUES " +
                              "(@service_id, @name)";
            cmd.Parameters.AddWithValue("@service_id", idservice);
            cmd.Parameters.AddWithValue("@name", serviceName);

            try
            {
                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText += "SELECT @@IDENTITY";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// Проверка на наличие услуги в базе данных по наименованию
        /// </summary>
        /// <param name="serviceName">Наименование услуги</param>
        /// <returns>ID услуги (0 - не найдена)</returns>
        private int CheckService(string serviceName)
        {
            string query = "SELECT * " +
                           "FROM services " +
                           "WHERE services.name LIKE '%" + serviceName + "%'";

            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return 0;
                return Convert.ToInt32(dt.Rows[dt.Rows.Count - 1]["id"]);
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// Проверка на наличие в базе данных по ID услуги
        /// </summary>
        /// <param name="id">ID услуги</param>
        /// <returns></returns>
        private bool CheckServiceId(string id)
        {
            string query = "SELECT * " +
                           "FROM services " +
                           "WHERE services.service_id = '" + id + "'";

            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return false;
                return true;
            }
            catch //(Exception)
            {
                return false;
            }
        }


        /// <summary>
        /// Загрузить файл на FTP-сервер
        /// </summary>
        /// <param name="idUslugi">ID услуги (Портал)</param>
        /// <param name="fileName">Наименование файла на FTP</param>
        /// <returns></returns>
        private string UploadFileFtp(string idUslugi, string fileName = "file.xml")
        {
            string file = "/" + idUslugi + "/" + fileName;
            FtpClient client = new FtpClient(FtpUrl)
            {
                Credentials = new NetworkCredential(FtpUser, FtpPassword),
                Port = Properties.Settings.Default.Ftp_Port
            };
            try
            {
                client.Connect();
                client.UploadFile(_fileName, file, FtpRemoteExists.Overwrite, true);
                return file;
            }
            catch (Exception ex)
            {
                string mes = "Возникла ошибка при загрузке файла на FTP-сервер: " + ex.InnerException?.Message;
                Logger.Log.Warn(mes);
                //_error += "\n\n" + mes;
                return "";
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
                File.Delete(_fileName);
            }
        }

        /// <summary>
        /// Удаление файла с FTP-сервера
        /// </summary>
        /// <param name="ftpPath">Путь к файлу</param>
        /// <returns></returns>
        private bool DeleteFileFtp(string ftpPath)
        {
            FtpClient client = new FtpClient(FtpUrl)
            {
                Credentials = new NetworkCredential(FtpUser, FtpPassword),
                Port = FtpPort
            };
            try
            {
                client.Connect();
                if (client.FileExists(ftpPath))
                    client.DeleteFile(ftpPath);
                return true;
            }
            catch (Exception ex)
            {
                string mes = "Возникла ошибка при удалении файла с FTP-сервера: " + ex.Message;
                Logger.Log.Warn(mes);
                //_error += "\n\n" + mes;
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }

        /// <summary>
        /// Добавить информацию о файле в базу данных
        /// </summary>
        /// <returns></returns>
        private int AddFileMySql(string filePathFtp)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandText = "INSERT INTO files " +
                                  "( file) " +
                                  "VALUES " +
                                  "(@file)";

                cmd.Parameters.AddWithValue("@file", filePathFtp);
                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT @@IDENTITY";
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                Logger.Log.Info("Файл сохранён в базе данных. ID: " + id);
                return id;
            }
            catch (Exception ex)
            {
                string mes = "Возникла при добавлении файла в БД: " + ex.Message;
                Logger.Log.Warn(mes);
                _error = mes;
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }


        private bool AddErrorFileMySql()
        {
            FtpClient client = new FtpClient(FtpUrl) { Credentials = new NetworkCredential(FtpUser, FtpPassword) };
            try
            {
                client.Connect();
                //client.UploadFile()
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Загрузить файл через ссылку
        /// </summary>
        /// <param name="url">Ссылка</param>
        /// <returns></returns>
        private bool GetFile(string url)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri(url), _fileName);
                if (File.Exists(_fileName))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                _error = "Не удалось скачать файл: " + ex.Message;
                return false;
            }
        }
    }
}

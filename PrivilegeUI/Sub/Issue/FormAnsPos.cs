using PrivilegeAPI;
using PrivilegeAPI.Services;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static PrivilegeUI.Models.FullApplication;
using static System.Net.WebRequestMethods;
using Application = PrivilegeAPI.Models.Application;
using File = System.IO.File;

namespace PrivilegeUI.Sub.Issue
{
    public partial class FormAnsPos : Form
    {
        #region Fields

        /// <summary
        /// Клиент для работы с API
        /// </summary>
        private readonly MyHttpClient _apiClient;
        /// <summary>
        /// Родительская форма
        /// </summary>
        private readonly FormMain _parentForm;
        /// <summary>
        /// ID записи
        /// </summary>
        private int _id;
        /// <summary>
        /// ID госуслуг
        /// </summary>
        private string _idGosUslugi;
        /// <summary>
        /// ID госуслуги (ЕРГУ)
        /// </summary>
        private int _idService;
        /// <summary>
        /// Путь до исходящего документа
        /// </summary>
        private string _path;
        /// <summary>
        /// Текущая заявка 
        /// </summary>
        private Application _app;
        private Htmlx fullApp;

        private FtpSettings ftpSettings;
        private FtpService ftpContext;

        #endregion


        #region Constructor

        public FormAnsPos(MyHttpClient apiClient, Application app, FormMain parentForm)
        {
            InitializeComponent();
            ftpSettings = new FtpSettings
            {
                Server = Properties.Settings.Default.ServerFTP,
                Username = Properties.Settings.Default.UserFTP,
                Password = Properties.Settings.Default.PassFTP,
                Port = Properties.Settings.Default.PortFTP
            };
            ftpContext = new FtpService(ftpSettings);
            _apiClient = apiClient;
            _app = app;
            _parentForm = parentForm;
        }

        #endregion


        #region Events

        private async void FormAnsPos_Load(object sender, EventArgs e)
        {
            try
            {
                if (_app.Id <= 0)
                {
                    MessageBox.Show("Неверный идентификатор заявки.");
                    Close();
                    return;
                }

                fullApp = await LoadHtmlxFromFtpAsync(_app.File.Path);
                if (fullApp == null)
                {
                    MessageBox.Show("Не удалось загрузить данные заявки.");
                    Close();
                    return;
                }

                tB_operator.Text = UserInfo.CurrentUser.Name;
                tB_service.Text = fullApp.Body2.Servinfo.Nameservice;
                tB_fio.Text = $"{fullApp.Body2.Container.Topheader.Tophead.PersData.Fam} {fullApp.Body2.Container.Topheader.Tophead.PersData.Im} {fullApp.Body2.Container.Topheader.Tophead.PersData.Ot}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заявки: " + ex.Message);
            }
        }

        private async void btnOk_Click(object sender, EventArgs e)
        {
            if (!Valid())
                return;

            bool ok = await CreateDoc();
            if (!ok)
                return;

            DialogResult = DialogResult.OK;
            _parentForm?.ResetButton();
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _parentForm?.ResetButton();
            Close();
        }

        private async void btn_preview_Click(object sender, EventArgs e)
        {
            bool ok = await CreateDoc();
            if (!ok)
                return;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Проверка заполнения полей
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            bool f = true;

            foreach (Control item in Controls)
            {
                if (item is Label) item.ForeColor = Color.Black;
            }
            toolTip1.RemoveAll();

            if (tB_fio.Text == "")
            {
                string caption = "Это поле не может быть пустым";
                lbl_fio.ForeColor = Color.Red;
                toolTip1.SetToolTip(lbl_fio, caption);
                toolTip1.SetToolTip(tB_fio, caption);
                f = false;
            }

            return f;
        }

        /// <summary>
        /// Создать документ
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CreateDoc()
        {
            //if (!WorkMethods.CheckStatus(_id, new[] { "3", "8", "9" }))
            //{
            //    MessageBox.Show(@"Этап принятия в работу уже выполнен", @"Внимание",
            //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}

            if (!await GenerateHtmlxAndUploadAsync(fullApp.Body2.Servinfo.Idservice, fullApp.Body2.Servinfo.Nameservice, tB_fio.Text, tB_operator.Text, tB_operatorTel.Text))
                return false;

            return true;
        }

        /// <summary>
        /// Формирование документа
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GenerateHtmlxAndUploadAsync(int idGosUslugi, string service, string fio, string operatorName, string operatorPhone)
        {
            try
            {
                string baseTempPath = Path.Combine(Path.GetTempPath(), "Applications", "AnswerPos");
                Directory.CreateDirectory(baseTempPath);

                string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "template", "template.xml");
                if (!File.Exists(templatePath))
                {
                    MessageBox.Show("Не найден шаблон: template.xml", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                XDocument xdoc = XDocument.Load(templatePath);
                XNamespace ns = "http://www.w3.org/1999/xhtml";

                var body2 = xdoc.Descendants(ns + "body2").FirstOrDefault();
                if (body2 == null)
                {
                    MessageBox.Show("Элемент <body2> не найден в шаблоне.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var container = body2.Element(ns + "container");
                if (container == null)
                {
                    MessageBox.Show("Элемент <container> не найден в шаблоне.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var content = container.Element(ns + "content");
                if (content != null)
                {
                    content.RemoveAll(); 

                    var styleAttr = new XAttribute("style", "text-indent: 0cm;");

                    content.Add(new XElement(ns + "p", styleAttr, new XText("Уважаемый(ая) " + fio + ", ")));
                    content.Add(new XElement(ns + "p", styleAttr, new XText("В соответствии с Вашим обращением по услуге \"" + service + "\".")));
                    content.Add(new XElement(ns + "p", styleAttr, new XText("Ваша заявка успешно обработана и одобрена.")));
                }

                var executor = container.Element(ns + "executor");
                executor?.Element(ns + "executorname")?.SetValue(operatorName);
                executor?.Element(ns + "executorphone")?.SetValue(operatorPhone);
                executor?.Element(ns + "executordate")?.SetValue(DateTime.Now.ToShortDateString());

                var docstatus = container.Element(ns + "docstatus");

                var reg = container.Element(ns + "reg");

                var servinfo = body2.Element(ns + "servinfo");
                if (servinfo != null)
                {
                    var idGos = servinfo.Element(ns + "idgosuslug");
                    if (idGos == null)
                    {
                        servinfo.Add(new XElement(ns + "idgosuslug", idGosUslugi));
                    }
                    else
                    {
                        idGos.Value = idGosUslugi.ToString();
                    }

                    servinfo.Element(ns + "signaturesxml")?.SetValue(string.Empty);
                    servinfo.Element(ns + "timestampout")?.SetValue(DateTime.Now.ToString("o"));
                }

                string tempFileName = $"answer_{_app.File.Name}";
                string tempFilePath = Path.Combine(baseTempPath, tempFileName);
                xdoc.Save(tempFilePath, SaveOptions.DisableFormatting);

                string ftpFilePath = $"Applications/AnswerPos/{tempFileName}";
                await ftpContext.SaveFileAsync(ftpFilePath, tempFilePath);
                await ftpContext.DisconnectAsync();

                //if (File.Exists(tempFilePath))
                //    File.Delete(tempFilePath);

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при создании/загрузке HTMLX: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<Htmlx> LoadHtmlxFromFtpAsync(string remoteFtpPath)
        {
            try
            {
                string fileName = Path.GetFileName(remoteFtpPath);
                string tempDir = Path.GetTempPath();
                string tempPath = Path.Combine(tempDir, fileName);

                if (!File.Exists(tempPath))
                {
                    await ftpContext.DownloadFileAsync(remoteFtpPath, tempPath);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Htmlx));
                using (FileStream fs = new FileStream(tempPath, FileMode.Open))
                {
                    var htmlx = (Htmlx)serializer.Deserialize(fs);
                    return htmlx;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке/десериализации XML: " + ex.Message);
                return null;
            }
        }

        #region UpdateDb

        /// <summary>
        /// Обновление данных в базе данных MySQL.
        /// </summary>
        //private bool UpdateSql()
        //{
        //    int file = SaveFile();
        //    if (file == 0)
        //        return false;

        //    MySqlConnection conn = new MySqlConnection(Connection.Conn);
        //    conn.Open();
        //    MySqlCommand cmd = conn.CreateCommand();

        //    try
        //    {
        //        cmd.CommandText = "UPDATE documents SET " +
        //                          "status = @status, " +
        //                          "date_ispoln = @date_ispoln, " +
        //                          "file_finaly = @file_finaly, " +
        //                          "flag_file_finaly = @flag_file_finaly " +
        //                          "WHERE id = @id";

        //        cmd.Parameters.AddWithValue("@status", 6);
        //        cmd.Parameters.AddWithValue("@date_ispoln", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@file_finaly", file);
        //        cmd.Parameters.AddWithValue("@flag_file_finaly", 0);
        //        cmd.Parameters.AddWithValue("@id", _id);
        //        cmd.ExecuteNonQuery();
        //        Logger.Log.Warn("[" + _id + "] " + "Документ выдан");
        //        Connection.AddLogs(_id.ToString(), "Документ выдан");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Warn("[" + _id + "] Возникла ошибка обновления записи: " + ex.Message);
        //        DelFile(file);
        //        Connection.AddLogs(_id.ToString(), "Возникла ошибка обновления записи: " + ex.Message);
        //        MessageBox.Show(@"Возникла ошибка обновления записи:" + Environment.NewLine + ex.Message,
        //            @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        ///// <summary>
        ///// Обновление данных в базе данных MySQL. Документ отправляется руководителю
        ///// </summary>
        //private bool UpdateDirSql()
        //{
        //    int file = SaveFile();
        //    if (file == 0)
        //        return false;

        //    MySqlConnection conn = new MySqlConnection(Connection.Conn);
        //    conn.Open();
        //    MySqlCommand cmd = conn.CreateCommand();

        //    try
        //    {
        //        cmd.CommandText = "UPDATE documents SET " +
        //                          "status = @status, " +
        //                          "date_ispoln = @date_ispoln, " +
        //                          "file_director = @file_director, " +
        //                          "director_decision = @director_decision " +
        //                          "WHERE id = @id";

        //        cmd.Parameters.AddWithValue("@status", 5);
        //        cmd.Parameters.AddWithValue("@date_ispoln", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@file_director", file);
        //        cmd.Parameters.AddWithValue("@director_decision", 1);
        //        cmd.Parameters.AddWithValue("@id", _id);
        //        cmd.ExecuteNonQuery();
        //        Logger.Log.Info("[" + _id + "] " + "Выдача документа");
        //        Connection.AddLogs(_id.ToString(), "Выдача документа");
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Warn("[" + _id + "] Возникла ошибка обновления записи: " + ex.Message);
        //        DelFile(file);
        //        Connection.AddLogs(_id.ToString(), "Возникла ошибка обновления записи: " + ex.Message);
        //        MessageBox.Show(@"Возникла ошибка обновления записи:" + Environment.NewLine + ex.Message,
        //            @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return false;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        ///// <summary>
        ///// Сохранить файл в базе данных MySQL
        ///// </summary>
        //private int SaveFile()
        //{
        //    int serviceResult = 1;
        //    string param = "{\"serviceId\":\"" + _idGosUslugi + "\", \"serviceResult\":" + serviceResult + "}";
        //    string signature = "";

        //    bool flagDocSig = false;
        //    List<MyCert> cert = SignDoc.GelAllCertificates();
        //    foreach (var c in cert)
        //    {
        //        if (c.ToString() == UserInfo.Sert)
        //        {
        //            try
        //            {
        //                XmlDocument xmlDocument = new XmlDocument { PreserveWhitespace = true };
        //                xmlDocument.Load(_path);
        //                var xml1 = xmlDocument.InnerXml;
        //                xmlDocument = SignDoc.FileSignCadesBesX(xmlDocument, c);
        //                var xml2 = xmlDocument.InnerXml;
        //                WorkMethods.SaveFileXml(xmlDocument, _path);
        //                flagDocSig = !Equals(xml1, xml2);

        //                string fileParam = WorkMethods.GetFileParam();
        //                if (fileParam != "")
        //                {
        //                    fileParam = fileParam.Replace("<container></container>",
        //                        "<container>" + param + "</container>");
        //                    signature =
        //                        WorkMethods.GetSignaturesFromStream(
        //                            SignDoc.FileSignCadesBes(WorkMethods.GenerateStreamFromString(fileParam), c));
        //                }
        //                else
        //                {
        //                    using (Stream stream = WorkMethods.GenerateStreamFromString(param))
        //                    {
        //                        signature = WorkMethods.GenerateStringFromStream(SignDoc.FileSignCadesBes(stream, c));
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                string mes = "Возникла ошибка при подписании: " + ex.Message;
        //                Logger.Log.Warn("[" + _id + "] " + mes);
        //                Connection.AddLogs(_id.ToString(), "Возникла ошибка при подписании: " + ex.Message);
        //                MessageBox.Show(mes);
        //                return 0;
        //            }
        //        }
        //    }

        //    if (signature == string.Empty || !flagDocSig)
        //    {
        //        string mes = "Документ не подписан";
        //        Logger.Log.Warn("[" + _id + "] " + mes);
        //        Connection.AddLogs(_id.ToString(), mes);
        //        MessageBox.Show(mes + @"." + Environment.NewLine + @"Возможно не выбран сертификат в настройках программы.",
        //            @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return 0;
        //    }

        //    string pathFtp = "/" + _idGosUslugi + "/finaly.xml";
        //    if (!MyFtp.UploadToFtp(_path, pathFtp))
        //    {
        //        string mes = "Возникла ошибка при загрузке файл на FTP-сервер.";
        //        Logger.Log.Warn("[" + _id + "] " + mes);
        //        Connection.AddLogs(_id.ToString(), mes + " Файл: " + pathFtp);
        //        MessageBox.Show(mes, @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return 0;
        //    }

        //    MySqlConnection conn = new MySqlConnection(Connection.Conn);
        //    conn.Open();
        //    MySqlCommand cmd = conn.CreateCommand();

        //    try
        //    {
        //        cmd.CommandText = "INSERT INTO files " +
        //                          "( file,  service_result,  params,  param_signature) " +
        //                          "VALUES " +
        //                          "(@file, @service_result, @params, @param_signature)";

        //        cmd.Parameters.AddWithValue("@file", pathFtp);
        //        cmd.Parameters.AddWithValue("@service_result", serviceResult);
        //        cmd.Parameters.AddWithValue("@params", param);
        //        cmd.Parameters.AddWithValue("@param_signature", signature);
        //        cmd.ExecuteNonQuery();
        //        cmd = conn.CreateCommand();
        //        cmd.CommandText += "SELECT @@IDENTITY";    //Получение id
        //        int id = Convert.ToInt32(cmd.ExecuteScalar());
        //        Logger.Log.Info("[" + _id + "] Файл сохранён в базе данных");
        //        return id;
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Warn("[" + _id + "] Возникла при добавлении файла в БД: " + ex.Message);
        //        Connection.AddLogs(_id.ToString(), "Возникла при добавлении файла в БД: " + ex.Message);
        //        MessageBox.Show(@"Возникла при добавлении файла в БД:" + Environment.NewLine + ex.Message,
        //            @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        return 0;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        ///// <summary>
        ///// Удаление файла по ID
        ///// </summary>
        ///// <param name="fileId">ID файла</param>
        //private void DelFile(int fileId)
        //{
        //    MySqlConnection conn = new MySqlConnection(Connection.Conn);
        //    conn.Open();
        //    MySqlCommand cmd = conn.CreateCommand();

        //    cmd.CommandText = "DELETE FROM files " +
        //                      "WHERE id = @id";
        //    cmd.Parameters.AddWithValue("@id", fileId);

        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log.Warn("[" + _id + "] " + "Ошибка удаления файла из базы данных: " + ex.Message);
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        #endregion

        #endregion
    }
}

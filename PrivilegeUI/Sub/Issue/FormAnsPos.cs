using PrivilegeUI.Models;
using System.Xml;
using System.Xml.Linq;

namespace PrivilegeUI.Sub.Issue
{
    public partial class FormAnsPos : Form
    {
        #region Fields

        /// <summary>
        /// Родительская форма
        /// </summary>
        private readonly FormMain _parentForm;
        /// <summary>
        /// Строка из DataGridView
        /// </summary>
        private readonly DataGridViewRow _row;

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

        #endregion


        #region Constructor

        public FormAnsPos(FormMain parentForm, DataGridViewRow row)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _row = row;
        }

        #endregion


        #region Events

        private void FormAnsPos_Load(object sender, EventArgs e)
        {
            tB_operator.Text = UserInfo.CurrentUser.Name;

            _idGosUslugi = _row.Cells["id_gosuslug"].Value.ToString();
            int.TryParse(_row.Cells["id"].Value.ToString(), out _id);
            int.TryParse(_row.Cells["service_id"].Value.ToString(), out _idService);
            tB_fio.Text = _row.Cells["fio"].Value.ToString();
            tB_service.Text = _row.Cells["name_usl"].Value.ToString();

            lbl_header.Text += @" [" + _idGosUslugi + @"]";
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!Valid())
                return;

            if (!CreateDoc())
                return;

            //if (!UpdateSql())
            //    return;

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

        private void btn_preview_Click(object sender, EventArgs e)
        {
            //if (CreateDoc())
            //    WorkMethods.OpenFile(_path);
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
        private bool CreateDoc()
        {
            //if (!WorkMethods.CheckStatus(_id, new[] { "3", "8", "9" }))
            //{
            //    MessageBox.Show(@"Этап принятия в работу уже выполнен", @"Внимание",
            //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}

            if (!DocGenerNew())
                return false;

            return true;
        }

        /// <summary>
        /// Формирование документа
        /// </summary>
        /// <returns></returns>
        private bool DocGenerNew()
        {
            //WorkMethods.CheckTempDirectory();

            //_path = @"temp\doc" + _idGosUslugi + ".xml";
            //if (!File.Exists(@"template\template.xml"))
            //{
            //    MessageBox.Show(@"Не найден шаблон: template.xml", @"Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}

            //XDocument xdoc = XDocument.Load(@"template\template.xml");
            //XNamespace ns = xdoc.Root?.GetDefaultNamespace();
            //if (ns == null)
            //{
            //    MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить корневой элемент)", @"Ошибка",
            //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}
            //XElement body = xdoc.Root?.Element(ns + "body2");
            //if (body != null)
            //{
            //    XElement container = body.Element(ns + "container");
            //    if (container != null)
            //    {
            //        #region Основная часть

            //        XElement reg = container.Element(ns + "reg");
            //        if (reg != null)
            //        {
            //            //XElement datareg = reg.Element(ns + "datareg");
            //            //if (datareg != null)
            //            //    datareg.Value = Convert.ToDateTime(dTP_dateOut.Value).ToShortDateString();
            //        }

            //        XElement header = container.Element(ns + "header");
            //        if (header != null)
            //        {
            //            header.Value = "";
            //        }

            //        XElement content = container.Element(ns + "content");
            //        if (content != null)
            //        {
            //            #region content

            //            content.Value = "";
            //            switch (_idService)
            //            {
            //                case 447:
            //                    XAttribute style = new XAttribute("style", "text-indent: 0cm;");
            //                    content.Add(
            //                        new XElement(ns + "p",
            //                            "Уважаемый(ая) " + tB_fio.Text + ".", style),
            //                        new XElement(ns + "p",
            //                            "По Вашему запросу предоставляем Вам следующую информацию:", style),
            //                        new XElement(ns + "p", tB_info447.Text, style),
            //                        new XElement(ns + "p", tB_extra447.Text, style)
            //                    );
            //                    break;
            //                    //default:
            //                    //    {
            //                    //        content.Add(
            //                    //            new XElement(ns + "p",
            //                    //                "Уважаемый(ая), " + tB_fio.Text + ", сообщаем Вам о готовности заказанной Вами услуги «" +
            //                    //                _row.Cells["name_usl"].Value + "». " +
            //                    //                "Вы можете Получить результат вы можете с " + dTP_consid.Value.ToString("d") +
            //                    //                " по будням в рабочее время  по адресу г.Тирасполь, ул.25 Октября, д.114, кабинет " +
            //                    //                tB_cab.Text + "."));
            //                    //    }
            //                    //    break;
            //            }

            //            #endregion
            //        }

            //        XElement executor = container.Element(ns + "executor");
            //        if (executor != null)
            //        {
            //            XElement executorname = executor.Element(ns + "executorname");
            //            if (executorname != null)
            //                executorname.Value = "Исполнитель - " + tB_operator.Text;
            //            XElement executorphone = executor.Element(ns + "executorphone");
            //            if (executorphone != null)
            //                executorphone.Value = "Контактный телефон - " + tB_operatorTel.Text;
            //            XElement executordate = executor.Element(ns + "executordate");
            //            if (executordate != null)
            //                executordate.Value = "";
            //        }
            //        XElement docstatus = container.Element(ns + "docstatus");
            //        if (docstatus != null)
            //        {
            //            XElement datedocexecutor = docstatus.Element(ns + "datedocexecutor");
            //            if (datedocexecutor != null)
            //            {
            //                datedocexecutor.Value = "";
            //                datedocexecutor.Add(DateTime.Now.ToString("g"));
            //            }
            //        }

            //        #endregion
            //    }
            //    else
            //    {
            //        MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить тег контейнера)",
            //            @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return false;
            //    }

            //    XElement servinfo = body.Element(ns + "servinfo");
            //    if (servinfo != null)
            //    {
            //        XElement signaturesxml = servinfo.Element(ns + "signaturesxml");
            //        if (signaturesxml != null)
            //        {
            //            signaturesxml.Value = "";
            //        }
            //        XElement idgosuslug = servinfo.Element(ns + "idgosuslug");
            //        if (idgosuslug == null)
            //        {
            //            idgosuslug = new XElement(
            //                ns + "idgosuslug",
            //                _idGosUslugi,
            //                new XAttribute("style", "display: none !important;"));
            //            servinfo.Add(idgosuslug);
            //        }
            //        else
            //        {
            //            idgosuslug.Value = _idGosUslugi;
            //            idgosuslug.Add(new XAttribute("style", "display: none !important;"));
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить тег сервисной информации)",
            //            @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        return false;
            //    }
            //}
            //else
            //{
            //    MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить тег body2)",
            //        @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}

            //try
            //{
            //    xdoc.Save(_path, SaveOptions.DisableFormatting);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(@"Не удалось сохранить документ: " + ex.Message, @"Ошибка",
            //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return false;
            //}

            return true;
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

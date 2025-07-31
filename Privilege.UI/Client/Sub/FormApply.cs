using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Privilege.UI.Classes;
using Privilege.UI.Classes.Signature;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace Privilege.UI.Window.Client.Sub
{
    public partial class FormApply : Form
    {
        #region Fields

        /// <summary>
        /// Родительская форма
        /// </summary>
        private readonly FormMain _parentForm;

        /// <summary>
        /// Данные из основной таблицы
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

        /// <summary>
        /// Флаг принятого документа (отказаного)
        /// </summary>
        private readonly bool _decision = true;

        #endregion


        #region Constructor

        public FormApply(FormMain parentForm, DataGridViewRow row, bool decision = true)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _row = row;
            _decision = decision;
        }

        #endregion


        #region Events

        private void FormApply_Load(object sender, EventArgs e)
        {
            tB_operator.Text = UserInfo.Name;
            tB_operatorTel.Text = UserInfo.Tel;

            int.TryParse(_row.Cells["id"].Value.ToString(), out _id);
            _idGosUslugi = _row.Cells["id_gosuslug"].Value.ToString();
            int.TryParse(_row.Cells["service_id"].Value.ToString(), out _idService);

            tB_fio.Text = _row.Cells["fio"].Value.ToString();
            tB_service.Text = _row.Cells["name_usl"].Value.ToString();

            lbl_header.Text = _decision
                ? "Принятие в работу документа [" + _idGosUslugi + @"]"
                : "Отказ в приёме [" + _idGosUslugi + "]";

            if (!_decision)
            {
                lbl_denial.Visible = true;
                tB_denial.Visible = true;
                lbl_consid.Visible = false;
                dTP_consid.Visible = false;
            }

            dTP_consid.Value = DateTime.Now.AddDays(3);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!Valid())
                return;

            if (!CreateDoc())
                return;

            if (!UpdateMySql())
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

        private void btn_preview_Click(object sender, EventArgs e)
        {
            if (CreateDoc())
                WorkMethods.OpenFile(_path);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Создать документ
        /// </summary>
        /// <returns></returns>
        private bool CreateDoc()
        {
            if (!WorkMethods.CheckStatus(_id, new[] { "0", "1" }))
            {
                MessageBox.Show(@"Этап принятия в работу уже выполнен", @"Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (!DocGenerNew())
                return false;

            return true;
        }

        private bool Valid()
        {


            return true;
        }

        /// <summary>
        /// Формирование документа
        /// </summary>
        /// <returns></returns>
        private bool DocGenerNew()
        {
            WorkMethods.CheckTempDirectory();

            _path = @"temp\doc" + _idGosUslugi + ".xml";
            if (!File.Exists(@"template\template.xml"))
            {
                MessageBox.Show(@"Не найден шаблон: template.xml", @"Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            XDocument xdoc = XDocument.Load(@"template\template.xml");
            XNamespace ns = xdoc.Root?.GetDefaultNamespace();
            if (ns == null)
            {
                MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить корневой элемент)", @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            XElement body = xdoc.Root?.Element(ns + "body2");
            if (body != null)
            {
                XElement container = body.Element(ns + "container");
                if (container != null)
                {
                    #region Основная часть

                    XElement reg = container.Element(ns + "reg");
                    if (reg != null)
                    {
                        XElement datareg = reg.Element(ns + "datareg");
                        if (datareg != null)
                            datareg.Value = Convert.ToDateTime(dTP_dateOut.Value).ToShortDateString();
                    }

                    XElement header = container.Element(ns + "header");
                    if (header != null)
                    {
                        header.Value = "";
                    }

                    XElement content = container.Element(ns + "content");
                    if (content != null)
                    {
                        #region Основная часть

                        content.Value = "";

                        if (_decision)
                        {
                            switch (_idService)
                            {
                                case 297:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Присвоение правового статуса ребенку-сироте, " +
                                            "ребенку, оставшемуся без попечения родителей, лицу из числа детей-сирот и детей, " +
                                            "оставшихся без попечения родителей» принято в работу. О результатах обработки " +
                                            "Вы будете проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 301:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Назначение граждан опекунами, попечителями " +
                                            "несовершеннолетних детей» принято в работу. О результатах обработки Вы будете " +
                                            "проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 302:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача органом опеки и попечительства решения " +
                                            "об объявлении несовершеннолетнего, достигшего возраста 16 (шестнадцати) лет, полностью " +
                                            "дееспособным» принято в работу. О результатах обработки Вы будете проинформированы " +
                                            "в течении 15 дней.")
                                    );
                                    break;
                                case 304:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача органом опеки и попечительства " +
                                            "разрешения на заключение трудовых договоров с несовершеннолетними» принято в работу. " +
                                            "О результатах обработки Вы будете проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 305:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача разрешения органом опеки и " +
                                            "попечительства на изменение имени и фамилии ребенку (детям)» принято в работу. " +
                                            "О результатах обработки Вы будете проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 307:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Постановка на учет в качестве кандидатов " +
                                            "в усыновители, опекуны (попечители), граждан, выразивших желание принять ребенка на " +
                                            "воспитание в свою семью» принято в работу. О результатах обработки Вы будете " +
                                            "проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 308:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача разрешения на " +
                                            "посещение ребенка, оставшегося без попечения родителей» принято в работу. О результатах " +
                                            "обработки Вы будете проинформированы в течении 10 дней.")
                                    );
                                    break;
                                case 309:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Временная передача детей, находящихся в " +
                                            "организациях обеспечивающих содержание, образование и воспитание детей-сирот и детей, " +
                                            "оставшихся без попечения родителей, в семьи граждан, постоянно проживающих на " +
                                            "территории Приднестровской Молдавской Республики» принято в работу. О результатах " +
                                            "обработки Вы будете проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 314:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Установление патронажа над совершеннолетними " +
                                            "дееспособными гражданами, которые по состоянию здоровья не могут самостоятельно " +
                                            "осуществлять и защищать свои права и исполнять свои обязанности» принято в работу. " +
                                            "О результатах обработки Вы будете проинформированы в течении 15 дней.")
                                    );
                                    break;
                                case 318:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача разрешений на " +
                                            "выезд из Приднестровской Молдавской Республики несовершеннолетних граждан " +
                                            "Приднестровской Молдавской Республики, оставшихся без попечения родителей, в том " +
                                            "числе находящихся в организациях для детей-сирот и детей, оставшихся без попечения " +
                                            "родителей» принято в работу. О результатах обработки Вы будете проинформированы " +
                                            "в течении 15 дней.")
                                    );
                                    break;
                                case 319:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача органом опеки и попечительства " +
                                            "разрешения (согласия) на совершение сделок, связанных с имуществом, принадлежащим " +
                                            "подопечному» принято в работу. О результатах обработки Вы будете проинформированы " +
                                            "в течении 15 дней.")
                                    );
                                    break;
                                case 321:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Ваше заявление на предоставление услуги «Выдача согласия на приватизацию жилого " +
                                            "дома, жилого помещения, в которых проживают и зарегистрированы совершеннолетние " +
                                            "недееспособные или не полностью дееспособные граждане, несовершеннолетние " +
                                            "дети-сироты, дети, оставшиеся без попечения родителей» принято в работу. " +
                                            "О результатах обработки Вы будете проинформированы в течении 15 дней.")
                                    );
                                    break;
                                default:
                                    {
                                        int day = dTP_consid.Value.Subtract(DateTime.Now).Days + 1;
                                        string dayStr = day == 0
                                            ? "сегодняшнего дня"
                                            : day + " " + GetDeclension(day, "дня", "дней", "дней");
                                        content.Add(
                                            new XElement(ns + "p",
                                                "Уважаемый(ая) " + tB_fio.Text +
                                                ". Ваше заявление на предоставление услуги «" +
                                                tB_service.Text +
                                                "» принято в работу. О результатах обработки Вы будете проинформированы в течении " +
                                                dayStr + ".")
                                        );
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            switch (_idService)
                            {
                                case 297:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Присвоение правового " +
                                            "статуса ребенку-сироте, ребенку, оставшемуся без попечения родителей, лицу из числа " +
                                            "детей-сирот и детей, оставшихся без попечения родителей» по причине " +
                                            tB_denial.Text + ".")
                                    );
                                    break;
                                case 301:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Назначение граждан опекунами, " +
                                            "попечителями несовершеннолетних детей» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                case 302:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Выдача органом опеки и " +
                                            "попечительства решения об объявлении несовершеннолетнего, достигшего возраста 16 (шестнадцати) " +
                                            "лет, полностью дееспособным» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                case 304:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги " +
                                            "«Выдача органом опеки и попечительства разрешения на заключение трудовых договоров с " +
                                            "несовершеннолетними» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                case 305:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Выдача разрешения органом " +
                                            "опеки и попечительства на изменение имени и фамилии ребенку (детям)» по причине " +
                                            tB_denial.Text + ".")
                                    );
                                    break;
                                case 307:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Постановка на учет " +
                                            "в качестве кандидатов в усыновители, опекуны (попечители), граждан, выразивших желание принять " +
                                            "ребенка на воспитание в свою семью» по причине " +
                                            tB_denial.Text + ".")
                                    );
                                    break;
                                case 308:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Выдача разрешения на " +
                                            "посещение ребенка, оставшегося без попечения родителей» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                case 309:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Временная передача " +
                                            "детей, находящихся в организациях обеспечивающих содержание, образование и воспитание " +
                                            "детей-сирот и детей, оставшихся без попечения родителей, в семьи граждан, постоянно " +
                                            "проживающих на территории Приднестровской Молдавской Республики» по причине " +
                                            tB_denial.Text + ".")
                                    );
                                    break;
                                case 314:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Установление " +
                                            "патронажа над совершеннолетними дееспособными гражданами, которые по состоянию " +
                                            "здоровья не могут самостоятельно осуществлять и защищать свои права и исполнять " +
                                            "свои обязанности» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                case 318:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Выдача " +
                                            "разрешений на выезд из Приднестровской Молдавской Республики несовершеннолетних " +
                                            "граждан Приднестровской Молдавской Республики, оставшихся без попечения родителей, " +
                                            "в том числе находящихся в организациях для детей-сирот и детей, оставшихся без " +
                                            "попечения родителей» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                case 319:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Выдача " +
                                            "органом опеки и попечительства разрешения (согласия) на совершение сделок, " +
                                            "связанных с имуществом, принадлежащим подопечному» по причине " +
                                            tB_denial.Text + ".")
                                    );
                                    break;
                                case 321:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «Выдача " +
                                            "согласия на приватизацию жилого дома, жилого помещения, в которых проживают " +
                                            "и зарегистрированы совершеннолетние недееспособные или не полностью " +
                                            "дееспособные граждане, несовершеннолетние дети-сироты, дети, оставшиеся " +
                                            "без попечения родителей» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                                default:
                                    content.Add(
                                        new XElement(ns + "p",
                                            "Уважаемый(ая) " + tB_fio.Text +
                                            ". Вам отказано в принятии в работу Заявления на предоставление услуги «" +
                                            tB_service.Text + "» по причине " + tB_denial.Text + ".")
                                    );
                                    break;
                            }
                        }

                        #endregion
                    }

                    XElement executor = container.Element(ns + "executor");
                    if (executor != null)
                    {
                        XElement executorname = executor.Element(ns + "executorname");
                        if (executorname != null)
                            executorname.Value = "Исполнитель - " + tB_operator.Text;
                        XElement executorphone = executor.Element(ns + "executorphone");
                        if (executorphone != null)
                            executorphone.Value = "Контактный телефон - " + tB_operatorTel.Text;
                        XElement executordate = executor.Element(ns + "executordate");
                        if (executordate != null)
                            executordate.Value = "";
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
                    MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить тег контейнера)",
                        @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                XElement servinfo = body.Element(ns + "servinfo");
                if (servinfo != null)
                {
                    XElement signaturesxml = servinfo.Element(ns + "signaturesxml");
                    if (signaturesxml != null)
                    {
                        signaturesxml.Value = "";
                    }
                    XElement idgosuslug = servinfo.Element(ns + "idgosuslug");
                    if (idgosuslug == null)
                    {
                        idgosuslug = new XElement(
                            ns + "idgosuslug",
                            _idGosUslugi,
                            new XAttribute("style", "display: none !important;"));
                        servinfo.Add(idgosuslug);
                    }
                    else
                    {
                        idgosuslug.Value = _idGosUslugi;
                        idgosuslug.Add(new XAttribute("style", "display: none !important;"));
                    }
                }
                else
                {
                    MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить тег сервисной информации)",
                        @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }
            else
            {
                MessageBox.Show(@"Ошибка при формировании документа (Не удалось получить тег body2)",
                    @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            try
            {
                xdoc.Save(_path, SaveOptions.DisableFormatting);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Не удалось сохранить документ: " + ex.Message, @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            return true;
        }


        #region UpdateDb

        /// <summary>
        /// Обновление данных в базе данных MySQL.
        /// </summary>
        private bool UpdateMySql()
        {
            int file = SaveFile();
            if (file == 0)
                return false;

            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandText = "UPDATE documents SET " +
                                  "fio = @fio, " +
                                  "status = @status, " +
                                  "date_ispoln = @date_ispoln, " +
                                  "file_apply = @file_apply, " +
                                  "flag_file_apply = @flag_file_apply " +
                                  "WHERE id = @id";

                cmd.Parameters.AddWithValue("@fio", tB_fio.Text);
                cmd.Parameters.AddWithValue("@status", _decision ? 3 : 2);
                cmd.Parameters.AddWithValue("@date_ispoln", _decision ? dTP_consid.Value : DateTime.Now);
                cmd.Parameters.AddWithValue("@file_apply", file);
                cmd.Parameters.AddWithValue("@flag_file_apply", 0);
                cmd.Parameters.AddWithValue("@id", _id);
                cmd.ExecuteNonQuery();
                Logger.Log.Warn("[" + _id + "] " + (_decision ? "Заявка принята" : "Отказано в рассмотрении"));
                Connection.AddLogs(_id.ToString(), _decision ? "Заявка принята" : "Отказано в рассмотрении");
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("[" + _id + "] Возникла ошибка обновления записи: " + ex.Message);
                DelFile(file);
                Connection.AddLogs(_id.ToString(), "Возникла ошибка обновления записи: " + ex.Message);
                MessageBox.Show(@"Возникла ошибка обновления записи:" + Environment.NewLine + ex.Message, 
                    @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Сохранить файл в базе данных MySQL
        /// </summary>
        private int SaveFile()
        {
            int serviceResult = _decision ? 1 : 0;
            string param = "{\"serviceId\":\"" + _idGosUslugi + "\", \"serviceResult\":" + serviceResult + "}";
            string signature = "";

            bool flagDocSig = false;
            List<MyCert> cert = SignDoc.GelAllCertificates();
            foreach (var c in cert)
            {
                if (c.ToString() == UserInfo.Sert)
                {
                    try
                    {
                        XmlDocument xmlDocument = new XmlDocument { PreserveWhitespace = true };
                        xmlDocument.Load(_path);
                        var xml1 = xmlDocument.InnerXml;
                        xmlDocument = SignDoc.FileSignCadesBesX(xmlDocument, c);
                        var xml2 = xmlDocument.InnerXml;
                        WorkMethods.SaveFileXml(xmlDocument, _path);
                        flagDocSig = !Equals(xml1, xml2);

                        string fileParam = WorkMethods.GetFileParam();
                        if (fileParam != "")
                        {
                            fileParam = fileParam.Replace("<container></container>",
                                "<container>" + param + "</container>");
                            signature =
                                WorkMethods.GetSignaturesFromStream(
                                    SignDoc.FileSignCadesBes(WorkMethods.GenerateStreamFromString(fileParam), c));
                        }
                        else
                        {
                            using (Stream stream = WorkMethods.GenerateStreamFromString(param))
                            {
                                signature = WorkMethods.GenerateStringFromStream(SignDoc.FileSignCadesBes(stream, c));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        string mes = "Возникла ошибка при подписании: " + ex.Message;
                        Logger.Log.Warn("[" + _id + "] " + mes);
                        Connection.AddLogs(_id.ToString(), "Возникла ошибка при подписании: " + ex.Message);
                        MessageBox.Show(mes);
                        return 0;
                    }
                }
            }

            if (signature == string.Empty || !flagDocSig)
            {
                string mes = "Документ не подписан";
                Logger.Log.Warn("[" + _id + "] " + mes);
                Connection.AddLogs(_id.ToString(), mes);
                MessageBox.Show(mes + @"." + Environment.NewLine + @"Возможно не выбран сертификат в настройках программы.",
                    @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }


            string pathFtp = "/" + _idGosUslugi + "/apply.xml";
            if (!MyFtp.UploadToFtp(_path, pathFtp))
            {
                string mes = "Возникла ошибка при загрузке файл на FTP-сервер.";
                Logger.Log.Warn("[" + _id + "] " + mes);
                Connection.AddLogs(_id.ToString(), mes + " Файл: " + pathFtp);
                MessageBox.Show(mes, @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
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

                cmd.Parameters.AddWithValue("@file", pathFtp);
                cmd.Parameters.AddWithValue("@service_result", _decision ? 1 : 0);
                cmd.Parameters.AddWithValue("@params", param);
                cmd.Parameters.AddWithValue("@param_signature", signature);
                cmd.ExecuteNonQuery();
                cmd = conn.CreateCommand();
                cmd.CommandText += "SELECT @@IDENTITY";    //Получение id
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                Logger.Log.Info("[" + _id + "] Файл сохранён в базе данных. ID: " + id);
                return id;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("[" + _id + "] Возникла при добавлении файла в БД: " + ex.Message);
                Connection.AddLogs(_id.ToString(), "Возникла при добавлении файла в БД: " + ex.Message);
                MessageBox.Show(@"Возникла при добавлении файла в БД:" + Environment.NewLine + ex.Message,
                    @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Удаление файла по ID
        /// </summary>
        /// <param name="fileId">ID файла</param>
        private void DelFile(int fileId)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM files " +
                              "WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", fileId);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("[" + _id + "] " + "Ошибка удаления файла из базы данных: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion


        /// <summary>
        /// Возвращает слова в падеже, зависимом от заданного числа 
        /// </summary>
        /// <param name="number">Число от которого зависит выбранное слово</param>
        /// <param name="nominativ">Именительный падеж слова. Например "день"</param>
        /// <param name="genetiv">Родительный падеж слова. Например "дня"</param>
        /// <param name="plural">Множественное число слова. Например "дней"</param>
        /// <returns></returns>
        public static string GetDeclension(int number, string nominativ, string genetiv, string plural)
        {
            number = number % 100;
            if (number >= 11 && number <= 19)
            {
                return plural;
            }

            var i = number % 10;
            switch (i)
            {
                case 1:
                    return nominativ;
                case 2:
                case 3:
                case 4:
                    return genetiv;
                default:
                    return plural;
            }

        }

        #endregion
    }
}

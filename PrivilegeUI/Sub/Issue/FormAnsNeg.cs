using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using PrivilegeAPI.Services;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using System.Xml.Linq;
using System.Xml.Serialization;
using static PrivilegeUI.Models.FullApplication;
using Application = PrivilegeAPI.Models.Application;
using File = System.IO.File;
using FtpSettings = PrivilegeAPI.Services.FtpSettings;

namespace PrivilegeUI.Sub.Issue
{
    public partial class FormAnsNeg : Form
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
        /// Путь до исходящего документа
        /// </summary>
        private string _path;
        private Application _app;
        private Htmlx fullApp;

        private FtpSettings ftpSettings;
        private FtpService ftpContext;

        #endregion


        #region Constructor

        public FormAnsNeg(MyHttpClient apiClient, Application app, FormMain parentForm)
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

        private async void FormAnsNeg_Load(object sender, EventArgs e)
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
                dTP_dateOut.Value = DateTime.Now;
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

            if (tB_denial.Text == "")
            {
                string caption = "Это поле не может быть пустым";
                lbl_denial.ForeColor = Color.Red;
                toolTip1.SetToolTip(lbl_denial, caption);
                toolTip1.SetToolTip(tB_denial, caption);
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

            if (!await GenerateHtmlxAndUploadAsync(fullApp.Body2.Servinfo.Idservice, fullApp.Body2.Servinfo.Nameservice, tB_fio.Text, tB_operator.Text, tB_operatorTel.Text, tB_denial.Text))
                return false;

            return true;
        }

        /// <summary>
        /// Формирование документа
        /// </summary>
        /// <returns></returns>
        public async Task<bool> GenerateHtmlxAndUploadAsync(int idGosUslugi, string service, string fio, string operatorName, string operatorPhone, string reason)
        {
            try
            {
                string baseTempPath = Path.Combine(Path.GetTempPath(), "Applications", "AnswerNeg");
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
                    content.Add(new XElement(ns + "p", styleAttr, new XText("Ваша заявка отклонена по причине: \"" + reason + "\".")));
                }

                var executor = container.Element(ns + "executor");
                executor?.Element(ns + "executorname")?.SetValue(operatorName);
                executor?.Element(ns + "executorphone")?.SetValue(operatorPhone);
                executor?.Element(ns + "executordate")?.SetValue(DateTime.Now.ToString("dd.MM.yyyy"));

                var docstatus = container.Element(ns + "docstatus");
                docstatus?.Element(ns + "datedocexecutor")?.SetValue(DateTime.Now.ToString("g"));

                var reg = container.Element(ns + "reg");
                reg?.Element(ns + "datareg")?.SetValue(DateTime.Now.ToString("dd.MM.yyyy"));

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
                string tempFilePath = FileHelper.PrepareTempFilePath(tempFileName);
                xdoc.Save(tempFilePath, SaveOptions.DisableFormatting);

                string ftpFilePath = $"Applications/AnswerNeg/{tempFileName}";
                await ftpContext.SaveFileAsync(ftpFilePath, tempFilePath);
                await ftpContext.DisconnectAsync();

                await UpdateAsync();

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
                string tempPath = FileHelper.PrepareTempFilePath(fileName);

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

        ///// <summary>
        ///// Обновление данных
        ///// </summary>
        private async Task<bool> UpdateAsync()
        {
            try
            {
                ApplicationDto applicationDto = new ApplicationDto
                {
                    Id = _app.Id,
                    Status = StatusEnum.DenialApply
                };

                var result = await _apiClient.PutAsync<ApplicationDto, BaseResult<ApplicationDto>>("api/applications", applicationDto);

                if (!(result == null) && result.IsSuccess)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Возникла ошибка обновления записи:" + Environment.NewLine + ex.Message,
                    @"Ошибка обновления", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #endregion
    }
}

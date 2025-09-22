using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using PrivilegeAPI.Services;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static PrivilegeUI.Models.FullApplication;
using Application = PrivilegeAPI.Models.Application;
using File = System.IO.File;
using FtpSettings = PrivilegeAPI.Services.FtpSettings;

namespace PrivilegeUI.Sub.Issue
{
    public partial class FormApply : Form
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
        /// Текущая заявка 
        /// </summary>
        private Application _app;
        private Htmlx fullApp;

        private FtpSettings ftpSettings;
        private FtpService ftpContext;
        /// <summary>
        /// Флаг принятого документа (отказаного)
        /// </summary>
        private readonly bool _decision = true;

        #endregion


        #region Constructor

        public FormApply(MyHttpClient apiClient, Application app, FormMain parentForm, bool decision = true)
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
            _decision = decision;

            if (!_decision)
            {
                lbl_denial.Visible = true;
                tB_denial.Visible = true;
                lbl_consid.Visible = false;
                dTP_consid.Visible = false;
            }
        }

        #endregion


        #region Events

        private async void FormApply_Load(object sender, EventArgs e)
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

                string fio = fullApp.Body2.Container.Content.P[2].Person.Fio.Fam + " " +
                             fullApp.Body2.Container.Content.P[2].Person.Fio.Im + " " +
                             fullApp.Body2.Container.Content.P[2].Person.Fio.Ot;

                string adress = fullApp.Body2.Container.Content.P[2].Person.AdressProj.Row.Raion + ", " +
                                fullApp.Body2.Container.Content.P[2].Person.AdressProj.Row.Ulica + ", " +
                                fullApp.Body2.Container.Content.P[2].Person.AdressProj.Row.Dom;

                tB_operator.Text = UserInfo.CurrentUser.Name;
                tB_operatorTel.Text = Properties.Settings.Default.UserTel;
                tB_service.Text = fullApp.Body2.Container.Header.ToString();
                tB_fio.Text = fio;
                dTP_consid.Value = DateTime.Now.AddDays(3);
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

            bool ok = await CreateDocAsync();
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
            bool ok = await CreateDocAsync();
            if (!ok)
                return;
        }

        #endregion


        #region Methods

        /// <summary>
        /// Создать документ
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CreateDocAsync()
        {
            if (_app.Status != StatusEnum.Delivered)
            {
                string errorMessage = _decision
                    ? "Заявка уже принята в работу"
                    : "Заявка уже отклонена";

                MessageBox.Show(errorMessage, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            //if (!await GenerateHtmlxAndUploadAsync(fullApp.Body2.Servinfo.Idservice, fullApp.Body2.Servinfo.Nameservice, tB_fio.Text, tB_operator.Text, tB_operatorTel.Text))
            //    return false;

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

                    if (_decision)
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
                    }
                    else
                    {
                        content.Add(
                            new XElement(ns + "p",
                                "Уважаемый(ая) " + tB_fio.Text +
                                ". Вам отказано в принятии в работу Заявления на предоставление услуги «" +
                                tB_service.Text + "» по причине: " + tB_denial.Text + ".")
                            );
                    }
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

                if (_decision)
                {
                    string tempFileName = $"answer_{_app.File.Name}";
                    string tempFilePath = FileHelper.PrepareTempFilePath(tempFileName);
                    xdoc.Save(tempFilePath, SaveOptions.DisableFormatting);

                    string ftpFilePath = $"Applications/Apply/{tempFileName}";
                    string xmlcontent = await File.ReadAllTextAsync(tempFilePath);
                    await ftpContext.SaveFileAsync(ftpFilePath, xmlcontent);
                    await ftpContext.DisconnectAsync();
                }
                else
                {
                    string tempFileName = $"answer_{_app.File.Name}";
                    string tempFilePath = FileHelper.PrepareTempFilePath(tempFileName);
                    xdoc.Save(tempFilePath, SaveOptions.DisableFormatting);

                    string ftpFilePath = $"Applications/Denial/{tempFileName}";
                    string xmlcontent = await File.ReadAllTextAsync(tempFilePath);
                    await ftpContext.SaveFileAsync(ftpFilePath, xmlcontent);
                    await ftpContext.DisconnectAsync();
                }

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

        /// <summary>
        /// Обновление данных
        /// </summary>
        private async Task<bool> UpdateAsync()
        {
            try
            {
                ApplicationDto applicationDto;

                if (_decision)
                {
                    applicationDto = new ApplicationDto
                    {
                        Id = _app.Id,
                        Status = StatusEnum.Apply
                    };
                }
                else
                {
                    applicationDto = new ApplicationDto
                    {
                        Id = _app.Id,
                        Status = StatusEnum.Denial
                    };
                }

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

using PrivilegeAPI;
using PrivilegeAPI.Services;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using System.Xml.Serialization;
using static PrivilegeUI.Models.FullApplication;
using Application = PrivilegeAPI.Models.Application;
using FtpSettings = PrivilegeAPI.Services.FtpSettings;

namespace PrivilegeUI.Sub
{
    public partial class FormInfo : Form
    {
        #region Fields

        private readonly MyHttpClient _apiClient;
        private readonly int _orderId;
        private FtpSettings ftpSettings;
        private FtpService ftpContext;
        private Application _app;
        private FormMain _parentForm;

        #endregion


        #region Constructor

        public FormInfo(MyHttpClient apiClient, Application app, FormMain parentForm)
        {
            InitializeComponent();
            _apiClient = apiClient;
            ftpSettings = new FtpSettings
            {
                Server = Properties.Settings.Default.ServerFTP,
                Username = Properties.Settings.Default.UserFTP,
                Password = Properties.Settings.Default.PassFTP,
                Port = Properties.Settings.Default.PortFTP
            };
            ftpContext = new FtpService(ftpSettings);
            _app = app;
            _orderId = _app.Id;
            _parentForm = parentForm;

            UpdateButtonsVisibility();
        }

        #endregion


        #region Events

        private async void FormInfo_Load(object sender, EventArgs e)
        {
            await LoadOrderAsync();
        }

        private async Task LoadOrderAsync()
        {
            try
            {
                if (_orderId <= 0)
                {
                    MessageBox.Show("Неверный идентификатор заявки.");
                    Close();
                    return;
                }

                var fullApp = await LoadHtmlxFromApiAsync(_app.Orgnumber);
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

                tB_service.Text = fullApp.Body2.Container.Header.Text;
                tB_serviceId.Text = fullApp.Body2.Servinfo.Idgosuslug.ToString();
                tB_npp.Text = _app.Id.ToString();
                tB_fio.Text = fio;
                tB_address.Text = adress;
                tB_status.Text = EnumHelper.GetEnumDisplayName(_app.Status);
                tB_dateAdd.Text = fullApp.Body2.Container.Dateblank;
                tB_dateEnd.Text = _app.DateEdit.ToShortDateString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке заявки: " + ex.Message);
            }
        }

        private async void btn_fileAddDownload_Click(object sender, EventArgs e)
        {
            try
            {
                await DownloadAndOpenFile($"Applications/New/Application_{_app.Orgnumber}.xml", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        private async void btn_fileAddOpen_Click(object sender, EventArgs e)
        {
            try
            {
                await DownloadAndOpenFile($"Applications/New/Application_{_app.Orgnumber}.xml", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_fileApplyDownload_Click(object sender, EventArgs e)
        {
        }

        private void btn_fileApplyOpen_Click(object sender, EventArgs e)
        {
        }

        private async void btn_fileArrivedDownload_Click(object sender, EventArgs e)
        {
            try
            {
                await DownloadAndOpenFile($"Applications/Reply/Application_{_app.Orgnumber}.xml", false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        private async void btn_fileArrivedOpen_Click(object sender, EventArgs e)
        {
            try
            {
                await DownloadAndOpenFile($"Applications/Reply/Application_{_app.Orgnumber}.xml", true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
            }
        }

        private async void btn_fileFinalyDownload_Click(object sender, EventArgs e)
        {
            if (_app.Status == PrivilegeAPI.Models.StatusEnum.Final)
            {
                await DownloadAndOpenFile($"Applications/Final/Application_{_app.Orgnumber}.xml", false);
            }
            else if (_app.Status == PrivilegeAPI.Models.StatusEnum.DenialFinal)
            {
                await DownloadAndOpenFile($"Applications/Final/Application_{_app.Orgnumber}.xml", false);
            }
            else
            {
                MessageBox.Show("Невозможно загрузить файл, так как заявка не завершена.");
            }
        }

        private async void btn_fileFinalyOpen_Click(object sender, EventArgs e)
        {
            if (_app.Status == PrivilegeAPI.Models.StatusEnum.Final)
            {
                await DownloadAndOpenFile($"Applications/Final/Application_{_app.Orgnumber}.xml", true);
            }
            else if (_app.Status == PrivilegeAPI.Models.StatusEnum.DenialFinal)
            {
                await DownloadAndOpenFile($"Applications/Final/Application_{_app.Orgnumber}.xml", true);
            }
            else
            {
                MessageBox.Show("Невозможно открыть файл, так как заявка не завершена.");
            }
        }

        private async Task DownloadAndOpenFile(string remotePath, bool openAfterDownload)
        {
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), Path.GetDirectoryName(remotePath) ?? string.Empty);
                Directory.CreateDirectory(tempDir);

                string fileName = Path.GetFileName(remotePath);
                string localPath = Path.Combine(tempDir, fileName);

                if (!File.Exists(localPath))
                {
                    var response = await _apiClient.GetRawAsync($"/api/ftp/file?remotePath={Uri.EscapeDataString(remotePath)}");

                    if (!response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Файл не найден на сервере: " + response.ReasonPhrase);
                        return;
                    }

                    using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }

                if (openAfterDownload)
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = localPath,
                        UseShellExecute = true,
                        WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с файлом: {ex.Message}");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _parentForm?.ResetButton();
            Close();
        }

        #endregion


        #region Methods

        private void UpdateButtonsVisibility()
        {
            switch (_app.Status)
            {
                case PrivilegeAPI.Models.StatusEnum.Apply:
                    btn_fileApplyDownload.Visible = true;
                    btn_fileApplyOpen.Visible = true;
                    btn_fileFinalyDownload.Visible = false;
                    btn_fileFinalyOpen.Visible = false;
                    lbl_finaly.Visible = false;
                    break;

                case PrivilegeAPI.Models.StatusEnum.Denial:
                    btn_fileApplyDownload.Visible = true;
                    btn_fileApplyOpen.Visible = true;
                    btn_fileFinalyDownload.Visible = false;
                    btn_fileFinalyOpen.Visible = false;
                    lbl_apply.Text = "Отклонение заявки";
                    lbl_finaly.Visible = false;
                    break;

                case PrivilegeAPI.Models.StatusEnum.DenialFinal:
                    btn_fileApplyDownload.Visible = true;
                    btn_fileApplyOpen.Visible = true;
                    btn_fileFinalyDownload.Visible = true;
                    btn_fileFinalyOpen.Visible = true;
                    break;

                case PrivilegeAPI.Models.StatusEnum.Final:
                    btn_fileApplyDownload.Visible = true;
                    btn_fileApplyOpen.Visible = true;
                    btn_fileFinalyDownload.Visible = true;
                    btn_fileFinalyOpen.Visible = true;
                    break;

                case PrivilegeAPI.Models.StatusEnum.Delivered:
                    btn_fileApplyDownload.Visible = false;
                    btn_fileApplyOpen.Visible = false;
                    btn_fileFinalyDownload.Visible = false;
                    btn_fileFinalyOpen.Visible = false;
                    lbl_finaly.Visible = false;
                    lbl_apply.Visible = false;
                    break;
                default:
                    btn_fileApplyDownload.Visible = false;
                    btn_fileApplyOpen.Visible = false;
                    btn_fileArrivedDownload.Visible = false;
                    btn_fileArrivedOpen.Visible = false;
                    btn_fileFinalyDownload.Visible = false;
                    btn_fileFinalyOpen.Visible = false;
                    break;
            }
        }

        public async Task<Htmlx?> LoadHtmlxFromApiAsync(string orgnumber)
        {
            try
            {
                string remotePath = $"Applications/New/Application_{orgnumber}.xml";

                var response = await _apiClient.GetRawAsync($"/api/ftp/file?remotePath={Uri.EscapeDataString(remotePath)}");

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Ошибка при загрузке файла с сервера: " + response.ReasonPhrase);
                    return null;
                }

                string fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"')
                                  ?? Path.GetFileName(remotePath);

                string tempDir = Path.Combine(Path.GetTempPath(), Path.GetDirectoryName(remotePath) ?? string.Empty);
                Directory.CreateDirectory(tempDir);

                string tempPath = Path.Combine(tempDir, fileName);

                using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
                {
                    await response.Content.CopyToAsync(fs);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(Htmlx));
                using (FileStream fs = new FileStream(tempPath, FileMode.Open, FileAccess.Read))
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

        //public async Task<Htmlx> LoadHtmlxFromFtpAsync(string orgnumber)
        //{
        //    try
        //    {
        //        string remoteFtpPath = $"Applications/New/Application_{orgnumber}.xml";
        //        string fileName = Path.GetFileName(remoteFtpPath);
        //        string tempPath = FileHelper.PrepareTempFilePath(fileName);

        //        if (!File.Exists(tempPath))
        //        {
        //            await ftpContext.DownloadFileAsync(remoteFtpPath, tempPath);
        //        }

        //        XmlSerializer serializer = new XmlSerializer(typeof(Htmlx));
        //        using (FileStream fs = new FileStream(tempPath, FileMode.Open))
        //        {
        //            var htmlx = (Htmlx)serializer.Deserialize(fs);
        //            return htmlx;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ошибка при загрузке/десериализации XML: " + ex.Message);
        //        return null;
        //    }
        //}

        #endregion
    }
}
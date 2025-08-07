using PrivilegeAPI;
using PrivilegeAPI.Services;
using PrivilegeUI.Classes;
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

                var fullApp = await LoadHtmlxFromFtpAsync(_app.File.Path);
                if (fullApp == null)
                {
                    MessageBox.Show("Не удалось загрузить данные заявки.");
                    Close();
                    return;
                }

                tB_service.Text = fullApp.Body2.Servinfo.Nameservice;
                tB_serviceId.Text = fullApp.Body2.Servinfo.Idservice.ToString();
                tB_npp.Text = _app.Id.ToString();
                tB_fio.Text = fullApp.Body2.Container.Topheader.Tophead.PersData.Fam + " " + fullApp.Body2.Container.Topheader.Tophead.PersData.Im + " " + fullApp.Body2.Container.Topheader.Tophead.PersData.Ot;
                tB_address.Text = fullApp.Body2.Container.Topheader.Tophead.AdressProj.Row.Raion + ", " + fullApp.Body2.Container.Topheader.Tophead.AdressProj.Row.Ulica + ", " + fullApp.Body2.Container.Topheader.Tophead.AdressProj.Row.Dom;
                tB_status.Text = EnumHelper.GetEnumDisplayName(_app.Status);
                tB_dateAdd.Text = fullApp.Body2.Container.Dateblank.ToShortDateString();
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
                await DownloadAndOpenFile(_app.File.Path, false);
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
                await DownloadAndOpenFile(_app.File.Path, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btn_fileApplyDownload_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile(_app.File.Path, false);
        }

        private async void btn_fileApplyOpen_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile(_app.File.Path, true);
        }

        private async void btn_fileArrivedDownload_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile(_app.File.Path, false);
        }

        private async void btn_fileArrivedOpen_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile(_app.File.Path, true);
        }

        private async void btn_fileFinalyDownload_Click(object sender, EventArgs e)
        {
            if (_app.Status == PrivilegeAPI.Models.StatusEnum.Final)
            {
                await DownloadAndOpenFile("Applications/AnswerPos/answer_" + _app.File.Name, false);
            }
            else if (_app.Status == PrivilegeAPI.Models.StatusEnum.Denial)
            {
                await DownloadAndOpenFile("Applications/AnswerNeg/answer_" + _app.File.Name, false);
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
                await DownloadAndOpenFile("Applications/AnswerPos/answer_" + _app.File.Name, true);
            }
            else if (_app.Status == PrivilegeAPI.Models.StatusEnum.Denial)
            {
                await DownloadAndOpenFile("Applications/AnswerNeg/answer_" + _app.File.Name, true);
            }
            else
            {
                MessageBox.Show("Невозможно открыть файл, так как заявка не завершена.");
            }
        }

        private async Task DownloadAndOpenFile(string fileName, bool openAfterDownload)
        {
            try
            {
                string localPath = FileHelper.PrepareTempFilePath(fileName);
                string localDir = Path.GetDirectoryName(localPath);

                if (!Directory.Exists(localDir))
                {
                    Directory.CreateDirectory(localDir);
                }

                if (System.IO.File.Exists(localPath))
                {
                    if (openAfterDownload)
                    {
                        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                        {
                            FileName = localPath,
                            UseShellExecute = true,
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal
                        });
                    }
                    return;
                }

                await ftpContext.ConnectAsync();

                string remotePath = fileName;

                if (await ftpContext.FileExistsAsync(remotePath))
                {
                    using (var stream = await ftpContext.OpenReadAsync(remotePath))
                    using (var fileStream = System.IO.File.Create(localPath))
                    {
                        await stream.CopyToAsync(fileStream);
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
                else
                {
                    MessageBox.Show("Файл не найден на сервере.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с файлом: {ex.Message}");
            }
            finally
            {
                await ftpContext.DisconnectAsync();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _parentForm?.ResetButton();
            Close();
        }
        #endregion


        #region Methods

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

        #endregion
    }
}
using Microsoft.AspNetCore.Http;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows.Forms;
using Application = PrivilegeAPI.Models.Application;
using FtpSettings = PrivilegeAPI.Services.FtpSettings;

namespace PrivilegeUI.Sub
{
    public partial class FormInfo : Form
    {
        #region Fields

        private readonly HttpClient _apiClient;
        private readonly int _orderId;
        private FtpSettings ftpSettings;
        private FtpService ftpContext;
        
        #endregion


        #region Constructor

        public FormInfo(HttpClient apiClient, int orderId)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _orderId = orderId;
            ftpSettings = new FtpSettings
            {
                Server = Properties.Settings.Default.ServerFTP,
                Username = Properties.Settings.Default.UserFTP,
                Password = Properties.Settings.Default.PassFTP,
                Port = Properties.Settings.Default.PortFTP
            };
            ftpContext = new FtpService(ftpSettings);
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

                var response = await _apiClient.GetAsync($"api/application/id/{_orderId}");
                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Ошибка при получении данных с сервера.");
                    Close();
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var app = JsonSerializer.Deserialize<Application>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (app == null)
                {
                    MessageBox.Show("Заявка не найдена.");
                    Close();
                    return;
                }

                tB_service.Text = app.Name;
                tB_npp.Text = app.Id.ToString();
                tB_status.Text = GetEnumDisplayName(app.Status);
                tB_dateAdd.Text = app.DateAdd.ToShortDateString();
                tB_dateEnd.Text = app.DateEdit.ToShortDateString();
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
                await ftpContext.ConnectAsync();

                string remoteFilePath = "/test.txt";
                string localFilePath = Path.Combine(Path.GetTempPath(), Path.GetFileName(remoteFilePath));

                if (await ftpContext.FileExistsAsync(remoteFilePath))
                {
                    await ftpContext.DownloadFileAsync(remoteFilePath, localFilePath);

                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = localFilePath,
                        UseShellExecute = true
                    });

                    MessageBox.Show("Файл успешно загружен и открыт.");
                }
                else
                {
                    MessageBox.Show("Файл не найден на сервере.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке файла: {ex.Message}");
            }
            finally
            {
                await ftpContext.DisconnectAsync();
            }
        }

        private async void btn_fileAddOpen_Click(object sender, EventArgs e)
        {
            try
            {
                await ftpContext.ConnectAsync();

                string remotePath = "/test.txt";
                string tempPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(remotePath));

                using (var stream = await ftpContext.OpenReadAsync(remotePath))
                using (var fileStream = System.IO.File.Create(tempPath))
                {
                    await stream.CopyToAsync(fileStream);
                }

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = tempPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                await ftpContext.DisconnectAsync();
            }
        }

        private async void btn_fileApplyDownload_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile("apply", $"doc{_orderId}.xml", false);
        }

        private async void btn_fileApplyOpen_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile("apply", $"doc{_orderId}.xml", true);
        }

        private async void btn_fileArrivedDownload_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile("arrived", $"doc{_orderId}.xml", false);
        }

        private async void btn_fileArrivedOpen_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile("arrived", $"doc{_orderId}.xml", true);
        }

        private async void btn_fileFinalyDownload_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile("final", $"doc{_orderId}.xml", false);
        }

        private async void btn_fileFinalyOpen_Click(object sender, EventArgs e)
        {
            await DownloadAndOpenFile("final", $"doc{_orderId}.xml", true);
        }

        private async Task DownloadAndOpenFile(string category, string fileName, bool openAfterDownload)
        {
            try
            {
                string localDir = Path.Combine(Path.GetTempPath(), category);
                Directory.CreateDirectory(localDir);
                string localPath = Path.Combine(localDir, fileName);

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

                string remotePath = $"{category}/{fileName}";

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

        public static string GetEnumDisplayName(Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .FirstOrDefault()?
                        .GetCustomAttribute<DisplayAttribute>()?
                        .Name ?? value.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}

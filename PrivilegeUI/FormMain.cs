using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Privilege.UI.Classes;
using Privilege.UI.Window.Client.Sub.Issue;
using Privilege.UI.Window.Client.Sub;
using PrivilegeUI.Models;
using System;
using System.Drawing;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;
using Application = PrivilegeUI.Models.Application;
using HubConnection = Microsoft.AspNetCore.SignalR.Client.HubConnection;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PrivilegeUI
{
    public partial class FormMain : Form
    {
        private HubConnection _hubConnection;
        private readonly string _apiBaseUrl = "https://localhost:7227";
        private readonly HttpClient _httpClient;
        private DataGridView _dataGridView;

        public FormMain()
        {
            InitializeComponent();
            InitializeDataGridView();

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);

            this.SavingOn();
            InitializeSignalR();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            //string[] ver = Application.ProductVersion.Split('.');
            //Text += @" v" + ver[0] + @"." + ver[1];
            //if (ver[2] != 0.ToString())
            //{
            //    Text += @"." + ver[2];
            //    if (ver[3] != 0.ToString())
            //        Text += @"." + ver[3];
            //}
            //else
            //{
            //    if (ver[3] != 0.ToString())
            //        Text += @"." + ver[2] + @"." + ver[3];
            //}

            //Text += @"  [" + UserInfo.Login + @"]";
        }

        private void FormMain_Shown(object sender, EventArgs e)
        {
            //обновление
            //CheckUpdate(false);

            ////проверка настроек и если не все заполнены, то открытие открытие окна настроек
            //if (Connection.Ftp == null || 
            //    (Connection.Ftp.Ip == "" && Connection.Ftp.User == "") ||
            //    !MyFtp.CheckConnection())
            //{
            //    ActivateButton(btn_settings);
            //    OpenChildForm(new FormSettings(this));
            //}

            //LoadInfo();
            //timer_refresh.Start();
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_refresh.Stop();
            try
            {
                if (System.IO.Directory.Exists("temp"))
                    System.IO.Directory.Delete(@"temp\", true);
            }
            catch (Exception ex)
            {
                Logger.Log.Warn(ex.Message);
            }
        }

        private void pB_header_Click(object sender, EventArgs e)
        {
            //ResetButton();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            //LoadInfo();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                //int.TryParse(dGV.CurrentRow.Cells["id"].Value.ToString(), out int num);
                //ActivateButton(sender);
                //OpenChildForm(new FormInfo(this, num));
            }
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                //ActivateButton(sender);
                //OpenChildForm(new FormApply(this, dGV.CurrentRow));
            }
        }

        private void btn_denial_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                //ActivateButton(sender);
                //OpenChildForm(new FormApply(this, dGV.CurrentRow, false));
            }
        }

        private void btn_finaly_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                //ActivateButton(sender);
                //OpenChildForm(new FormAnsPos(this, dGV.CurrentRow));
            }
        }

        private void btn_finalyPaper_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                //ActivateButton(sender);
                //OpenChildForm(new FormAnsPaper(this, dGV.CurrentRow));
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                //ActivateButton(sender);
                //OpenChildForm(new FormAnsNeg(this, dGV.CurrentRow));
            }
        }

        private void btn_archive_Click(object sender, EventArgs e)
        {
            //if (backgroundWorker1.IsBusy)
            //{
            //    MessageBox.Show(@"В данный момент происходит загрузка данных", @"Внимание",
            //        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    return;
            //}

            //if (_currentChildForm != null)
            //{
            //    if (MessageBox.Show(@"Закрыть предыдущее окно?", @"Внимание",
            //        MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        ResetButton();
            //    else
            //        return;
            //}

            //timer_refresh.Stop();
            //if (_isArchive)
            //{
            //    lblTitleClildForm.Text = @"Главный экран";
            //    pB_CurrentChildForm.BackgroundImage = Properties.Resources.home_white_96;
            //    _isArchive = false;
            //}
            //else
            //{
            //    lblTitleClildForm.Text = @"Архив";
            //    pB_CurrentChildForm.BackgroundImage = Properties.Resources.winrar_white_40;
            //    _isArchive = true;
            //}
            //LoadInfo();
            //timer_refresh.Start();
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            //ActivateButton(sender);
            //OpenChildForm(new FormSettings(this));
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //CheckUpdate(true);
        }

        private void InitializeDataGridView()
        {
            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true
            };
            _dataGridView.Columns.Add("Id", "ID");
            _dataGridView.Columns.Add("FullName", "Full Name");
            _dataGridView.Columns.Add("ServiceName", "Service Name");
            _dataGridView.Columns.Add("ApplicationDate", "Application Date");
            _dataGridView.Columns.Add("BenefitCategory", "Benefit Category");
            _dataGridView.Columns.Add("CardNumber", "Card Number");
            _dataGridView.Columns.Add("ServiceId", "Service ID");
            this.Controls.Add(_dataGridView);
        }

        private async void InitializeSignalR()
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"{_apiBaseUrl}/xmlHub")
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<string>("ReceiveMessage", (message) =>
                {
                    this.Invoke((Action)(async () =>
                    {
                        MessageBox.Show(message, "SignalR Notification");
                        await LoadApplicationsAsync();
                    }));
                });

                await _hubConnection.StartAsync();
                MessageBox.Show("Connected to SignalR hub.");
                await LoadApplicationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to SignalR: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private async Task SendXmlAsync(string xmlContent)
        {
            try
            {
                var content = new StringContent(xmlContent, Encoding.UTF8, "text/xml");
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/xml-listener", content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                MessageBox.Show($"XML sent successfully: {responseContent}");
                await LoadApplicationsAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending XML: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private async Task LoadApplicationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/applications");
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var applications = JsonSerializer.Deserialize<Application[]>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                _dataGridView.Rows.Clear();
                foreach (var app in applications)
                {
                    _dataGridView.Rows.Add(app.Id, app.FullName, app.ServiceName, app.ApplicationDate.ToString("dd.MM.yyyy"), app.BenefitCategory, app.CardNumber, app.ServiceId);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading applications: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private async void SendXmlButton_Click(object sender, EventArgs e)
        {
            var xmlContent = "";
            await SendXmlAsync(xmlContent);
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hubConnection != null)
            {
                _hubConnection.StopAsync();
                _hubConnection.DisposeAsync();
            }
            _httpClient.Dispose();
            base.OnFormClosing(e);
        }
    }
}
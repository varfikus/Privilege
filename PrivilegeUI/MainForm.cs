using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrivilegeUI.Models;
using System;
using System.Drawing;
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
    public partial class MainForm : Form
    {
        private HubConnection _hubConnection;
        private readonly string _apiBaseUrl = "https://localhost:7227";
        private readonly HttpClient _httpClient;
        private DataGridView _dataGridView;

        public MainForm()
        {
            InitializeComponent();
            InitializeDataGridView();

            // Bypass SSL validation for development (REMOVE IN PRODUCTION)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            InitializeSignalR();
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
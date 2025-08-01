using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net;
using HubConnection = Microsoft.AspNetCore.SignalR.Client.HubConnection;

namespace PrivilegeUI
{
    public partial class MainForm : Form
    {
        private readonly HttpClient _httpClient;
        private HubConnection _hubConnection;
        private IHubProxy _hubProxy;
        private readonly string _apiBaseUrl = "https://localhost:7227"; 
        private readonly DataGridView _dataGridView;
        private readonly Button _refreshButton;

        public MainForm()
        {
            InitializeComponent();
            _httpClient = new HttpClient { BaseAddress = new Uri(_apiBaseUrl) };
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            this.Text = "Application Viewer";
            this.Size = new System.Drawing.Size(1000, 600);

            _dataGridView = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ReadOnly = true
            };
            this.Controls.Add(_dataGridView);

            _refreshButton = new Button
            {
                Text = "Refresh Data",
                Dock = DockStyle.Top,
                Height = 30
            };
            _refreshButton.Click += async (s, e) => await LoadApplicationsAsync();
            this.Controls.Add(_refreshButton);

            InitializeSignalR();
            LoadApplicationsAsync();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to SignalR: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private async Task LoadApplicationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/applications");
                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var applications = JsonConvert.DeserializeObject<List<Application>>(json);

                this.Invoke((Action)(() =>
                {
                    _dataGridView.DataSource = applications;
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading applications: {ex.Message}");
            }
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
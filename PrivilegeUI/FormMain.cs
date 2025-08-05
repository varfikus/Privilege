using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using PrivilegeUI.Sub;
using System.Data;
using System.Net.Http.Headers;
using System.Text.Json;
using Application = PrivilegeAPI.Models.Application;
using HubConnection = Microsoft.AspNetCore.SignalR.Client.HubConnection;

namespace PrivilegeUI
{
    public partial class FormMain : Form
    {
        private HubConnection _hubConnection;
        private readonly string _apiBaseUrl = "https://localhost:7227";
        private readonly HttpClient _httpClient;
        private System.Timers.Timer _connectionCheckTimer;
        /// <summary>
        /// Выбранная кнопка
        /// </summary>
        private Button _currentBtn;
        /// <summary>
        /// Выделеноие кнопки слева
        /// </summary>
        private readonly Panel _leftBorderBtn = new Panel();
        /// <summary>
        /// Выбранная форма
        /// </summary>
        private Form _currentChildForm;
        /// <summary>
        /// Текущая таблица "архивная"?
        /// </summary>
        private bool _isArchive = false;
        private MyHttpClient _apiClient;
        private readonly int _userId;

        private List<Application> _applications;

        public FormMain(int userId, MyHttpClient apiClient)
        {
            InitializeComponent();
            InitializeDataGridView();

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);

            _apiClient = apiClient;
            _userId = userId;
            panelMenu.Controls.Add(_leftBorderBtn);
            this.SavingOn();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        private async void FormMain_Shown(object sender, EventArgs e)
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

            var result = await _apiClient.GetAsync<BaseResult<UserDto>>($"api/users/id/{_userId}");

            if (result == null)
            {
                MessageBox.Show("Не удалось получить ответ от сервера.");
                Close();
                return;
            }

            if (!result.IsSuccess || result.Data == null)
            {
                MessageBox.Show($"Ошибка загрузки пользователя: {result.ErrorMessage ?? "Неизвестная ошибка"}");
                Close();
                return;
            }

            UserInfo.CurrentUser = new UserDto
            {
                Id = result.Data.Id,
                Login = result.Data.Login,
                Name = result.Data.Name
            };

            InitializeSignalR();
        }

        private void LoadApplications()
        {
            InitializeDataGridView();
            LoadApplicationsAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show($"Ошибка при загрузке заявок: {task.Exception?.Message}");
                }
            });
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
                MessageBox.Show(ex.Message, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pB_header_Click(object sender, EventArgs e)
        {
            //ResetButton();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadApplicationsAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    MessageBox.Show($"Ошибка при обновлении данных: {task.Exception?.Message}");
                }
            });
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow == null)
            {
                MessageBox.Show("Выберите строку для отображения информации.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(dGV.CurrentRow.Cells["id"].Value?.ToString(), out int id))
            {
                MessageBox.Show("Не удалось определить ID заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var app = _applications?.FirstOrDefault(a => a.Id == id);

            if (app == null)
            {
                MessageBox.Show("Заявка не найдена в списке.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ActivateButton(sender);
            OpenChildForm(new FormInfo(_apiClient, app, this));
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

        private void dGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow dgvr = dGV.Rows[e.RowIndex];
            var cell = dgvr.Cells["StatusEnum"];

            if (cell?.Value != null && Enum.TryParse(typeof(StatusEnum), cell.Value.ToString(), out object statusObj))
            {
                var status = (StatusEnum)statusObj;
                cell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                switch (status)
                {
                    case StatusEnum.Add:
                        cell.Style.BackColor = Color.Red;
                        cell.Style.ForeColor = Color.Gold;
                        break;

                    case StatusEnum.Delivered:
                        cell.Style.BackColor = Color.CornflowerBlue;
                        cell.Style.ForeColor = Color.Black;
                        break;

                    case StatusEnum.Apply:
                        cell.Style.BackColor = Color.Yellow;
                        cell.Style.ForeColor = Color.Black;
                        break;

                    case StatusEnum.Final:
                        cell.Style.BackColor = Color.Green;
                        cell.Style.ForeColor = Color.WhiteSmoke;
                        break;

                    case StatusEnum.DenialApply:
                        cell.Style.BackColor = Color.DeepSkyBlue;
                        cell.Style.ForeColor = Color.Black;
                        break;

                    default:
                        cell.Style.BackColor = Color.White;
                        cell.Style.ForeColor = Color.Black;
                        break;
                }
            }
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
                        await LoadApplicationsAsync();
                    }));
                });

                _hubConnection.Closed += async (error) =>
                {
                    await Task.Delay(5000);
                    await TryReconnectAsync();
                };

                _hubConnection.Reconnecting += (error) =>
                {
                    return Task.CompletedTask;
                };

                _hubConnection.Reconnected += (connectionId) =>
                {
                    this.Invoke((Action)(async () =>
                    {
                        LoadApplications();
                    }));
                    return Task.CompletedTask;
                };

                await _hubConnection.StartAsync();
                LoadApplications();

                StartConnectionMonitor();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к SignalR: {ex.Message}");
            }
        }

        private async Task TryReconnectAsync()
        {
            try
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    await _hubConnection.StartAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при попытке переподключения: {ex.Message}");
            }
        }

        private void StartConnectionMonitor()
        {
            _connectionCheckTimer = new System.Timers.Timer(10000);
            _connectionCheckTimer.Elapsed += async (sender, e) =>
            {
                if (_hubConnection.State == HubConnectionState.Disconnected)
                {
                    await TryReconnectAsync();
                }
            };
            _connectionCheckTimer.AutoReset = true;
            _connectionCheckTimer.Start();
        }

        private void InitializeDataGridView()
        {
            dGV.Columns.Clear();
            dGV.AutoGenerateColumns = false;
            dGV.AllowUserToAddRows = false;
            dGV.ReadOnly = true;
            dGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV.CellFormatting += dGV_CellFormatting;
            panelDesktop.Controls.Add(dGV);

            dGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "ID",
                DataPropertyName = "Id",
                Name = "Id",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Имя",
                DataPropertyName = "Name",
                Name = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Статус",
                DataPropertyName = "Status",
                Name = "Status",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Статус",
                DataPropertyName = "StatusEnum",
                Name = "StatusEnum",
                Visible = false
            });

            dGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Дата добавления",
                DataPropertyName = "DateAdd",
                Name = "DateAdd",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });

            dGV.Columns.Add(new DataGridViewTextBoxColumn
            {
                HeaderText = "Дата изменения",
                DataPropertyName = "DateEdit",
                Name = "DateEdit",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells
            });
        }

        private async Task LoadApplicationsAsync()
        {
            try
            {
                var response = await _apiClient.GetAsync<CollectionResult<Application>>("api/applications");

                if(response == null || !response.IsSuccess || response.Data == null)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {response?.ErrorMessage ?? "Неизвестная ошибка"}");
                    return;
                }

                var applications = response.Data;
                _applications = applications.ToList();

                dGV.Rows.Clear();

                foreach (var app in applications)
                {
                    dGV.Rows.Add(
                        app.Id,
                        app.Name,
                        EnumHelper.GetEnumDisplayName(app.Status),
                        app.Status,
                        app.DateAdd,
                        app.DateEdit,
                        app.FileId
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading applications: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        private void ActivateButton(object senderBtn)
        {
            timer_refresh.Stop();
            DisableButton();

            _currentBtn = senderBtn as Button;
            if (_currentBtn == null) return;

            pB_CurrentChildForm.Image = _currentBtn.Image;
            lblTitleClildForm.Text = _currentBtn.Tag?.ToString();
        }

        private void DisableButton()
        {
            if (_currentBtn != null)
            {
                _currentBtn.BackColor = Color.FromArgb(60, 91, 116);
                _currentBtn.ForeColor = Color.Gainsboro;
            }
        }

        public void ResetButton()
        {
            DisableButton();
            LoadApplications();
            _currentChildForm?.Close();
            _currentChildForm = null;
            _leftBorderBtn.Visible = false;
            pB_CurrentChildForm.Image = _isArchive ? Properties.Resources.winrar_white_40 : Properties.Resources.home_white_96;
            lblTitleClildForm.Text = _isArchive ? @"Архив" : @"Главный экран";
            timer_refresh.Start();
        }

        private void OpenChildForm(Form childForm)
        {
            _currentChildForm?.Close();

            panelDesktop.Controls.Clear(); 

            _currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

            if (childForm.IsDisposed)
            {
                DisableButton();
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
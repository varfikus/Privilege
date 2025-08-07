using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using PrivilegeUI.Properties;
using PrivilegeUI.Sub;
using PrivilegeUI.Sub.Issue;
using System.Data;
using System.Windows.Forms;
using Application = PrivilegeAPI.Models.Application;
using HubConnection = Microsoft.AspNetCore.SignalR.Client.HubConnection;

namespace PrivilegeUI
{
    public partial class FormMain : Form
    {
        #region Fields

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

        #endregion


        #region Constructor

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

        #endregion


        #region Events

        private async void FormMain_Shown(object sender, EventArgs e)
        {
            //CheckUpdate(false);

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

            if (Settings.Default.ServerFTP == "" ||
                (Settings.Default.UserFTP == "" && Settings.Default.PassFTP == "") ||
                Settings.Default.PortFTP == 0)
            {
                ActivateButton(btn_settings);
                OpenChildForm(new FormSettings(this));
            }

            InitializeSignalR();
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

        private void pB_header_Click(object sender, EventArgs e)
        {
            ResetButton();
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
            OpenChildForm(new FormApply(_apiClient, app, this, true));
        }

        private void btn_denial_Click(object sender, EventArgs e)
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
            OpenChildForm(new FormApply(_apiClient, app, this, false));
        }

        private void btn_finaly_Click(object sender, EventArgs e)
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
            OpenChildForm(new FormAnsPos(_apiClient, app, this));
        }

        private void btn_cancel_Click(object sender, EventArgs e)
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
            OpenChildForm(new FormAnsNeg(_apiClient, app, this));
        }

        private void btn_archive_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                MessageBox.Show(@"В данный момент происходит загрузка данных", @"Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (_currentChildForm != null)
            {
                if (MessageBox.Show(@"Закрыть предыдущее окно?", @"Внимание",
                    MessageBoxButtons.YesNo) == DialogResult.Yes)
                    ResetButton();
                else
                    return;
            }

            timer_refresh.Stop();
            if (_isArchive)
            {
                lblTitleClildForm.Text = @"Главный экран";
                pB_CurrentChildForm.Image = Properties.Resources.home_white_96;
                _isArchive = false;
                ShowButtons(true);
            }
            else
            {
                lblTitleClildForm.Text = @"Архив";
                pB_CurrentChildForm.Image = Properties.Resources.winrar_white_40;
                _isArchive = true;
                ShowButtons(false);
            }
            LoadApplications();
            timer_refresh.Start();
        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            OpenChildForm(new FormSettings(this));
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //CheckUpdate(true);
        }

        private void dGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow dgvr = dGV.Rows[e.RowIndex];

            var enumCell = dgvr.Cells["StatusEnum"];

            var displayCell = dgvr.Cells["Status"];

            if (enumCell?.Value != null && displayCell != null &&
                Enum.TryParse(typeof(StatusEnum), enumCell.Value.ToString(), out object statusObj))
            {
                var status = (StatusEnum)statusObj;
                displayCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                switch (status)
                {
                    case StatusEnum.Add:
                        displayCell.Style.BackColor = Color.LightGray;
                        displayCell.Style.ForeColor = Color.Black;
                        break;

                    case StatusEnum.Delivered:
                        displayCell.Style.BackColor = Color.SteelBlue;
                        displayCell.Style.ForeColor = Color.White;
                        break;

                    case StatusEnum.Apply:
                        displayCell.Style.BackColor = Color.Gold;
                        displayCell.Style.ForeColor = Color.Black;
                        break;

                    case StatusEnum.Final:
                        displayCell.Style.BackColor = Color.ForestGreen;
                        displayCell.Style.ForeColor = Color.White;
                        break;

                    case StatusEnum.Denial:
                        displayCell.Style.BackColor = Color.IndianRed;
                        displayCell.Style.ForeColor = Color.White;
                        break;

                    case StatusEnum.DenialFinal:
                        displayCell.Style.BackColor = Color.DarkRed;
                        displayCell.Style.ForeColor = Color.White;
                        break;

                    default:
                        displayCell.Style.BackColor = Color.White;
                        displayCell.Style.ForeColor = Color.Black;
                        break;
                }
            }
        }

        private void dGV_CellMove(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow dgvr = dGV.Rows[e.RowIndex];
            var cell = dgvr.Cells["StatusEnum"];
            var statusCell = dgvr.Cells["status"];

            if (cell?.Value != null && Enum.TryParse(typeof(StatusEnum), cell.Value.ToString(), out object statusObj))
            {
                var status = (StatusEnum)statusObj;

                btn_info.Visible = true;

                switch (status)
                {
                    case StatusEnum.Delivered:
                        btn_apply.Visible = true;
                        btn_denial.Visible = true;
                        btn_finaly.Visible = false;
                        btn_cancel.Visible = false;
                        statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                        statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                        break;

                    case StatusEnum.Apply:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = true;
                        btn_cancel.Visible = true;
                        statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                        statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                        break;

                    case StatusEnum.Denial:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_cancel.Visible = true;
                        statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                        statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                        break;

                    case StatusEnum.Final:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_cancel.Visible = false;
                        statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                        statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                        break;

                    case StatusEnum.DenialFinal:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_cancel.Visible = false;
                        statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                        statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                        break;

                    default:
                        btn_info.Visible = false;
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_cancel.Visible = false;
                        statusCell.Style.SelectionBackColor = statusCell.Style.BackColor;
                        statusCell.Style.SelectionForeColor = statusCell.Style.ForeColor;
                        break;
                }
            }
        }

        private void ShowButtons(bool show)
        {
            btn_cancel.Visible = show;
            btn_finaly.Visible = show;
            btn_apply.Visible = show;
            btn_denial.Visible = show;
        }

        #endregion


        #region Methods


        #region SignalR

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

        #endregion


        #region DataGridView

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

        private void InitializeDataGridView()
        {
            dGV.Columns.Clear();
            dGV.AutoGenerateColumns = false;
            dGV.AllowUserToAddRows = false;
            dGV.ReadOnly = true;
            dGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dGV.CellFormatting += dGV_CellFormatting;
            dGV.CellEnter += dGV_CellMove;
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

        private void DGV_CellEnter(object? sender, DataGridViewCellEventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task LoadApplicationsAsync()
        {
            try
            {
                var response = await _apiClient.GetAsync<CollectionResult<Application>>("api/applications");

                if (response == null || !response.IsSuccess || response.Data == null)
                {
                    MessageBox.Show($"Ошибка загрузки данных: {response?.ErrorMessage ?? "Неизвестная ошибка"}");
                    return;
                }

                var applications = response.Data;

                var filteredApps = _isArchive
                    ? applications.Where(app =>
                        (app.Status == StatusEnum.Final || app.Status == StatusEnum.DenialFinal) &&
                        app.DateEdit <= DateTime.Now.AddDays(-30))
                    : applications;

                _applications = filteredApps.ToList();

                dGV.Rows.Clear();

                foreach (var app in _applications)
                {
                    dGV.Rows.Add(
                        app.Id,
                        app.Name,
                        EnumHelper.GetEnumDisplayName(app.Status),
                        app.Status,
                        app.DateAdd,
                        app.DateEdit
                    );
                }

                dGV.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}\nStackTrace: {ex.StackTrace}");
            }
        }

        #endregion


        #region ChildForm

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

        #endregion


        #endregion
    }
}
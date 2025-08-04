using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PrivilegeAPI.Models;
using PrivilegeUI.Classes;
using PrivilegeUI.Models;
using System.Data;
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
        private System.Timers.Timer _connectionCheckTimer;
        /// <summary>
        /// Выбранная кнопка
        /// </summary>
        private Button _currentBtn;
        /// <summary>
        /// Выделеноие кнопки слева
        /// </summary>
        private readonly Panel _leftBorderBtn;
        /// <summary>
        /// Выбранная форма
        /// </summary>
        private Form _currentChildForm;

        /// <summary>
        /// Основная таблица
        /// </summary>
        private DataTable _dt;
        /// <summary>
        /// Загруженная таблица
        /// </summary>
        private DataTable _dtOld;

        /// <summary>
        /// Количество ожидающих записей
        /// </summary>
        private int _countAdd = 0;
        /// <summary>
        /// Количество ожидающих выдачи записей
        /// </summary>
        private int _countApply = 0;

        /// <summary>
        /// Список учреждений
        /// </summary>
        private readonly Dictionary<int, string> _officeDictionary = new Dictionary<int, string>();

        /// <summary>
        /// Сообщение с ошибкой
        /// </summary>
        private string _error;
        /// <summary>
        /// Текущая таблица "архивная"?
        /// </summary>
        private bool _isArchive = false;

        public FormMain()
        {
            InitializeComponent();
            InitializeDataGridView();

            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);

            this.SavingOn();
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

        private void dGV_SelectionChanged(object sender, EventArgs e)
        {
            CellMove();
        }

        private void dGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            DataGridViewRow dgvr = dGV.Rows[e.RowIndex];
            var cell = dgvr.Cells["status"];

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

        /// <summary>
        /// Изменение состояния кнопок после выбора ячейки таблицы
        /// </summary>
        private void CellMove()
        {
            if (dGV.CurrentRow != null && dGV.Rows[dGV.CurrentRow.Index].Cells["status"].Value != null)
            {
                string statusStr = dGV.Rows[dGV.CurrentRow.Index].Cells["status"].Value.ToString();
                btn_info.Visible = true;

                if (!Enum.TryParse(typeof(StatusEnum), statusStr, out var statusObj))
                {
                    HideAllActionButtons();
                    return;
                }

                var status = (StatusEnum)statusObj;

                switch (status)
                {
                    case StatusEnum.Add:
                        btn_apply.Visible = true;
                        btn_denial.Visible = true;
                        btn_finaly.Visible = false;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;

                    case StatusEnum.Delivered:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = true;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;

                    case StatusEnum.Apply:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = true;
                        btn_finalyPaper.Visible = true;
                        btn_cancel.Visible = true;
                        break;

                    case StatusEnum.Final:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;

                    case StatusEnum.DenialApply:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;

                    default:
                        HideAllActionButtons();
                        break;
                }
            }
            else
            {
                HideAllActionButtons();
            }
        }

        private void HideAllActionButtons()
        {
            btn_info.Visible = false;
            btn_apply.Visible = false;
            btn_denial.Visible = false;
            btn_finaly.Visible = false;
            btn_finalyPaper.Visible = false;
            btn_cancel.Visible = false;
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
                        await LoadApplicationsAsync();
                    }));
                    return Task.CompletedTask;
                };

                await _hubConnection.StartAsync();
                await LoadApplicationsAsync();

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
            dGV.SelectionChanged += dGV_SelectionChanged;

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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/applications");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var applications = JsonSerializer.Deserialize<Application[]>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                dGV.Rows.Clear();

                foreach (var app in applications)
                {
                    dGV.Rows.Add(
                        app.Id,
                        app.Name,
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

            _currentBtn = (Button)senderBtn;
            _currentBtn.BackColor = Color.FromArgb(0, 36, 63);
            _leftBorderBtn.BackColor = Color.Gainsboro;
            _leftBorderBtn.Location = new Point(0, _currentBtn.Location.Y);
            _leftBorderBtn.Visible = true;
            _leftBorderBtn.BringToFront();

            pB_CurrentChildForm.BackgroundImage = _currentBtn.Image;
            lblTitleClildForm.Text = _currentBtn.Tag.ToString();
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
            if (_currentChildForm?.DialogResult == DialogResult.OK)
                LoadApplicationsAsync().ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        MessageBox.Show($"Ошибка при обновлении данных: {task.Exception?.Message}");
                    }
                });
            _currentChildForm?.Close();
            _currentChildForm = null;
            _leftBorderBtn.Visible = false;
            pB_CurrentChildForm.BackgroundImage = _isArchive ? Properties.Resources.winrar_white_40 : Properties.Resources.home_white_96;
            lblTitleClildForm.Text = _isArchive ? @"Архив" : @"Главный экран";
            timer_refresh.Start();
        }

        private void OpenChildForm(Form childForm)
        {
            _currentChildForm?.Close();

            _currentChildForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            if (childForm.IsDisposed)
                DisableButton();
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
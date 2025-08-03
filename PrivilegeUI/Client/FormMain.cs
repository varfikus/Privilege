using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text.Json;
using System.Windows.Forms;
using Privilege.UI.Classes;
using Privilege.UI.Classes.Json;
using Privilege.UI.Classes.Json.Sub;
using Privilege.UI.Window.Client.Sub;
using Privilege.UI.Window.Client.Sub.Issue;
//using Privilege.UI.Window.Update;

namespace Privilege.UI.Window.Client
{
    public partial class FormMain : Form
    {
        #region Fields

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

        #endregion


        #region Constructor

        public FormMain()
        {
            InitializeComponent();
            _leftBorderBtn = new Panel { Size = new Size(7, 60) };
            panelMenu.Controls.Add(_leftBorderBtn);
            this.SavingOn();
            timer_refresh.Interval = UserInfo.TableRefresh == 0 ? 60000 : UserInfo.TableRefresh;
        }

        #endregion


        #region Events

        private void FormMain_Load(object sender, EventArgs e)
        {
            string[] ver = Application.ProductVersion.Split('.');
            Text += @" v" + ver[0] + @"." + ver[1];
            if (ver[2] != 0.ToString())
            {
                Text += @"." + ver[2];
                if (ver[3] != 0.ToString())
                    Text += @"." + ver[3];
            }
            else
            {
                if (ver[3] != 0.ToString())
                    Text += @"." + ver[2] + @"." + ver[3];
            }

            Text += @"  [" + UserInfo.Login + @"]";
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
            Application.Exit();
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
            ResetButton();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void btn_info_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                int.TryParse(dGV.CurrentRow.Cells["id"].Value.ToString(), out int num);
                ActivateButton(sender);
                OpenChildForm(new FormInfo(this, num));
            }
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                ActivateButton(sender);
                OpenChildForm(new FormApply(this, dGV.CurrentRow));
            }
        }

        private void btn_denial_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                ActivateButton(sender);
                OpenChildForm(new FormApply(this, dGV.CurrentRow, false));
            }
        }

        private void btn_finaly_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                ActivateButton(sender);
                OpenChildForm(new FormAnsPos(this, dGV.CurrentRow));
            }
        }

        private void btn_finalyPaper_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                ActivateButton(sender);
                OpenChildForm(new FormAnsPaper(this, dGV.CurrentRow));
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            if (dGV.CurrentRow != null)
            {
                ActivateButton(sender);
                OpenChildForm(new FormAnsNeg(this, dGV.CurrentRow));
            }
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
                pB_CurrentChildForm.BackgroundImage = Properties.Resources.home_white_96;
                _isArchive = false;
            }
            else
            {
                lblTitleClildForm.Text = @"Архив";
                pB_CurrentChildForm.BackgroundImage = Properties.Resources.winrar_white_40;
                _isArchive = true;
            }
            LoadInfo();
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

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            LoadTable();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            Point cellAdress = dGV.CurrentCellAddress;

            if (!string.IsNullOrEmpty(_error))
                MessageBox.Show(_error, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            if (!AreTablesTheSame(_dt, _dtOld))
            {
                _dtOld?.Rows.Clear();
                _dtOld = _dt?.Copy();
                dGV.DataSource = _dtOld;

                lbl_add.Text = _countAdd.ToString();
                lbl_apply.Text = _countApply.ToString();

                if (_dtOld?.Rows.Count == 0)
                    CellMove();
            }

            if (cellAdress.X > -1 && cellAdress.Y > -1 && cellAdress.Y < dGV.RowCount)
            {
                dGV.Rows[cellAdress.Y].Cells[cellAdress.X].Selected = true;
            }

            lbl_time.Text = DateTime.Now.ToString("G");
        }

        private void dGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            DataGridViewRow dgvr = dGV.Rows[e.RowIndex];
            if (dgvr.Cells["status"].Value != null)
            {
                dgvr.Cells["status"].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

                switch (dgvr.Cells["status"].Value.ToString())
                {
                    case "Не принят":
                    case "Ожидание подтверждения":
                    case "Ожидание приёма в работу":
                        dgvr.Cells["status"].Style.BackColor = Color.Red;
                        dgvr.Cells["status"].Style.ForeColor = Color.Gold;
                        break;
                    case "Принят на рассмотрение":
                    case "Ответ с Портала":
                        dgvr.Cells["status"].Style.BackColor = Color.Yellow;
                        dgvr.Cells["status"].Style.ForeColor = Color.Black;
                        break;
                    case "Ожидание подписи руководителя":
                    case "Ожидание документов":
                        dgvr.Cells["status"].Style.BackColor = Color.CornflowerBlue;
                        dgvr.Cells["status"].Style.ForeColor = Color.Black;
                        break;
                    case "Ожидание выдачи":
                    case "Руководитель не подписал документ":
                        dgvr.Cells["status"].Style.BackColor = Color.DeepSkyBlue;
                        dgvr.Cells["status"].Style.ForeColor = Color.Black;
                        break;
                    default:
                        dgvr.Cells["status"].Style.BackColor = Color.Green;
                        dgvr.Cells["status"].Style.ForeColor = Color.WhiteSmoke;
                        break;
                }
            }
        }

        private void dGV_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            CellMove();
        }

        private void timer_refresh_Tick(object sender, EventArgs e)
        {
            LoadInfo();
        }

        #endregion


        #region Methods

        #region Открытие окон и визуал

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
                LoadInfo();
            _currentChildForm?.Close();
            _currentChildForm = null;
            _leftBorderBtn.Visible = false;
            pB_CurrentChildForm.BackgroundImage = _isArchive ? Properties.Resources.winrar_white_40 : Properties.Resources.home_white_96;
            lblTitleClildForm.Text = _isArchive ? @"Архив" : @"Главный экран";
            timer_refresh.Interval = UserInfo.TableRefresh;
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

        #endregion


        #region Загрузка таблицы

        /// <summary>
        /// Загрузить информацию
        /// </summary>
        private void LoadInfo()
        {
            _error = "";
            //поля поиска

            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }

        /// <summary>
        /// Заполнить таблицу
        /// </summary>
        private async Task LoadApplicationsAsync()
        {
            try
            {
                // Construct query parameters based on _isArchive, UserInfo.RuleOffice, and UserInfo.RuleService
                string office = UserInfo.RuleOffice == "0" ? "" : $"office={UserInfo.RuleOffice}";
                string service = UserInfo.RuleService == "0" ? "" : $"service_id={UserInfo.RuleService}";
                string archive = _isArchive ? "isArchive=true" : "isArchive=false";
                string queryParams = string.Join("&", new[] { office, service, archive }.Where(s => !string.IsNullOrEmpty(s)));
                string url = $"{_apiBaseUrl}/api/applications" + (string.IsNullOrEmpty(queryParams) ? "" : $"?{queryParams}");

                // Make API request
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                // Deserialize JSON into Application array
                var applications = JsonSerializer.Deserialize<Application[]>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Initialize counts and DataTable
                _countAdd = 0;
                _countApply = 0;
                _dt = InitDataTable();
                LoadDictionary(); // Load _officeDictionary as in the old method
                _dataGridView.Rows.Clear();

                int npp = 0;

                foreach (var app in applications)
                {
                    DataRow dr = _dt.NewRow();
                    DateTime.TryParse(app.DateAdd.ToString(), out DateTime dateAdd);

                    dr["npp"] = ++npp;
                    dr["id"] = app.Id.ToString();
                    dr["num_id"] = dateAdd.Year + "." + app.Id;
                    dr["fio"] = app.Fio?.ToString() ?? "";
                    dr["birthday"] = app.Birthday?.ToString() ?? "";
                    dr["address_reg"] = app.AddressReg?.ToString() ?? "";
                    dr["date_add"] = dateAdd.ToString("d");

                    // Map status as in the old method
                    switch (app.Status?.ToString())
                    {
                        case "0":
                            dr["status"] = "Не принят";
                            break;
                        case "1":
                            dr["status"] = "Ожидание приёма в работу";
                            break;
                        case "2":
                            dr["status"] = "Отказано в приёме";
                            break;
                        case "3":
                            dr["status"] = "Принят на рассмотрение";
                            break;
                        case "4":
                            dr["status"] = "Отказано в выдаче документа";
                            break;
                        case "5":
                            dr["status"] = "Ожидание подписи руководителя";
                            break;
                        case "6":
                            dr["status"] = "Выдан документ";
                            break;
                        case "7":
                            dr["status"] = "Ожидание выдачи";
                            break;
                        case "8":
                            dr["status"] = "Ожидание документов";
                            break;
                        case "9":
                            dr["status"] = "Ответ с Портала";
                            break;
                        default:
                            dr["status"] = "";
                            break;
                    }

                    // Calculate days left as in the old method
                    if (DateTime.TryParse(app.DateIspoln?.ToString(), out DateTime dateIspoln))
                    {
                        TimeSpan subDate = dateIspoln.Subtract(DateTime.Now);
                        if (subDate.Days > 0 && (app.Status == "0" || app.Status == "3" || app.Status == "1"))
                            dr["days_left"] = subDate.Days;
                        else
                            dr["days_left"] = "0";
                        dr["date_ispoln"] = app.DateIspoln.ToString();
                    }
                    else
                    {
                        dr["days_left"] = "0";
                        dr["date_ispoln"] = "";
                    }

                    dr["id_gosuslug"] = app.IdUslugi?.ToString() ?? "";
                    dr["name_usl"] = app.NameUsl?.ToString() ?? "";
                    dr["region"] = app.Region?.ToString() ?? "";

                    // Map office using _officeDictionary
                    if (!string.IsNullOrEmpty(app.Office?.ToString()) && app.Office.ToString() != "0" && int.TryParse(app.Office.ToString(), out int officeId))
                        dr["office"] = _officeDictionary.Count != 0 ? _officeDictionary[officeId] : app.Office.ToString();
                    else
                        dr["office"] = "";

                    dr["electronic"] = app.Electronic?.ToString() == "True";
                    dr["service_id"] = app.ServiceId?.ToString() ?? "";

                    _dt.Rows.Add(dr);

                    // Update counts for non-archive mode
                    if (!_isArchive)
                    {
                        if (app.Status == "0" || app.Status == "1")
                            _countAdd++;
                        else if (app.Status == "3" || app.Status == "9")
                            _countApply++;
                    }
                }

                // Bind DataTable to DataGridView
                _dataGridView.DataSource = _dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading applications: {ex.Message}\nStackTrace: {ex.StackTrace}");
                _error = $"При получении или заполнении данных возникла ошибка:\n{ex.Message}";
            }
        }

        /// <summary>
        /// Инициализация таблицы, чтобы правильно работала привязка
        /// </summary>
        /// <returns></returns>
        private DataTable InitDataTable()
        {
            DataTable dtView = new DataTable();

            foreach (DataGridViewColumn column in dGV.Columns)
            {
                dtView.Columns.Add(column.Name);
                dGV.Columns[column.Name].DataPropertyName = column.Name;
            }

            dtView.Columns["npp"].DataType = Type.GetType("System.Int32");
            dtView.Columns["date_add"].DataType = Type.GetType("System.DateTime");

            return dtView;
        }

        /// <summary>
        /// Заполнение словарей.
        /// </summary>
        private void LoadDictionary()
        {
            _officeDictionary?.Clear();

            string query = "SELECT * " +
                           "FROM office;";

            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    int.TryParse(row["id"].ToString(), out int num);
                    if (_officeDictionary != null && !_officeDictionary.ContainsKey(num))
                        _officeDictionary.Add(num, row["name"].ToString());
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                //throw;
            }
        }


        #region Сравнение таблиц

        /// <summary>
        /// Сравнение 2 таблиц
        /// </summary>
        /// <param name="tbl1">Таблица 1</param>
        /// <param name="tbl2">Таблица 2</param>
        /// <returns>true - таблицы равны</returns>
        public static bool AreTablesTheSame(DataTable tbl1, DataTable tbl2)
        {
            if (tbl1 == null || tbl2 == null)
                return false;
            if (tbl1.Rows.Count != tbl2.Rows.Count || tbl1.Columns.Count != tbl2.Columns.Count)
                return false;

            for (int i = 0; i < tbl1.Rows.Count; i++)
            {
                for (int c = 0; c < tbl1.Columns.Count; c++)
                {
                    if (!Equals(tbl1.Rows[i][c], tbl2.Rows[i][c]))
                        return false;
                }
            }
            return true;
        }

        #endregion

        #endregion


        /// <summary>
        /// Изменение состояния кнопок после выбора ячейки таблицы
        /// </summary>
        private void CellMove()
        {
            if (dGV.CurrentRow != null && dGV.Rows[dGV.CurrentRow.Index].Cells["status"].Value != null)
            {
                string status = dGV.Rows[dGV.CurrentRow.Index].Cells["status"].Value.ToString().ToLower();
                btn_info.Visible = true;

                //todo: проверка на сертификат

                switch (status)
                {
                    case "не принят":
                    case "ожидание приёма в работу":
                        btn_apply.Visible = true;
                        btn_denial.Visible = true;
                        btn_finaly.Visible = false;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;
                    case "принят на рассмотрение":
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = true;
                        btn_finalyPaper.Visible = true;
                        btn_cancel.Visible = true;
                        break;
                    case "ожидание выдачи":
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = true;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;
                    case "ожидание документов":
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = true;
                        break;
                    case "ответ с портала":
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = true;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;
                    default:
                        btn_apply.Visible = false;
                        btn_denial.Visible = false;
                        btn_finaly.Visible = false;
                        btn_finalyPaper.Visible = false;
                        btn_cancel.Visible = false;
                        break;
                }
            }
            else
            {
                btn_info.Visible = false;
                btn_apply.Visible = false;
                btn_denial.Visible = false;
                btn_finaly.Visible = false;
                btn_finalyPaper.Visible = false;
                btn_cancel.Visible = false;
            }
        }


        //#region Update

        ///// <summary>
        ///// Выполнение GET запроса
        ///// </summary>
        ///// <param name="url">Адрес ресурса</param>
        ///// <returns>Ответ на запрос</returns>
        //public static string GetNewVersion(string url)
        //{
        //    try
        //    {
        //        WebRequest req = WebRequest.Create(url);
        //        WebResponse resp = req.GetResponse();
        //        System.IO.Stream stream = resp.GetResponseStream();
        //        System.IO.StreamReader sr = new System.IO.StreamReader(stream);
        //        string outText = sr.ReadToEnd();
        //        sr.Close();
        //        return outText;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        ///// <summary>
        ///// Проверить наличие новой версии
        ///// </summary>
        ///// <param name="search"></param>
        //private void CheckUpdate(bool search)
        //{
        //    try
        //    {
        //        ServicePointManager.SecurityProtocol =
        //            SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        //        string verJson = GetNewVersion("https://gupcit.com/data/update/mineconom/client/" + "version.json");
        //        if (verJson == "")
        //        {
        //            if (search)
        //                MessageBox.Show(@"Пустой ответ от сервера обновлений. Возможно сервер обновлений недоступен.",
        //                    @"Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        JsonUpdate json = JsonWorker.DeserializVersion(verJson);
        //        if (json == null)
        //        {
        //            MessageBox.Show(@"Не удалось преобразовать скачанный файл с версией", @"Внимание",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        Version updaiteVersion = new Version(json.Version);
        //        Version curentVersion = new Version(Application.ProductVersion);

        //        if (updaiteVersion > curentVersion)
        //        {
        //            ChangeNewImage();

        //            if (!json.Critical && search == true)
        //                new FormUpdateNew(json.Version) { Owner = this }.ShowDialog();
        //            else if (json.Critical)
        //                new FormUpdateCritical(json.Version) { Owner = this }.ShowDialog();
        //        }
        //        else
        //        {
        //            if (search)
        //                MessageBox.Show(@"Новая версия не найдена");
        //        }
        //    }
        //    catch
        //    {
        //        if (search)
        //            MessageBox.Show(@"Сервер обновлений временно недоступен");
        //    }

        //}

        ///// <summary>
        ///// Поменять картинку при наличии нового обновления
        ///// </summary>
        //private void ChangeNewImage()
        //{
        //    //toolTip1.SetToolTip(this.btnUpdate_, "Доступна новая версия программы!");
        //    btn_update.BackgroundImage = Properties.Resources.synchronize_red_96;
        //}

        ////#endregion

        #endregion
    }
}

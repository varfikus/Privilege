using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Privilege.UI.Classes;
using Privilege.UI.Classes.Json;
using Privilege.UI.Classes.Signature;
using MySql.Data.MySqlClient;

namespace Privilege.UI.Window.Client.Sub
{
    public partial class FormSettings : Form
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
        /// Родительская форма
        /// </summary>
        private readonly FormMain _parentForm;

        /// <summary>
        /// Настройки
        /// </summary>
        private JsonSettings _settings;

        #endregion


        #region Constuctor

        public FormSettings(FormMain parentForm)
        {
            InitializeComponent();
            _leftBorderBtn = new Panel { Size = new Size(7, 60) };
            panelMenu.Controls.Add(_leftBorderBtn);
            _parentForm = parentForm;
        }

        #endregion


        #region Events

        private void FormSettings_Load(object sender, EventArgs e)
        {
            LoadInfo();
            btn_user_Click(btn_user, e);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!Valid())
                return;

            if (!Save())
                return;

            Connection.CreateConnection();
            UserInfo.TableRefresh = _settings.TableRefresh * 60000;
            UserInfo.Sert = _settings.User.Sert;
            UserInfo.Name = _settings.User.Fio;
            UserInfo.Tel = _settings.User.Tel;
            DialogResult = DialogResult.OK;
            _parentForm?.ResetButton();
            Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            _parentForm?.ResetButton();
            Close();
        }

        private void btn_user_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl.SelectedTab = tabPageUser;
        }

        private void btn_db_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl.SelectedTab = tabPageDb;
        }

        private void btn_ftp_Click(object sender, EventArgs e)
        {
            ActivateButton(sender);
            tabControl.SelectedTab = tabPageFtp;
        }


        #endregion


        #region Methods

        private void ActivateButton(object senderBtn)
        {
            DisableButton();

            _currentBtn = (Button)senderBtn;
            _currentBtn.BackColor = Color.FromArgb(0, 36, 63);
            _leftBorderBtn.BackColor = Color.Gainsboro;
            _leftBorderBtn.Location = new Point(0, _currentBtn.Location.Y);
            _leftBorderBtn.Visible = true;
            _leftBorderBtn.BringToFront();
        }

        private void DisableButton()
        {
            if (_currentBtn != null)
            {
                _currentBtn.BackColor = Color.FromArgb(38, 75, 119);
                _currentBtn.ForeColor = Color.Gainsboro;
            }
        }

        /// <summary>
        /// Заполнить поля
        /// </summary>
        private void LoadInfo()
        {
            LoadSert();

            SettingsFile sf = new SettingsFile();
            _settings = sf.LoadSettings();

            tB_ip.DataBindings.Add(new Binding("Text", _settings, "Conn.Ip", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_user.DataBindings.Add(new Binding("Text", _settings, "Conn.User", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_pass.DataBindings.Add(new Binding("Text", _settings, "Conn.Password", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_name.DataBindings.Add(new Binding("Text", _settings, "Conn.Name", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_port.DataBindings.Add(new Binding("Text", _settings, "Conn.Port", true,
                DataSourceUpdateMode.OnPropertyChanged));

            tB_ipFtp.DataBindings.Add(new Binding("Text", _settings, "ConnFtp.Ip", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_userFtp.DataBindings.Add(new Binding("Text", _settings, "ConnFtp.User", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_passFtp.DataBindings.Add(new Binding("Text", _settings, "ConnFtp.Password", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_portFtp.DataBindings.Add(new Binding("Text", _settings, "ConnFtp.Port", true,
                DataSourceUpdateMode.OnPropertyChanged));

            tB_fio.DataBindings.Add(new Binding("Text", _settings, "User.Fio", true,
                DataSourceUpdateMode.OnPropertyChanged));
            tB_tel.DataBindings.Add(new Binding("Text", _settings, "User.Tel", true,
                DataSourceUpdateMode.OnPropertyChanged));
            cB_sert.DataBindings.Add(new Binding("Text", _settings, "User.Sert", true,
                DataSourceUpdateMode.OnPropertyChanged));
            num_tableRefresh.DataBindings.Add(new Binding("Value", _settings, "TableRefresh", true,
                DataSourceUpdateMode.OnPropertyChanged));
        }

        /// <summary>
        /// Проверить заполнения полей
        /// </summary>
        /// <returns></returns>
        private bool Valid()
        {
            bool f = true;

            if (_settings.ConnFtp.Ip == "" || _settings.ConnFtp.User == "")
            {
                f = MessageBox.Show(@"Не заполнены параметры FTP!" + Environment.NewLine + @"Продолжить?",
                    @"Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
            }
            else
            {
                if (!MyFtp.CheckConnection(_settings.ConnFtp))
                    f = MessageBox.Show(@"Не удалось подключиться к FTP!" + Environment.NewLine + @"Продолжить?",
                        @"Внимание!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes;
            }

            return f;
        }

        /// <summary>
        /// Сохранить параметры
        /// </summary>
        private bool Save()
        {
            if (!UpdateUserInfoDb())
                return false;

            UserInfo.Name = tB_fio.Text;
            UserInfo.Tel = tB_tel.Text;

            SettingsFile sf = new SettingsFile();
            return sf.SaveSettings(_settings);
        }

        /// <summary>
        /// Обновить в базе данных информацию оператора
        /// </summary>
        /// <returns></returns>
        private bool UpdateUserInfoDb()
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandText = "UPDATE autoris SET " +
                                  "fio = @fio, " +
                                  "tel = @tel " +
                                  "WHERE id = @id";

                cmd.Parameters.AddWithValue("@fio", tB_fio.Text);
                cmd.Parameters.AddWithValue("@tel", tB_tel.Text);
                cmd.Parameters.AddWithValue("@id", UserInfo.Id);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Возникла ошибка обновления записи:" + Environment.NewLine + ex, @"Ошибка обновления",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Заполнить список сертификатов
        /// </summary>
        private void LoadSert()
        {
            List<MyCert> certificates = SignDoc.GelAllCertificates();
            cB_sert.DataSource = certificates;
        }

        #endregion
    }
}

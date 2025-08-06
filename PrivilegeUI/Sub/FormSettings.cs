using PrivilegeUI.Models;

namespace PrivilegeUI.Sub
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
            Properties.Settings.Default.Save();
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
            tB_ipFtp.DataBindings.Add("Text", Properties.Settings.Default, "ServerFTP", true, DataSourceUpdateMode.OnPropertyChanged);
            tB_userFtp.DataBindings.Add("Text", Properties.Settings.Default, "UserFTP", true, DataSourceUpdateMode.OnPropertyChanged);
            tB_passFtp.DataBindings.Add("Text", Properties.Settings.Default, "PassFTP", true, DataSourceUpdateMode.OnPropertyChanged);
            tB_portFtp.DataBindings.Add("Text", Properties.Settings.Default, "PortFTP", true, DataSourceUpdateMode.OnPropertyChanged);

            tB_fio.DataBindings.Add("Text", UserInfo.CurrentUser, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
            tB_tel.DataBindings.Add("Text", Properties.Settings.Default, "UserTel", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        #endregion
    }
}

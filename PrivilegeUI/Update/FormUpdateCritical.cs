using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PrivilegeUI.Update
{
    public partial class FormUpdateCritical : Form
    {
        #region Fields

        private bool _flag = true;
        private readonly string _newVersion;

        #endregion


        #region Constructor

        public FormUpdateCritical(string newVer)
        {
            InitializeComponent();
            _newVersion = newVer;
            lbl_verCurrent.Text = Application.ProductVersion;
            lbl_verNew.Text = _newVersion;
        }

        #endregion


        #region Events

        private void FormUpdateCritical_Shown(object sender, EventArgs e)
        {
            if (!File.Exists("updater.exe"))
                if (MessageBox.Show(@"Не найдена программа обновлений!" + Environment.NewLine + @"Загрузить её?",
                    @"Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    DownloadUpdater();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateVer();
        }

        private void FormUpdateCritical_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_flag)
            {
                Environment.Exit(0);
            }
            else
            {
                _flag = true;
            }
        }

        #endregion


        #region Methods

        private void UpdateVer()
        {
            if (!File.Exists("updater.exe"))
            {
                MessageBox.Show(@"Не найдена программа обновлений!", @"Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _flag = false;
                return;
            }

            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "updater.exe";
                string arg = "\"" + AppDomain.CurrentDomain.FriendlyName + "\" " + _newVersion;

                Process.Start(fileName, arg);

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Пои запуске программы обновления возникла ошибка:" + Environment.NewLine + ex.Message, @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                _flag = false;
                Close();
            }
        }

        /// <summary>
        /// Загрузка новой версии программы обновления
        /// </summary>
        /// <returns></returns>
        private bool DownloadUpdater()
        {
            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFile(
                    new Uri("https://gupcit.com/data/update/mineconom/client/" + "updater.exe"),
                    AppDomain.CurrentDomain.BaseDirectory + "updater.exe");
                return true;
            }
            catch (WebException ex)
            {
                MessageBox.Show(@"При скачивании возникла ошибка:" + Environment.NewLine + ex.Message, @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }

        #endregion
    }
}

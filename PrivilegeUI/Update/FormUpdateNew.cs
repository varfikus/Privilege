using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace PrivilegeUI.Update
{
    public partial class FormUpdateNew : Form
    {
        #region Fields

        /// <summary>
        /// Новая версия программы
        /// </summary>
        private readonly string _newVer;

        #endregion


        #region Constructor

        public FormUpdateNew(string newVer)
        {
            InitializeComponent();
            _newVer = newVer;
        }

        #endregion


        #region Events

        private void FormUpdateNew_Load(object sender, EventArgs e)
        {
            lbl_verCurrent.Text = Application.ProductVersion;
            lbl_verNew.Text = _newVer;

            if (!File.Exists("updater.exe"))
                if (MessageBox.Show(@"Не найдена программа обновлений!" + Environment.NewLine + @"Загрузить её?",
                    @"Внимание!", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    DownloadUpdater();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            UpdateVer();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion


        #region Methods

        private void UpdateVer()
        {
            if (!File.Exists("updater.exe"))
            {
                MessageBox.Show(@"Не найдена программа обновлений!", @"Внимание!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "updater.exe";
                string arg = "\"" + AppDomain.CurrentDomain.FriendlyName + "\" " + _newVer;

                Process.Start(fileName, arg);

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Пои запуске программы обновления возникла ошибка:" + Environment.NewLine + ex.Message, @"Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    new Uri("https://gupcit.com/data/update/mineconom/client/" + "update.exe"),
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

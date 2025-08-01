using System;
using System.Collections.Generic;
using System.Windows.Forms;
using FluentFTP;
using Microsoft.Win32;
using ServiceMinsoc.Properties;

namespace ServiceMinsoc
{
    public partial class FormSettings : Form
    {
        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            tB_ip.Text = Settings.Default.Db_Ip;
            tB_userName.Text = Settings.Default.Db_UserName;
            tB_password.Text = Settings.Default.Db_Password;
            tB_name.Text = Settings.Default.Db_Name;
            tB_port.Text = Settings.Default.Db_Port;
            num_interval.Value = Settings.Default.Interval;

            tB_url.Text = Settings.Default.Url;
            tB_url_port.Text = Settings.Default.Port;

            cB_post.Checked = !Settings.Default.PostEnable;
            cB_autorun.Checked = Settings.Default.isAutorun;

            num_apply.Value = Settings.Default.NumApply;

            tB_ftpIp.Text = Settings.Default.Ftp_Url;
            tB_ftpUser.Text = Settings.Default.Ftp_User;
            tB_ftpPass.Text = Settings.Default.Ftp_Pass;
            tB_ftpPort.Text = Settings.Default.Ftp_Port.ToString();

            CryptSettings();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            if (Valid())
            {
                if (Save())
                    Close();
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Сохранить параметры.
        /// </summary>
        private bool Save()
        {
            Settings.Default.Db_Ip = tB_ip.Text;
            Settings.Default.Db_UserName = tB_userName.Text;
            Settings.Default.Db_Password = tB_password.Text;
            Settings.Default.Db_Name = tB_name.Text;
            Settings.Default.Db_Port = tB_port.Text;
            Settings.Default.Interval = (int) num_interval.Value;

            Settings.Default.Url = tB_url.Text;
            Settings.Default.Port = tB_url_port.Text;

            Settings.Default.PostEnable = !cB_post.Checked;
            Settings.Default.NumApply = (int) num_apply.Value;

            Settings.Default.CryptoSert = cB_crypto.Text;

            Settings.Default.isAutorun = cB_autorun.Checked;
            SetAutorunValue(cB_autorun.Checked);

            Settings.Default.Ftp_Url = tB_ftpIp.Text;
            Settings.Default.Ftp_User = tB_ftpUser.Text;
            Settings.Default.Ftp_Pass = tB_ftpPass.Text;
            int ftpPort;
            Settings.Default.Ftp_Port = int.TryParse(tB_ftpPort.Text, out ftpPort) ? ftpPort : 21;

            if (!CheckFtpConnection())
            {
                if (MessageBox.Show(@"Ошибка при подключении к FTP-серверу" + Environment.NewLine + @"Продолжить?",
                        @"Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
                    return false;
            }

            Settings.Default.Save();
            return true;
        }


        private bool Valid()
        {

            return true;
        }

        /// <summary>
        /// Подстановка в comboBox сертификатов и выбор ранее сохранённого
        /// </summary>
        private void CryptSettings()
        {
            List<MyCert> certificates = Cripto.GelAllCertificates();
            foreach (var sert in certificates)
            {
                cB_crypto.Items.Add(sert);
            }

            cB_crypto.SelectedIndex = cB_crypto.Items.IndexOf(Settings.Default.CryptoSert);

            for (int i = 0; i < cB_crypto.Items.Count; i++)
            {
                if (cB_crypto.Items[i].ToString() == Settings.Default.CryptoSert)
                {
                    cB_crypto.SelectedIndex = i;
                    break;
                }
            }
        }

        /// <summary>
        /// Добавление или удаление из автозагрузки
        /// </summary>
        /// <param name="autorun">true-добавить, false-удалить</param>
        /// <returns></returns>
        private bool SetAutorunValue(bool autorun)
        {
            string exePath = Application.ExecutablePath;
            RegistryKey reg;
            reg = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");
            try
            {
                if (autorun)
                {
                    reg?.SetValue(MyStatic.ProgramName, exePath);
                    Settings.Default.isAutorun = true;
                    Settings.Default.Save();
                }
                else
                {
                    reg?.DeleteValue(MyStatic.ProgramName);
                    Settings.Default.isAutorun = false;
                    Settings.Default.Save();
                }

                reg?.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Проверить соединение к FTP-серверу
        /// </summary>
        /// <returns>true - подключение установлено, false - ошибка при соединении</returns>
        private bool CheckFtpConnection()
        {
            FtpClient client = new FtpClient(tB_url.Text, tB_ftpUser.Text, tB_ftpPass.Text);
            //client.Credentials = new NetworkCredential(tB_ftpUser.Text, tB_ftpPass.Text);
            if (tB_ftpPort.Text != "")
            {
                int port;
                if (int.TryParse(tB_ftpPort.Text, out port))
                    client.Port = port;
            }
            try
            {
                client.Connect();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }
    }
}

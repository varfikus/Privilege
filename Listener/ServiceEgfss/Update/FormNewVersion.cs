using System;
using System.Windows.Forms;

namespace ServiceMinsoc.Update
{
    public partial class FormNewVersion : Form
    {
        public FormNewVersion(string newVersion)
        {
            InitializeComponent();
            _newVersion = newVersion;
        }

        private readonly string _newVersion;

        private void UpdateVer()
        {
            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "updater.exe";
                string arg = "\"" + AppDomain.CurrentDomain.FriendlyName + "\" " + _newVersion;
                if (System.IO.File.Exists(fileName))
                {
                    System.Diagnostics.Process.Start(fileName, arg);
                    Environment.Exit(0);
                }
                else
                    MessageBox.Show(@"Программа обновлени не найдена");
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Сервер обновлений временно недоступен\n" + ex.Message);
                Close();
            }
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            UpdateVer();
        }

        private void button_no_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

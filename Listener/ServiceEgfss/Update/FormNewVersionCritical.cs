using System;
using System.Windows.Forms;

namespace ServiceMinsoc.Update
{
    public partial class FormNewVersionCritical : Form
    {
        public FormNewVersionCritical(string New)
        {
            InitializeComponent();
            _newVersion = New;
        }

        private bool _flag = true;
        private readonly string _newVersion;

        private void button_yes_KeyDown(object sender, KeyEventArgs e)
        {
            Update(_newVersion);
        }

        private void button_yes_Click(object sender, EventArgs e)
        {
            Update(_newVersion);
        }

        private void FormNewVersionCritical_FormClosing(object sender, FormClosingEventArgs e)
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

        private void Update(string newVersion)
        {
            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + "updater.exe";
                string arg = "\"" + AppDomain.CurrentDomain.FriendlyName + "\" " + newVersion;

                System.Diagnostics.Process.Start(fileName, arg);

                Environment.Exit(0);
            }
            catch
            {
                MessageBox.Show(@"Сервер обновлений временно недоступен");
                _flag = false;
                Close();
            }
        }
    }
}

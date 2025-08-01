using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Updater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string _nameProgram;
        private string _newVersion;
        private string _url = "";
        private string _error = "";
        private string _fileName;

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 2)
            {
                _nameProgram = args[1];
                _newVersion = args[2];
                if (args.Length > 3)
                    _url = args[3];
            }
            else
            {
                _nameProgram = "Minsoc Service.exe";
                _newVersion = GET("https://gupcit.com/data/update/minsoc/service/version.txt");
            }
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            MyUpdate();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_error == "")
            {
                try
                {
                    if (File.Exists(_nameProgram))
                        File.Delete(_nameProgram);
                    File.Move(_fileName, _nameProgram);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("При обновлении возникла следующая ошибка:\n" + ex.Message, @"Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                Process.Start(_nameProgram);
            }
            else
            {
                MessageBox.Show(_error, @"Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            Environment.Exit(0);
        }

        private void MyUpdate()
        {
            try
            {
                _fileName = "v." + _newVersion + "(" + DateTime.Now.Day + "." + DateTime.Now.Month + "." + DateTime.Now.Year + ")" + @"AP.exe";
                string directory = Path.GetTempPath();


                string[] fullfilesPath = Directory.GetFiles(@directory, "*AP.exe");

                foreach (var file in fullfilesPath)
                {
                    File.Delete(file);
                }

                WebClient webClient = new WebClient();
                webClient.DownloadFile(new Uri(_url == "" ? "https://gupcit.com/data/update/minsoc/service/update.exe" : _url), _fileName);
            }
            catch (Exception ex)
            {
                _error = "Сервер обновлений временно недоступен" + ex.Message;
            }
        }

        /// <summary>
        /// Выполнение GET запроса
        /// </summary>
        /// <param name="url">Адрес ресурса</param>
        /// <returns>Ответ на запрос</returns>
        public static string GET(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string Out = sr.ReadToEnd();
                sr.Close();
                return Out;
            }
            catch
            {
                return "Error zapros";
            }
        }
    }
}

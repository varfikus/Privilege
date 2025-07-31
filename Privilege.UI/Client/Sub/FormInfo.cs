using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Privilege.UI.Classes;
using MySql.Data.MySqlClient;

namespace Privilege.UI.Window.Client.Sub
{
    public partial class FormInfo : Form
    {
        #region Fields

        /// <summary>
        /// Родительская форма
        /// </summary>
        private readonly FormMain _parentForm;

        /// <summary>
        /// ID записи
        /// </summary>
        private readonly int _id;
        /// <summary>
        /// ID услуги
        /// </summary>
        private string _gosuslugId;
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private string _path;

        private int _fileId;
        private int _fileArrived;
        private int _fileApply;
        private int _fileFinaly;
        private int _fileDirector;
        private int _fileReturn;
        private int _fileIncoming;

        #endregion


        #region Constructor

        public FormInfo(FormMain parentForm, int id)
        {
            InitializeComponent();
            _parentForm = parentForm;
            _id = id;
        }

        #endregion


        #region Events

        private void FormInfo_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _parentForm?.ResetButton();
            Close();
        }

        private void btn_fileAddOpen_Click(object sender, EventArgs e)
        {
            OpenFile(_fileId);
        }

        private void btn_fileAddDownload_Click(object sender, EventArgs e)
        {
            DownloadFile(_fileId);
        }

        private void btn_fileArrivedOpen_Click(object sender, EventArgs e)
        {
            OpenFile(_fileArrived);
        }

        private void btn_fileArrivedDownload_Click(object sender, EventArgs e)
        {
            DownloadFile(_fileArrived);
        }

        private void btn_fileApplyOpen_Click(object sender, EventArgs e)
        {
            OpenFile(_fileApply);
        }

        private void btn_fileApplyDownload_Click(object sender, EventArgs e)
        {
            DownloadFile(_fileApply);
        }

        private void btn_fileFinalyOpen_Click(object sender, EventArgs e)
        {
            OpenFile(_fileFinaly);
        }

        private void btn_fileFinalyDownload_Click(object sender, EventArgs e)
        {
            DownloadFile(_fileFinaly);
        }
        
        #endregion


        #region Methods

        /// <summary>
        /// Заполнить поля данными
        /// </summary>
        private void LoadInfo()
        {
            string query = "SELECT * " +
                           "FROM documents " +
                           "WHERE id = '" + _id + "'";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return;

                tB_npp.Text = _id.ToString();
                tB_fio.Text = dt.Rows[0]["fio"].ToString();
                tB_address.Text = dt.Rows[0]["address_reg"].ToString();
                tB_serviceId.Text = dt.Rows[0]["id_uslugi"].ToString();
                _gosuslugId = dt.Rows[0]["id_uslugi"].ToString();
                tB_service.Text = dt.Rows[0]["name_usl"].ToString();

                if (!string.IsNullOrEmpty(dt.Rows[0]["birthday"].ToString()))
                {
                    if (DateTime.TryParse(dt.Rows[0]["birthday"].ToString(), out DateTime date))
                        tB_birthday.Text = date.ToString("d");
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["date_add"].ToString()))
                {
                    if (DateTime.TryParse(dt.Rows[0]["date_add"].ToString(), out DateTime date))
                        tB_dateAdd.Text = date.ToString("d");
                }
                if (!string.IsNullOrEmpty(dt.Rows[0]["date_ispoln"].ToString()))
                {
                    if (DateTime.TryParse(dt.Rows[0]["date_ispoln"].ToString(), out DateTime date))
                        tB_dateAdd.Text = date.ToString("d");
                }

                switch (dt.Rows[0]["status"].ToString())
                {
                    case "0":
                        tB_status.Text = @"Не принят";
                        break;
                    case "1":
                        tB_status.Text = @"Ожидание приёма в работу";
                        break;
                    case "2":
                        tB_status.Text = @"Отказано в приёме документа";
                        break;
                    case "3":
                        tB_status.Text = @"Принят на рассмотрение";
                        break;
                    case "4":
                        tB_status.Text = @"Отказано в выдаче";
                        break;
                    case "5":
                        tB_status.Text = @"Ожидает заявителя";
                        break;
                    case "6":
                        tB_status.Text = @"Документ выдан";
                        break;
                    case "7":
                        tB_status.Text = @"Ожидание выдачи";
                        break;
                    case "8":
                        tB_status.Text = @"Ожидание документов";
                        break;
                    case "9":
                        tB_status.Text = @"Ответ с Портала";
                        break;
                }

                int.TryParse(dt.Rows[0]["file_id"].ToString(), out _fileId);
                if (_fileId == 0)
                {
                    lbl_fileAdd.Enabled = false;
                    btn_fileAddOpen.Enabled = false;
                    btn_fileAddOpen.BackColor = Color.FromArgb(60, 91, 116);
                    btn_fileAddDownload.Enabled = false;
                    btn_fileAddDownload.BackColor = Color.FromArgb(60, 91, 116);
                }

                int.TryParse(dt.Rows[0]["file_arrived"].ToString(), out _fileArrived);
                if (_fileArrived == 0)
                {
                    lbl_arrived.Enabled = false;
                    btn_fileArrivedOpen.Enabled = false;
                    btn_fileArrivedOpen.BackColor = Color.FromArgb(60, 91, 116);
                    btn_fileArrivedDownload.Enabled = false;
                    btn_fileArrivedDownload.BackColor = Color.FromArgb(60, 91, 116);
                }

                int.TryParse(dt.Rows[0]["file_apply"].ToString(), out _fileApply);
                if (_fileApply == 0)
                {
                    lbl_apply.Enabled = false;
                    btn_fileApplyOpen.Enabled = false;
                    btn_fileApplyOpen.BackColor = Color.FromArgb(60, 91, 116);
                    btn_fileApplyDownload.Enabled = false;
                    btn_fileApplyDownload.BackColor = Color.FromArgb(60, 91, 116);
                }

                int.TryParse(dt.Rows[0]["file_finaly"].ToString(), out _fileFinaly);
                if (_fileFinaly == 0)
                {
                    lbl_finaly.Enabled = false;
                    btn_fileFinalyOpen.Enabled = false;
                    btn_fileFinalyOpen.BackColor = Color.FromArgb(60, 91, 116);
                    btn_fileFinalyDownload.Enabled = false;
                    btn_fileFinalyDownload.BackColor = Color.FromArgb(60, 91, 116);
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine(ex);
                //throw;
            }
        }

        /// <summary>
        /// Получить путь к файлу на FTP
        /// </summary>
        /// <param name="fileId">ID файла</param>
        /// <returns></returns>
        private string GetFileInfo(int fileId)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            string query = "SELECT file " +
                           "FROM files " +
                           "WHERE id = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", fileId);

            try
            {
                string ans = cmd.ExecuteScalar().ToString();
                return ans;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("При получении файла из базы данных возникла следующая ошибка: " + ex.Message);
                return string.Empty;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        private void OpenFile(int id)
        {
            WorkMethods.CheckTempDirectory();
            _path = "temp\\Документ_" + _id + ".xml";
            string pathFtp = GetFileInfo(id);
            if (string.IsNullOrEmpty(pathFtp))
            {
                MessageBox.Show(@"Не найден путь в базе данных", @"Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!MyFtp.DownloadFromFtp(_path, pathFtp))
            {
                MessageBox.Show(@"Возникла ошибка при скачивании файла", @"Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (!File.Exists(_path))
            {
                MessageBox.Show(@"Файл не найден или он пустой", @"Внимание",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            WorkMethods.OpenFile(_path);
        }

        /// <summary>
        /// Скачать файл
        /// </summary>
        private void DownloadFile(int id)
        {
            WorkMethods.CheckTempDirectory();
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xml файлы (*.xml)|*.xml",
                FileName = "Документ_" + id
            };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                string pathFtp = GetFileInfo(id);
                if (string.IsNullOrEmpty(pathFtp))
                {
                    MessageBox.Show(@"Не найден путь в базе данных", @"Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (MyFtp.DownloadFromFtp(_path, pathFtp))
                {
                    MessageBox.Show(@"Возникла ошибка при скачивании файла", @"Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!File.Exists(_path))
                {
                    MessageBox.Show(@"Файл не найден или он пустой", @"Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                MessageBox.Show(@"Документ загружен", @"Информация",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        #endregion
    }
}

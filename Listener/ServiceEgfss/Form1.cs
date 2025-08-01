using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FluentFTP;
using MySql.Data.MySqlClient;
using ServiceMinsoc.Properties;
using RestSharp;
using ServiceMinsoc.Json;

namespace ServiceMinsoc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            Logger.InitLogger();
            Logger.Log.Info("----------Старт программы. Начало работы----------");
            Connection.Create();
            InitializeComponent();
            this.SavingOn();
            MyStatic.ProgramName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
        }


        #region Переменные

        /// <summary>
        /// ip-адрес или имя сервера, который слушает
        /// </summary>
        private static string _url = Settings.Default.Url;
        /// <summary>
        /// порт, который прослушивает сервер
        /// </summary>
        private static string _port = Settings.Default.Port;

        /// <summary>
        /// флаг определяет запущен ли сервер
        /// </summary>
        private bool _isListening = false;//Settings.Default.isListening;
        /// <summary>
        /// есть ли программа в автозагрузке
        /// </summary>
        public static bool IsAautorun = Settings.Default.isAutorun;

        /// <summary>
        ///объект прослушивателя протокола HTTP
        /// </summary>
        private HttpListener _listener;
        /// <summary>
        /// прослушивание будет работать в отдельном потоке
        /// </summary>
        private Thread _bgThread;
        
        
        private bool _enablePost;

        #endregion


        #region Формы, кнопки, обработчики

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("files"))
                Directory.CreateDirectory("files");
            _enablePost = Settings.Default.PostEnable;

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
            Text += @" [" + Settings.Default.Url + @":" + Settings.Default.Port + @"]";

            //_programName = AppDomain.CurrentDomain.FriendlyName;
            //_programName = _programName.Substring(0, _programName.Length - 4);

            //Avtozapusk();
            //_isListening = false;

#if DEBUG
            _enablePost = false;
#endif
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            CheckUpdate(false);
            if (Settings.Default.isListening == true && Settings.Default.isAutorun == true)
            {
                button_start_stop_Click(sender, e);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Logger.Log.Info("----------Завершение работы программы----------");
            Environment.Exit(0);
        }

        private void button_start_stop_Click(object sender, EventArgs e)
        {
            Settings.Default.isListening = !_isListening;
            Settings.Default.Save();
            if (!_isListening)
            {
                if (!Connection.CheckFtpConnection())
                {
                    AddDataGrid("warn", "Ошибка при подключении к FTP-серверу");
                    return;
                }
                if (!Connection.CheckConnection())
                {
                    AddDataGrid("warn", "Ошибка при подключении к базе данных");
                    return;
                }
                Logger.Log.Info("Запуск сервиса.");
                AddDataGrid("info", "Запуск сервиса");
                button_start_stop.Image = Resources.denied_icon;
                button_settings.Enabled = false;
                timer1.Interval = Settings.Default.Interval * 60000;
                timer1.Start();
                timer1_Tick(sender, e);
                StartListen();
                Settings.Default.isListening = true;
                Settings.Default.Save();
            }
            else
            {
                AddDataGrid("info", "Остановка сервиса");
                Logger.Log.Info("Остановка сервиса.");
                button_start_stop.Image = Resources.play_icon;
                button_settings.Enabled = true;
                timer1.Stop();
                StopListen();
                Settings.Default.isListening = false;
                Settings.Default.Save();
            }
        }

        private void button_settings_Click(object sender, EventArgs e)
        {
            FormSettings form = new FormSettings();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
            Connection.Create();
            timer1.Interval = Settings.Default.Interval*60000;
            _url = Settings.Default.Url;
            _port = Settings.Default.Port;
            _enablePost = Settings.Default.PostEnable;
            Text = Text.Substring(0, Text.IndexOf("[", StringComparison.Ordinal)) +
                   @"[" + Settings.Default.Url + @":" + Settings.Default.Port + @"]";
        }

        private void button_logs_Click(object sender, EventArgs e)
        {
            string folder = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
            if (Directory.Exists(folder))
                System.Diagnostics.Process.Start("explorer", folder);
        }

        #endregion


        #region Ожидание информации с порта

        /// <summary>
        /// метод запускает отдельный поток,
        /// в котором вызывается метод инициализации прослушивания
        /// </summary>
        public void StartListen()
        {
            try
            {
                _bgThread = new Thread(new ThreadStart(TaskStart));
                _bgThread.IsBackground = true;
                _bgThread.Name = "MyHttpListener";
                _bgThread.Start();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Создание потока MyHttpListener. " + ex.Message);
            }
        }

        /// <summary>
        /// остановка сервера прослушивания
        /// </summary>
        public void StopListen()
        {
            try
            {
                if (_listener.IsListening)
                {
                    _listener.Stop();
                    _listener.Close();
                }
                Logger.Log.Info("Сервис остановлен.");
                _isListening = false;
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Остановка сервиса. " + ex.Message);
                AddDataGrid("warn", "Остановка сервиса. " + ex.Message);
            }
        }

        /// <summary>
        /// Опредение ip-адреса текущей машины
        /// </summary>
        private void GetIp()
        {
            try
            {
                // Получение имени компьютера.
                String host = Dns.GetHostName();
                // Получение ip-адреса.
                IPAddress ip = Dns.GetHostByName(host).AddressList[0];
                _url = "http://" + ip;
                Settings.Default.Url = _url;
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Получение имени и ip адреса. " + ex.Message); 
            }
        }

        /// <summary>
        /// слушаем входящие соединения, принимаем-обработываем запросы,
        /// отправляем ответы
        /// </summary>
        public void TaskStart()
        {
            try
            {
                _isListening = true;
                _listener = new HttpListener();
                string url2;
                if (_url.IndexOf("http") != -1)
                    url2 = _url;
                else
                    url2 = "http://" + _url;
                string prefix = string.Format("{0}:{1}/", url2, _port);
                _listener.Prefixes.Add(prefix);
                _listener.Start();
                Logger.Log.Info("Сервис запущен.");
                AddDataGrid("info", "Сервис запущен");
            }
            catch (Exception ex)
            {
                //StopListen();
                Logger.Log.Info("Запуск листенера. " + ex.Message);
                AddDataGrid("Ошибка", "При запуске листенера: " + ex.Message);
                //MessageBox.Show("Проверьте, все ли вы правильно указали в настройках", "Внимание");
                //isListening = false;
            }
            if (!HttpListener.IsSupported) return; // текущая ос не поддерживается

            while (_listener.IsListening)//сервер запущен? Тогда слушаем входящие соединения
            {
                try
                {
                    HttpListenerContext context = _listener.GetContext();  //ожидаем входящие запросы                    
                    Thread obrThread = new Thread(new ParameterizedThreadStart(ObrabotkaPost));
                    obrThread.IsBackground = true;
                    obrThread.Start(context);
                }
                catch (HttpListenerException ex)
                {
                    if (ex.Message == "Операция ввода/вывода была прервана из-за завершения потока команд или по запросу приложения")
                        return;
                    //Logger.Log.Error("Получение входящего запроса. " + ex.Message);
                    Logger.Log.Warn("Операция ввода/вывода была прервана из-за завершения потока команд или по запросу приложения. " + ex.Message);
                }
            }
        }

        /// <summary>
        /// в отдельном потоке выполняется обработка запроса
        /// </summary>
        /// <param name="context1"></param>
        private void ObrabotkaPost(Object context1)
        {
            string dannye = "";
            string responseSait = "";
            HttpListenerContext context = null;

            try
            {
                context = (HttpListenerContext)context1;
                HttpListenerRequest request = context.Request;

                #region обрабатываем POST запрос

                if (request.HttpMethod == "POST") //запрос получен методом POST (пришли данные формы)
                {
                    AddDataGrid("info", "Новый запрос");
                    dannye = ShowRequestData(request);
                    Logger.Log.Info("Новый запрос\n" + dannye);

                    responseSait = Obrabotka(dannye);
                }

                #endregion
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Обработка входящего запроса. " + ex.Message);
                responseSait = "Обработка входящего запроса. " + ex.Message;
            }
            
            try
            {
                //отправка данных клиенту
                if (context == null)
                    return;
                HttpListenerResponse response = context.Response;
                response.ContentType = "text/html; charset=UTF-8";
                byte[] buffer = Encoding.UTF8.GetBytes(responseSait);
                response.ContentLength64 = buffer.Length;

                using (Stream output = response.OutputStream)
                {
                    output.Write(buffer, 0, buffer.Length);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка ответа на POST-запрос: " + ex.Message);
                AddDataGrid("Ошибка ответа", ex.Message);
            }
        }

        /// <summary>
        /// Обработка входящего заявления
        /// </summary>
        /// <param name="dannye">Пришедшие данные</param>
        /// <returns></returns>
        private string Obrabotka(string dannye)
        {
            string response = "";

            if (dannye == "")
            {
                string mes = "Получен пустой текст";
                AddDataGrid("info", mes);
                Logger.Log.Info(mes);
                response = mes;
            }
            else
            {
                JsonInput input = JsonWorker.Deserializ(dannye);
                if (input != null)
                {
                    string mes;
                    if (!string.IsNullOrEmpty(input.Link))
                    {
                        Uri uri;
                        if (Uri.TryCreate(input.Link, UriKind.RelativeOrAbsolute, out uri) && uri.IsAbsoluteUri)
                        {
                            MyReceiveFile receive = new MyReceiveFile
                            {
                                EnablePost = _enablePost,
                                ServiceId = input.OpderId
                            };
                            string ans = receive.DownloadFile(uri.AbsoluteUri);
                            if (ans == "ok")
                            {
                                mes = receive.Info + " [" + uri.AbsoluteUri + "]";
                                AddDataGrid("info", mes);
                                Logger.Log.Info(mes);
                                response = "OK";
                            }
                            else
                            {
                                mes = ans + " [" + uri.AbsoluteUri + "]";
                                AddDataGrid("error", mes);
                                Logger.Log.Warn(mes);
                                response = ans;
                            }
                        }
                        else
                        {
                            mes = "Не удалось преобразовать ссылку [" + input.Link + "]";
                            Logger.Log.Warn(mes);
                            AddDataGrid("error", mes);
                            response = mes;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(input.Xml))
                        {
                            if (MyBase64.Decode(input.Xml) == "")
                            {
                                mes = "Ошибка декодирования base64 строки";
                                Logger.Log.Warn(mes);
                                AddDataGrid("warn", mes);
                                response = mes;
                            }
                            else
                            {
                                MyReceiveFile receive = new MyReceiveFile
                                {
                                    EnablePost = _enablePost,
                                    ServiceId = input.OpderId
                                };
                                string ans = receive.AddBase64(input.Xml);
                                if (ans == "ok")
                                {
                                    AddDataGrid("info", receive.Info);
                                    Logger.Log.Info(receive.Info);
                                    response = "OK";
                                }
                                else
                                {
                                    AddDataGrid("error", ans);
                                    Logger.Log.Warn(ans);
                                    response = ans;
                                }
                            }
                        }
                        else
                        {
                            mes = "Пустой параметр base64-строки в JSON";
                            Logger.Log.Warn(mes);
                            AddDataGrid("error", mes);
                            response = mes;
                        }
                    }
                }
                else
                {
                    if (MyBase64.Decode(dannye) == "")
                    {
                        string mes = "Ошибка декодирования base64 строки";
                        Logger.Log.Warn(mes);
                        AddDataGrid("warn", mes);
                        response = mes;
                    }
                    else
                    {
                        MyReceiveFile receive = new MyReceiveFile { EnablePost = _enablePost };
                        string ans = receive.AddBase64(dannye);
                        if (ans == "ok")
                        {
                            AddDataGrid("info", receive.Info);
                            Logger.Log.Info(receive.Info);
                            response = "OK";
                        }
                        else
                        {
                            AddDataGrid("error", ans);
                            Logger.Log.Warn(ans);
                            response = ans;
                        }
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// получаем из запроса данные
        /// </summary>
        /// <param name="request">объект запроса</param>
        /// <returns> строку данных</returns>
        private static string ShowRequestData(HttpListenerRequest request)
        {
            try
            {
                //есть данные от клиента?
                if (!request.HasEntityBody) return "";

                //смотрим, что пришло
                using (Stream body = request.InputStream)
                {
                    //using (StreamReader reader = new StreamReader(body, Encoding.UTF8, true, 1024))
                    using (StreamReader reader = new StreamReader(body, Encoding.UTF8, true, 1024, true))
                    {
                        string text = reader.ReadToEnd();
                        return text;
                    }
                }
                //using (StreamReader reader = new StreamReader(request.InputStream, Encoding.UTF8, true, 1024, true))
                //{
                //    string text = reader.ReadToEnd();
                //    return text;
                //}

            }
            catch (Exception ex)
            {
                Logger.Log.Warn(ex.Message); 
                return "";
            }
        }

        /// <summary>
        /// запись в dataGrid из другого потока
        /// </summary>
        /// <param name="status">трэк-номер</param>
        /// <param name="tip">тип запроса</param>
        /// <param name="zapros">тело запроса</param>
        private void AddDataGrid(string status, string info)
        {
            try
            {
                if (dGV.InvokeRequired)
                {
                    dGV.BeginInvoke(new Action(() =>
                    {
                        dGV.Rows.Insert(0, 1);
                        dGV.Rows[0].Cells[0].Value = "";
                        dGV.Rows[0].Cells[1].Value = status;
                        dGV.Rows[0].Cells[2].Value = DateTime.Now;
                        dGV.Rows[0].Cells[3].Value = info;
                        dGV.FirstDisplayedScrollingRowIndex = 0;
                        dGV.RowCount = Convert.ToInt32(500);
                    }));
                }
                else
                {
                    dGV.Rows.Insert(0, 1);
                    dGV.Rows[0].Cells[0].Value = "";
                    dGV.Rows[0].Cells[1].Value = status;
                    dGV.Rows[0].Cells[2].Value = DateTime.Now;
                    dGV.Rows[0].Cells[3].Value = info;
                    dGV.FirstDisplayedScrollingRowIndex = 0;
                    dGV.RowCount = Convert.ToInt32(500);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Добавление записи в  datagrid: " + ex.Message); 
            }
        }

        #endregion


        #region Таймер с проверкой базы

        private void timer1_Tick(object sender, EventArgs e)
        {
            Thread thread = new Thread(CheckDb);
            thread.Start();
            //CheckDb();
        }

        /// <summary>
        /// Проверка новых записей в базе данных.
        /// </summary>
        private void CheckDb()
        {
            DataTable dt = GetInfoDb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool flagArrived = true;
                bool flagApply = true;
                bool flagReturn = true;
                bool flagNotification = true;

                if (dt.Rows[i]["flag_file_arrived"].ToString() == "0" ||
                    dt.Rows[i]["flag_file_arrived"].ToString() == "False")
                {
                    DataTable dtFile = GetFileFromMySql(dt.Rows[i]["file_arrived"].ToString());
                    if (dtFile?.Rows.Count > 0)
                    {
                        if (SendToPortal(dtFile.Rows[0]))
                        {
                            if (UpdateFlagServiceMySql(dt.Rows[i]["id"].ToString(), "flag_file_arrived"))
                            {
                                string info = "В документе c ID:[" + dt.Rows[i]["id"] + "] файл с уведомлением";
                                Logger.Log.Info(info);
                                AddDataGrid("info", info);
                            }
                            else
                            {
                                flagArrived = false;
                            }
                        }
                        else
                        {
                            flagArrived = false;
                        }
                    }
                }

                if (flagArrived &&
                    dt.Rows[i]["flag_file_apply"].ToString() == "0" ||
                    dt.Rows[i]["flag_file_apply"].ToString() == "False")
                {
                    DataTable dtFile = GetFileFromMySql(dt.Rows[i]["file_apply"].ToString());
                    if (dtFile?.Rows.Count > 0)
                    {
                        if (SendToPortal(dtFile.Rows[0]))
                        {
                            if (UpdateFlagServiceMySql(dt.Rows[i]["id"].ToString(), "flag_file_apply"))
                            {
                                string info = "В документе c ID:[" + dt.Rows[i]["id"] + "] файл с принятым решением";
                                Logger.Log.Info(info);
                                AddDataGrid("info", info);
                            }
                            else
                            {
                                flagApply = false;
                            }
                        }
                        else
                        {
                            flagApply = false;
                        }
                    }
                }

                if (flagArrived && flagApply &&
                    dt.Rows[i]["flag_file_return"].ToString() == "0" ||
                    dt.Rows[i]["flag_file_return"].ToString() == "False")
                {
                    DataTable dtFile = GetFileFromMySql(dt.Rows[i]["file_return"].ToString());
                    if (dtFile?.Rows.Count > 0)
                    {
                        if (SendToPortal(dtFile.Rows[0]))
                        {
                            if (UpdateFlagServiceMySql(dt.Rows[i]["id"].ToString(), "flag_file_return"))
                            {
                                string info = "В документе c ID:[" + dt.Rows[i]["id"] + "] файл на доработку";
                                Logger.Log.Info(info);
                                AddDataGrid("info", info);
                            }
                            else
                            {
                                flagReturn = false;
                            }
                        }
                        else
                        {
                            flagReturn = false;
                        }
                    }
                }

                if (flagArrived && flagApply && flagReturn &&
                    dt.Rows[i]["flag_file_notification"].ToString() == "0" ||
                    dt.Rows[i]["flag_file_notification"].ToString() == "False")
                {
                    DataTable dtFile = GetFileFromMySql(dt.Rows[i]["file_apply"].ToString());
                    if (dtFile?.Rows.Count > 0)
                    {
                        if (SendToPortal(dtFile.Rows[0]))
                        {
                            if (UpdateFlagServiceMySql(dt.Rows[i]["id"].ToString(), "flag_file_notification"))
                            {
                                string info = "В документе c ID:[" + dt.Rows[i]["id"] + "] файл с уведомлением";
                                Logger.Log.Info(info);
                                AddDataGrid("info", info);
                            }
                            else
                            {
                                flagNotification = false;
                            }
                        }
                        else
                        {
                            flagNotification = false;
                        }
                    }
                }

                if (flagArrived && flagApply && flagReturn && flagNotification &&
                    dt.Rows[i]["flag_file_finaly"].ToString() == "0" ||
                    dt.Rows[i]["flag_file_finaly"].ToString() == "False")
                {
                    //SendToPortal(GetFileFromMySql(dt.Rows[i]["file_finaly"].ToString()).Rows[0]); - прост для сохранения
                    DataTable dtFile = GetFileFromMySql(dt.Rows[i]["file_finaly"].ToString());
                    if (dtFile?.Rows.Count > 0)
                    {
                        if (dt.Rows[i]["service_id"].ToString() == "278" &&
                            (dt.Rows[i]["status"].ToString() == "6" ||
                             (dt.Rows[i]["status"].ToString() == "4" &&
                              !string.IsNullOrEmpty(dt.Rows[i]["file_incoming"].ToString()))))
                        {
                            DataTable dtFileApply = GetFileFromMySql(dt.Rows[i]["file_apply"].ToString());
                            if (!SendToPortalNotFile(dtFileApply.Rows[0]))
                                continue;
                        }

                        if (SendToPortal(dtFile.Rows[0]))
                        {
                            if (UpdateFlagServiceMySql(dt.Rows[i]["id"].ToString(), "flag_file_finaly"))
                            {
                                string info = "В документе c ID:[" + dt.Rows[i]["id"] +
                                              "] файл с результатом оказания услуги";
                                Logger.Log.Info(info);
                                AddDataGrid("info", info);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Получение информации из базы данных.
        /// </summary>
        /// <returns></returns>
        private DataTable GetInfoDb()
        {
            string query = "SELECT id, file_arrived, flag_file_arrived, file_apply, flag_file_apply, file_finaly, flag_file_finaly, " +
                           "file_return, flag_file_return, file_notification, flag_file_notification, file_incoming, service_id, status " +
                           "FROM documents " +
                           "WHERE (file_apply is not null and flag_file_apply = '0') or (file_finaly is not null and flag_file_finaly = '0') " +
                           "or (file_arrived is not null and flag_file_arrived = '0') or (file_return is not null and flag_file_return = '0') " +
                           "or (file_notification is not null and flag_file_notification = '0')";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Ошибка при получении данных из базы данных: " + ex.Message);
                return new DataTable();
            }
        }

        /// <summary>
        /// Обновление флага в таблице документов.
        /// </summary>
        /// <param name="documentsId">ID документа</param>
        /// <param name="flag">Тип флага (0 - файл попал в ведомство, 1 - принятие документа, 2 - выдача документа)</param>
        /// <returns></returns>
        private bool UpdateFlagServiceMySql(string documentsId, int flag)
        {
            //string nameColumn = flag == 0 ? "flag_file_apply" : "flag_file_finaly";
            string nameColumn = "";

            switch (flag)
            {
                case 0:
                    nameColumn = "flag_file_arrived";
                    break;
                case 1:
                    nameColumn = "flag_file_apply";
                    break;
                case 2:
                    nameColumn = "flag_file_finaly";
                    break;
            }

            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE documents SET " +
                              nameColumn + " = @" + nameColumn + " " +
                              "WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", documentsId);
            cmd.Parameters.AddWithValue("@" + nameColumn, 1);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("В методе UpdateFlagServiceMySql возникла ошибка: " + ex.Message);
                AddDataGrid("warn", "При обновлении флага в БД возникла ошибка: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Обновление флага в таблице документов.
        /// </summary>
        /// <param name="documentsId">ID документа</param>
        /// <param name="nameColumn">Наименование столбца</param>
        /// <returns></returns>
        private bool UpdateFlagServiceMySql(string documentsId, string nameColumn)
        {
            //string nameColumn = flag == 0 ? "flag_file_apply" : "flag_file_finaly";
            //string nameColumn = "";

            //switch (flag)
            //{
            //    case 0:
            //        nameColumn = "flag_file_arrived";
            //        break;
            //    case 1:
            //        nameColumn = "flag_file_apply";
            //        break;
            //    case 2:
            //        nameColumn = "flag_file_finaly";
            //        break;
            //}

            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "UPDATE documents SET " +
                              nameColumn + " = @" + nameColumn + " " +
                              "WHERE id = @id";

            cmd.Parameters.AddWithValue("@id", documentsId);
            cmd.Parameters.AddWithValue("@" + nameColumn, 1);

            try
            {
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("В методе UpdateFlagServiceMySql возникла ошибка: " + ex.Message);
                AddDataGrid("warn", "При обновлении флага в БД возникла ошибка: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Получить base64 строку из базы.
        /// </summary>
        /// <param name="fileId">ID файла</param>
        /// <returns></returns>
        private DataTable GetFileFromMySql(string fileId)
        {
            string query = "SELECT * " +
                           "FROM files " +
                           "WHERE id = '" + fileId + "'";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Ошибка при получении файла из базы данных: " + ex.Message);
                return new DataTable();
            }
        }

        /// <summary>
        /// Отправить информацию на портал.
        /// </summary>
        private bool SendToPortal(DataRow row)
        {
            if (row == null)
                return false;
            if (!_enablePost)
                return true;

            string url = "https://uslugi.gospmr.org/serviceresponse.php";
            string param = row["params"].ToString();
            string signature = row["param_signature"].ToString();

            FtpClient client = new FtpClient(Settings.Default.Ftp_Url)
            {
                Credentials = new NetworkCredential(Settings.Default.Ftp_User, Settings.Default.Ftp_Pass),
                Port = Settings.Default.Ftp_Port
            };
            string path = "files\\upload_file_" + row["id"] + ".xml";
            try
            {
                client.Connect();
                client.DownloadFile(path, row["file"].ToString());
            }
            catch (Exception)
            {
                Logger.Log.Warn("ID файла:[" + row["id"] + "] Ошибка при загрузке информации с FTP-сервера");
                AddDataGrid("warn", "ID файла:[" + row["id"] + "] Ошибка при загрузке информации с FTP-сервера");
                if (File.Exists(path))
                    File.Delete(path);
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }

            string file = MyStatic.GetStringFromFile(path);

            if (signature == "")    //не отправлять на Портал, если сигнатура пустая
            {
                Logger.Log.Info("ID файла:[" + row["id"] + "] signature: Пусто");
                if (File.Exists(path))
                    File.Delete(path);
                return false;
            }
            //Logger.Log.Info("file: " + file);

            string ans = Post(url, param, signature, file);
            if (ans == "ok" || ans == "Ошибка: Услуга уже оказана")
            {
                Logger.Log.Info("ID файла:[" + row["id"] + "] status: " + ans);
                AddDataGrid("info", "ID файла:[" + row["id"] + "] status: " + ans);
                if (File.Exists(path))
                    File.Delete(path);
                return true;
            }

            Logger.Log.Warn("ID файла:[" + row["id"] + "] возникла ошибка: " + ans);
            AddDataGrid("warn", "ID файла:[" + row["id"] + "] возникла ошибка: " + ans);
            if (File.Exists(path))
                File.Delete(path);
            return false;
        }

        /// <summary>
        /// Отправить информацию на портал
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramSignature"></param>
        /// <param name="file"></param>
        /// <param name="idDoc"></param>
        /// <param name="idFile"></param>
        /// <returns></returns>
        private bool SendToPortal(string param, string paramSignature, string file, int idDoc, int idFile)
        {
            string url = "https://uslugi.gospmr.org/serviceresponse.php";
            string ans = Post(url, param, paramSignature, file);
            if (ans == "ok")
            {
                string mes = "ID документа:[" + idDoc + "] файл ID:[" + idFile + "] status: " + ans;
                Logger.Log.Info(mes);
                AddDataGrid("info", mes);
                return true;
            }
            else
            {
                string mes = "ID документа:[" + idDoc + "] файл ID:[" + idFile + "] возникла ошибка: " + ans;
                Logger.Log.Warn(mes);
                AddDataGrid("warn", mes);
                return false;
            }
        }

        /// <summary>
        /// Отправить информацию на портал без файла
        /// </summary>
        /// <param name="row">Данные</param>
        /// <returns></returns>
        private bool SendToPortalNotFile(DataRow row)
        {
            if (row == null)
                return false;

            string url = "https://uslugi.gospmr.org/serviceresponse.php";
            string param = row["params"].ToString();
            string signature = row["param_signature"].ToString();

            if (signature == "")
            {
                return false;
            }

            string ans = Post(url, param, signature, "");
            if (ans == "ok" || ans == "Ошибка: Услуга уже оказана")
            {
                Logger.Log.Info("ID уведомления без файла:[" + row["id"] + "] status: " + ans);
                AddDataGrid("info", "ID уведомления без файла:[" + row["id"] + "] status: " + ans);
                return true;
            }

            Logger.Log.Warn("ID уведомления без файла:[" + row["id"] + "] возникла ошибка: " + ans);
            AddDataGrid("warn", "ID уведомления без файла:[" + row["id"] + "] возникла ошибка: " + ans);
            return false;
        }

        /// <summary>
        /// POST запрос
        /// </summary>
        /// <param name="url">POST-запрос по следуюзему URL</param>
        /// <param name="_params">Параметры выполнения услуги (JSON строка)</param>
        /// <param name="signature">Электронная цифровая подпись</param>
        /// <param name="file">Документ, предоставляемый при подтверждении</param>
        /// <returns></returns>
        private string Post(string url, string _params, string signature, string file)
        {
            if (!_enablePost)
                return "ok";
            try
            {
                RestClient client = new RestClient(url);
                RestRequest request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "multipart/form-data");
                request.AddParameter("params", _params);
                request.AddParameter("signature", signature);
                request.AddFile("document", Encoding.UTF8.GetBytes(file), "Уведомление.xml", "text/xml");
                return client.Execute(request).Content;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("Возникла ошибка при отправке документа: " + ex.Message);
                return null;
            }
        }

        #endregion


        #region Своричивание в панель задач

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
            }
        }

        private void notifyIcon1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowInTaskbar = true;
                notifyIcon1.Visible = false;
                WindowState = FormWindowState.Normal;
            }
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Visible = true;
            }
        }

        private void toolStripMenuItem_open_Click(object sender, EventArgs e)
        {
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        private void toolStripMenuItem_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            toolStripMenuItem_start.Text = _isListening ? "Отключить" : "Запустить";
        }

        #endregion


        #region Автозапуск

        /// <summary>
        /// запускает листенер, если он был запущен при предыдыщем выходе из программы
        /// Проверяет есть ли программа в автозагрузке, при необходимости добавляет/удаляет
        /// </summary>
        private void Avtozapusk()
        {
            if (_isListening == true && Settings.Default.isAutorun == true)
            {
                Logger.Log.Info("Запуск сервиса.");
                AddDataGrid("info", "Запуск сервиса");
                button_start_stop.Image = Resources.denied_icon;
                button_settings.Enabled = false;
                timer1.Interval = Settings.Default.Interval * 60000;
                timer1.Start();
                StartListen();
                //bt_StartStop.Text = "Остановить сервер";
                //btSetting.Enabled = false;
            }

            //Проверка наличия программы в автозагрузке
            try
            {
                ManagementObjectSearcher searcher =
                    new ManagementObjectSearcher("root\\CIMV2",
                        "SELECT * FROM Win32_StartupCommand"); //WMI запросом получаем список всех программ в автозагрузке 
                bool yes = searcher.Get().Cast<ManagementObject>().Any(queryObj => MyStatic.ProgramName == queryObj["Name"].ToString());

                if (!yes && IsAautorun)
                    SetAutorunValue(true);
                if (yes && !IsAautorun)
                    SetAutorunValue(false);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Проверка наличя программы в автозапуске. " + ex.Message);
            }
        }

        /// <summary>
        /// добавляет или удаляет программу из автозагрузки
        /// </summary>
        /// <param name="autorun">true - добавление; false - удаление</param>
        /// <returns></returns>
        public static bool SetAutorunValue(bool autorun)
        {
            try
            {
                string ExePath = Application.ExecutablePath;
                Microsoft.Win32.RegistryKey reg =
                    Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                        "Software\\Microsoft\\Windows\\CurrentVersion\\Run\\");

                if (autorun)
                    reg.SetValue(MyStatic.ProgramName, ExePath);
                else
                    reg.DeleteValue(MyStatic.ProgramName);

                reg.Close();
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Добавление/удаление из автозагрузки. " + ex.Message);
                return false;
            }
            return true;
        }

        #endregion


        #region Обновление

        private void button_update_Click(object sender, EventArgs e)
        {
            CheckUpdate(true);
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

        private void CheckUpdate(bool search)
        {
            try
            {
                string crit = GET("https://gupcit.com/data/update/minsoc/service/criticalornot.txt");
                string vers = GET("https://gupcit.com/data/update/minsoc/service/version.txt");

                Version updaite_version = new Version(vers);
                Version curent_version = new Version(Application.ProductVersion); //System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (updaite_version > curent_version)
                {
                    if (crit.Equals("false") && search == true)
                        new Update.FormNewVersion(vers) { Owner = this }.ShowDialog();
                    else if (crit.Equals("true") && search == true)
                        new Update.FormNewVersionCritical(vers) { Owner = this }.ShowDialog();
                    else
                        ChangeNewImage();
                }
                else
                {
                    if (search)
                        MessageBox.Show(@"Новая версия не найдена");
                }
            }
            catch
            {
                if (search)
                    MessageBox.Show(@"Сервер обновлений временно недоступен");
            }

        }

        private void ChangeNewImage()
        {
            button_update.Text = @"Доступна новая версия программы!";
            button_update.Image = Resources.download_icon2;
        }

        #endregion


        #region Преобразование файлов

        private void button_sendFtp_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Вы действительно хотите отправить файлы из базы на FTP?", @"Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string query = "SELECT * FROM documents";
                MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
                DataTable dt = new DataTable();
                FtpClient client = new FtpClient(Settings.Default.Ftp_Url)
                { Credentials = new NetworkCredential(Settings.Default.Ftp_User, Settings.Default.Ftp_Pass) };
                try
                {
                    client.Connect();
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string path = "files\\file_temp.xml";
                        string ftpPath = "/" + dt.Rows[i]["id_uslugi"].ToString() + "/";
                        string fileId = dt.Rows[i]["file_id"].ToString();
                        string fileArrived = dt.Rows[i]["file_arrived"].ToString();
                        string fileApply = dt.Rows[i]["file_apply"].ToString();
                        string fileFinaly = dt.Rows[i]["file_finaly"].ToString();
                        string fileDirector = dt.Rows[i]["file_director"].ToString();

                        #region file_id

                        string file = GetFileInfo(fileId);
                        if (file != "" && MyBase64.Decode(file) != "")
                        {
                            MyBase64.SaveToFile(file, path);
                            client.UploadFile(path, ftpPath + "file.xml", FtpRemoteExists.Overwrite, true);
                            if (!UpdateFileInfo(fileId, ftpPath + "file.xml"))
                                MessageBox.Show(@"Ошибка при обновлении записи в базе данных (file)");
                        }

                        #endregion

                        #region file_arrived

                        file = GetFileInfo(fileArrived);
                        if (file != "" && MyBase64.Decode(file) != "")
                        {
                            MyBase64.SaveToFile(file, path);
                            client.UploadFile(path, ftpPath + "arrived.xml", FtpRemoteExists.Overwrite, true);
                            if (!UpdateFileInfo(fileArrived, ftpPath + "arrived.xml"))
                                MessageBox.Show(@"Ошибка при обновлении записи в базе данных (arrived)");
                        }

                        #endregion

                        #region file_finaly

                        file = GetFileInfo(fileApply);
                        if (file != "" && MyBase64.Decode(file) != "")
                        {
                            MyBase64.SaveToFile(file, path);
                            client.UploadFile(path, ftpPath + "apply.xml", FtpRemoteExists.Overwrite, true);
                            if (!UpdateFileInfo(fileApply, ftpPath + "apply.xml"))
                                MessageBox.Show(@"Ошибка при обновлении записи в базе данных (apply)");
                        }

                        #endregion

                        #region file_finaly

                        file = GetFileInfo(fileFinaly);
                        if (file != "" && MyBase64.Decode(file) != "")
                        {
                            MyBase64.SaveToFile(file, path);
                            client.UploadFile(path, ftpPath + "finaly.xml", FtpRemoteExists.Overwrite, true);
                            if (!UpdateFileInfo(fileFinaly, ftpPath + "finaly.xml"))
                                MessageBox.Show(@"Ошибка при обновлении записи в базе данных (file)");
                        }

                        #endregion

                        #region file_director

                        file = GetFileInfo(fileDirector);
                        if (file != "" && MyBase64.Decode(file) != "")
                        {
                            MyBase64.SaveToFile(file, path);
                            client.UploadFile(path, ftpPath + "director.xml", FtpRemoteExists.Overwrite, true);
                            if (!UpdateFileInfo(fileDirector, ftpPath + "director.xml"))
                                MessageBox.Show(@"Ошибка при обновлении записи в базе данных (file)");
                        }

                        #endregion



                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private string GetFileInfo(string id)
        {
            string query = "select * from files where id = '" + id + "'";

            MySqlDataAdapter da = new MySqlDataAdapter(query, Connection.Conn);
            DataTable dt = new DataTable();

            try
            {
                da.Fill(dt);
                if (dt.Rows.Count == 0)
                    return "";
                return dt.Rows[0]["file"].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        private bool UpdateFileInfo(string id, string newFileInfo)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            try
            {
                cmd.CommandText = "UPDATE files SET " +
                                  "file = @file " +
                                  "WHERE id = @id";

                cmd.Parameters.AddWithValue("@file", newFileInfo);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
    }
}

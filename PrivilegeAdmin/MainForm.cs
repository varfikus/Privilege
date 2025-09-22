using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using System.Net.Http;

namespace PrivilegeAdmin
{
    public partial class MainForm : Form
    {
        private readonly MyHttpClient _apiClient;
        private HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://192.168.69.236:5000";

        public MainForm()
        {
            InitializeComponent();

            //var handler = new HttpClientHandler
            //{
            //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
            //};
            //_httpClient = new HttpClient(handler);
            //_httpClient.BaseAddress = new Uri(_apiBaseUrl);

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiBaseUrl)
            };

            _apiClient = new MyHttpClient(httpClient);
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            Text += @" v1.0";
            Show();

            await LoadTableAsync();
            //CheckUpdate(false);
        }

        #region Кнопки

        /// <summary>
        /// Кнопка добавления пользователя
        /// </summary>
        private async void button_add_Click(object sender, EventArgs e)
        {
            UserForm form = new UserForm(_apiClient);
            form.StartPosition = FormStartPosition.CenterParent;

            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadTableAsync();
            }
        }

        /// <summary>
        /// Кнопка редактирования пользователя
        /// </summary>
        private async void button_edit_Click(object sender, EventArgs e)
        {
            if (dGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для редактирования.");
                return;
            }

            var selectedRow = dGV.SelectedRows[0];
            var userId = (int)selectedRow.Cells["Id"].Value;

            UserForm form = new UserForm(_apiClient, userId);
            form.StartPosition = FormStartPosition.CenterParent;

            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadTableAsync();
            }
        }

        /// <summary>
        /// Кнопка удаления пользователя
        /// </summary>
        private async void button_del_Click(object sender, EventArgs e)
        {
            if (dGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для удаления.");
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранного пользователя?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            var selectedRow = dGV.SelectedRows[0];
            var userId = (int)selectedRow.Cells["Id"].Value;
            var login = selectedRow.Cells["Login"].Value?.ToString();

            var userToDelete = new UserDto
            {
                Id = userId,
                Login = login
            };

            try
            {
                var result = await _apiClient.DeleteAsync<UserDto, BaseResult<UserDto>>("api/users", userToDelete);

                if (result != null && result.IsSuccess)
                {
                    MessageBox.Show("Пользователь успешно удалён.");
                }
                else
                {
                    MessageBox.Show($"Ошибка: {result?.ErrorMessage ?? "Неизвестная ошибка"} (код {result?.ErrorCode})");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        /// <summary>
        /// Кнопка для обновления таблицы пользователей
        /// </summary>
        private void button_refresh_Click(object sender, EventArgs e)
        {
            _ = LoadTableAsync();
        }

        /// <summary>
        /// Кнопка настроек
        /// </summary>
        private void button_settings_Click(object sender, EventArgs e)
        {
            //FormSettings form = new FormSettings();
            //form.StartPosition = FormStartPosition.CenterParent;
            //form.ShowDialog();
            //Connection.CreateConnection();
            _ = LoadTableAsync();
        }

        private void button_order_Click(object sender, EventArgs e)
        {
            ApplicationForm form = new ApplicationForm(_apiClient);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void button_files_Click(object sender, EventArgs e)
        {
            FileForm form = new FileForm(_apiClient);
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        #endregion

        #region Методы

        /// <summary>
        /// Метод для загрузки таблицы пользователей
        /// </summary>
        private async Task LoadTableAsync()
        {
            try
            {
                var result = await _apiClient.GetRawAsync<CollectionResult<User>>("api/users");

                if (result != null && result.IsSuccess && result.Data != null)
                {
                    this.dGV.Rows.Clear();

                    foreach (var user in result.Data)
                    {
                        dGV.Rows.Add(
                            user.Id,
                            user.Login
                        );
                    }
                }
                else
                {
                    MessageBox.Show($"Ошибка: {result?.ErrorMessage ?? "Неизвестная ошибка"} (код {result?.ErrorCode})");
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка HTTP-запроса: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке: {ex.Message}");
            }
        }

        //bool IsServerAvailable(string host)
        //{
        //    try
        //    {
        //        using (Ping ping = new Ping())
        //        {
        //            PingReply reply = ping.Send(host, 10000); 

        //            return reply.Status == IPStatus.Success;
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}
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
                System.Net.WebRequest req = System.Net.WebRequest.Create(url);
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream);
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
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls;
            try
            {
                string crit = GET("https://gupcit.com/data/update/minsoc/admin/criticalornot.txt");
                string vers = GET("https://gupcit.com/data/update/minsoc/admin/version.txt");

                Version updaite_version = new Version(vers);
                Version curent_version = new Version(System.Windows.Forms.Application.ProductVersion); //System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

                if (updaite_version > curent_version)
                {
                    //if (crit.Equals("false") && search == true)
                    //    new Update.FormNewVersion(updaite_version.ToString()) { Owner = this }.ShowDialog();
                    //else if (crit.Equals("true") && search == true)
                    //    new Update.FormNewVersionCritical(updaite_version.ToString()) { Owner = this }.ShowDialog();
                    //else
                    //    ChangeNewImage();
                }
                else
                {
                    if (search)
                        MessageBox.Show("Новая версия не найдена");
                }
            }
            catch
            {
                if (search)
                    MessageBox.Show("Сервер обновлений временно недоступен");
            }

        }

        private void ChangeNewImage()
        {
            //toolTip1.SetToolTip(this.button_update, "Доступна новая версия программы!");
            button_update.Text = @"Доступна новая версия программы!";
            //button_update.BackgroundImage = Properties.Resources.new_updates;
            //version.Foreground = new SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#b30000")); ;
        }

        #endregion
    }
}

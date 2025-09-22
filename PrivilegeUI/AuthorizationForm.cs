using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;
using PrivilegeUI.Classes.Json;
using PrivilegeUI.Classes.Json.Sub;
using PrivilegeUI.Update;
using System.Net;

namespace PrivilegeUI
{
    public partial class AuthorizationForm : Form
    {
        #region Fields

        private readonly string _apiBaseUrl = "http://192.168.69.236:5000";
        //private readonly string _apiBaseUrl = "http://localhost:5000";
        private MyHttpClient _apiClient;
        private int _userId;

        #endregion


        #region Constructor

        public AuthorizationForm()
        {
            InitializeComponent();

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri(_apiBaseUrl)
            };

            _apiClient = new MyHttpClient(httpClient);

            this.AcceptButton = btn_ok;
        }

        #endregion


        #region Events

        private void AuthorizationForm_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Login))
            {
                tB_login.Text = Properties.Settings.Default.Login;
                tB_pass.Text = Properties.Settings.Default.Password;
                cB_save.Checked = true;
            }
        }

        private async void btn_ok_Click(object sender, EventArgs e)
        {
            string login = tB_login.Text.Trim();
            string password = tB_pass.Text.Trim();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Пожалуйста, введите логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bool IsSuccess = await Login(login, password);

            if (!IsSuccess)
                return;

            SaveSettings();
            FormMain userForm = new FormMain(_userId, _apiClient);
            userForm.Show();
            this.Hide();
        }

        private void tB_pass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                btn_ok.PerformClick();
            }
        }

        #endregion


        #region Methods

        #region Authorization and Settings

        private async Task<bool> Login(string login, string password)
        {
            var loginDto = new AuthDto(login, password);

            var result = await _apiClient.PostAsync<AuthDto, BaseResult<TokenDto>>("api/auth/login", loginDto);

            if (result != null && result.IsSuccess && result.Data != null)
            {
                _apiClient.SetTokens(result.Data.AccessToken, result.Data.RefreshToken);
                _userId = result.Data.UserId;
                return true;
            }
            else
            {
                MessageBox.Show("Ошибка авторизации: " + (result?.ErrorMessage ?? "неизвестная ошибка"), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void SaveSettings()
        {
            if (cB_save.Checked)
            {
                Properties.Settings.Default.Login = tB_login.Text;
                Properties.Settings.Default.Password = tB_pass.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.Login = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Выполнение GET запроса
        /// </summary>
        /// <param name="url">Адрес ресурса</param>
        /// <returns>Ответ на запрос</returns>
        public static string GetNewVersion(string url)
        {
            try
            {
                WebRequest req = WebRequest.Create(url);
                WebResponse resp = req.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                System.IO.StreamReader sr = new System.IO.StreamReader(stream);
                string outText = sr.ReadToEnd();
                sr.Close();
                return outText;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Проверить наличие новой версии
        /// </summary>
        /// <param name="search"></param>
        private void CheckUpdate(bool search)
        {
            try
            {
                ServicePointManager.SecurityProtocol =
                    SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                string verJson = GetNewVersion("https://gupcit.com/data/update/privilege/client/" + "version.json");
                if (verJson == "")
                {
                    if (search)
                        MessageBox.Show(@"Пустой ответ от сервера обновлений. Возможно сервер обновлений недоступен.",
                            @"Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                JsonUpdate json = JsonWorker.DeserializVersion(verJson);
                if (json == null)
                {
                    MessageBox.Show(@"Не удалось преобразовать скачанный файл с версией", @"Внимание",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Version updaiteVersion = new Version(json.Version);
                Version curentVersion = new Version(System.Windows.Forms.Application.ProductVersion);

                if (updaiteVersion > curentVersion)
                {
                    if (!json.Critical && search == true)
                        new FormUpdateNew(json.Version) { Owner = this }.ShowDialog();
                    else if (json.Critical)
                        new FormUpdateCritical(json.Version) { Owner = this }.ShowDialog();
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

        #endregion

        #endregion
    }
}

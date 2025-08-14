


using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;

namespace PrivilegeAdmin
{
    public partial class UserForm : Form
    {
        private readonly int? _userId;
        private readonly MyHttpClient _apiClient;

        public UserForm(MyHttpClient apiClient, int? userId = null)
        {
            InitializeComponent();
            _userId = userId;
            _apiClient = apiClient;

            if (_userId.HasValue)
                _ = LoadUserAsync((int)_userId);

            Text = _userId.HasValue ? "Редактирование пользователя" : "Добавление пользователя";
        }

        #region Кнопки
        /// <summary>
        /// Кнопка для добавления нового пользователя или сохранения существующего
        /// </summary>
        public async void btnSave_Click(object sender, EventArgs e)
        {
            var username = txtUsername.Text.Trim();
            var fullName = txtFullName.Text.Trim();
            var password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(fullName) ||
                (!_userId.HasValue && string.IsNullOrWhiteSpace(password)))
            {
                MessageBox.Show("Пожалуйста, заполните все обязательные поля.");
                return;
            }

            try
            {
                BaseResult<UserDto> result;

                if (_userId.HasValue)
                {
                    var updateDto = new UserDto
                    {
                        Id = _userId.Value,
                        Login = username,
                        Name = fullName,
                        Password = string.IsNullOrWhiteSpace(password) ? null : password
                    };

                    result = await _apiClient.PutAsync<UserDto, BaseResult<UserDto>>("api/users", updateDto);
                }
                else
                {
                    var createDto = new UserDto
                    {
                        Login = username,
                        Name = fullName,
                        Password = password
                    };

                    result = await _apiClient.PostAsync<UserDto, BaseResult<UserDto>>("api/users", createDto);
                }

                if (result != null && result.IsSuccess)
                {
                    MessageBox.Show("Пользователь успешно сохранён.");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show($"Ошибка: {result?.ErrorMessage ?? "Неизвестная ошибка"}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
        #endregion

        #region Методы

        /// <summary>
        /// Загрузка данных пользователя по ID для редактирования
        /// </summary>
        private async Task LoadUserAsync(int userId)
        {
            try
            {
                var result = await _apiClient.GetAsync<BaseResult<UserDto>>($"api/users/id/{userId}");

                if (result == null)
                {
                    MessageBox.Show("Не удалось получить ответ от сервера.");
                    Close();
                    return;
                }

                if (!result.IsSuccess || result.Data == null)
                {
                    MessageBox.Show($"Ошибка загрузки пользователя: {result.ErrorMessage ?? "Неизвестная ошибка"}");
                    Close();
                    return;
                }

                var user = result.Data;
                txtUsername.Text = user.Login;
                txtFullName.Text = user.Name;

                txtPassword.Enabled = false;
            }
            catch (HttpRequestException httpEx)
            {
                MessageBox.Show("Ошибка подключения к серверу: " + httpEx.Message);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Неожиданная ошибка: " + ex.Message);
                Close();
            }
        }
        #endregion
    }
}
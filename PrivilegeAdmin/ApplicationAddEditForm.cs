using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace PrivilegeAdmin
{
    public partial class ApplicationAddEditForm : Form
    {
        private readonly int? _orderId;
        private readonly MyHttpClient _apiClient;

        public ApplicationAddEditForm(MyHttpClient apiClient, int? orderId = null)
        {
            InitializeComponent();
            _orderId = orderId;
            _apiClient = apiClient;

            Text = _orderId.HasValue ? "Редактирование заказа" : "Добавление заказа";
            buttonSave.Text = _orderId.HasValue ? "Сохранить" : "Создать";
            txtFilePath.Visible = !_orderId.HasValue;
            btnBrowse.Visible = !_orderId.HasValue;
            textBoxOrderId.Visible = _orderId.HasValue;
            textBoxFio.Visible = _orderId.HasValue;
            comboBoxStatus.Visible = _orderId.HasValue;
            labelOrderId.Visible = _orderId.HasValue;
            labelFio.Visible = _orderId.HasValue;
            labelStatus.Visible = _orderId.HasValue;
            Height = _orderId.HasValue ? 230 : 145;
            buttonSave.Location = new Point(150, 63 + (!_orderId.HasValue ? 0 : 85));

            _ = InitializeFormAsync();
        }

        private async Task InitializeFormAsync()
        {
            await LoadComboBoxesAsync();

            if (_orderId.HasValue)
                await LoadOrderAsync((int)_orderId);
        }

        private async Task LoadComboBoxesAsync()
        {
            var statusList = Enum.GetValues(typeof(StatusEnum))
                         .Cast<StatusEnum>()
                         .Select(status => new
                         {
                             Value = status,
                             Display = GetEnumDisplayName(status)
                         })
                         .ToList();

            comboBoxStatus.DisplayMember = "Display";
            comboBoxStatus.ValueMember = "Value";
            comboBoxStatus.DataSource = statusList;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите файл";
                openFileDialog.Filter = "XML файлы (*.xml)|*.xml|Все файлы (*.*)|*.*"; 

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = openFileDialog.FileName;
                }
            }
        }

        private async void buttonSave_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(textBoxOrderId.Text) ||
            //    string.IsNullOrWhiteSpace(textBoxFio.Text) ||
            //    comboBoxStatus.SelectedItem == null)
            //{
            //    MessageBox.Show("Заполните все поля.");
            //    return;
            //}

            var dto = new ApplicationDto
            {
                Status = (StatusEnum)comboBoxStatus.SelectedValue,
                DateEdit = DateTime.Now
            };

            BaseResult<ApplicationDto> result;

            if (_orderId.HasValue)
            {
                dto.Id = _orderId.Value;

                result = await _apiClient.PutAsync<ApplicationDto, BaseResult<ApplicationDto>>("api/applications", dto);
            }
            else
            {
                using var form = new MultipartFormDataContent();

                byte[] fileBytes = await System.IO.File.ReadAllBytesAsync(txtFilePath.Text);
                var fileContent = new ByteArrayContent(fileBytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/xml");

                form.Add(fileContent, "xmlfile", Path.GetFileName(txtFilePath.Text));

                result = await _apiClient.PostAsync<BaseResult<ApplicationDto>>("api/xml-listener", form);
            }

            if (result != null && result.IsSuccess)
            {
                MessageBox.Show(_orderId.HasValue ? "Заказ успешно обновлён." : "Заказ успешно создан.");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Ошибка при создании: " + result?.ErrorMessage ?? "Нет ответа от сервера.");
            }
        }

        /// <summary>
        /// Загрузка данных о заказе по ID
        /// </summary>
        private async Task LoadOrderAsync(int orderId)
        {
            try
            {
                var result = await _apiClient.GetAsync<BaseResult<ApplicationDto>>($"api/applications/id/{orderId}");

                if (result == null)
                {
                    MessageBox.Show("Не удалось получить ответ от сервера.");
                    Close();
                    return;
                }

                if (!result.IsSuccess || result.Data == null)
                {
                    MessageBox.Show($"Ошибка загрузки заказа: {result.ErrorMessage ?? "Неизвестная ошибка"}");
                    Close();
                    return;
                }

                var order = result.Data;

                textBoxOrderId.Text = order.Id.ToString();
                comboBoxStatus.SelectedValue = order.Status;
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

        public static string GetEnumDisplayName(Enum value)
        {
            return value.GetType()
                        .GetMember(value.ToString())
                        .FirstOrDefault()?
                        .GetCustomAttribute<DisplayAttribute>()?
                        .Name ?? value.ToString();
        }
    }
}

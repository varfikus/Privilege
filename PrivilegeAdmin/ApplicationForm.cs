using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Models;
using PrivilegeAPI.Result;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PrivilegeAdmin
{
    public partial class ApplicationForm : Form
    {
        private readonly MyHttpClient _apiClient;

        public ApplicationForm(MyHttpClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            _ = LoadOrders();
        }

        private async void button_add_Click(object sender, EventArgs e)
        {
            ApplicationAddEditForm form = new ApplicationAddEditForm(_apiClient);
            form.StartPosition = FormStartPosition.CenterParent;

            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadOrders();
            }
        }

        private async void button_edit_Click(object sender, EventArgs e)
        {
            if (dGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите заказ для редактирования.");
                return;
            }

            var selectedRow = dGV.SelectedRows[0];
            var orderId = (int)selectedRow.Cells["id"].Value;

            ApplicationAddEditForm form = new ApplicationAddEditForm(_apiClient, orderId);
            form.StartPosition = FormStartPosition.CenterParent;

            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadOrders();
            }
        }

        private async void button_del_Click(object sender, EventArgs e)
        {
            if (dGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
                return;
            }

            var confirmResult = MessageBox.Show( "Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes)
            {
                return;
            }

            var selectedRow = dGV.SelectedRows[0];

            var orderId = (int)selectedRow.Cells["id"].Value;

            var orderToDelete = new ApplicationDto
            {
                Id = orderId
            };

            try
            {
                var result = await _apiClient.DeleteAsync<ApplicationDto, BaseResult<ApplicationDto>>("api/applications", orderToDelete);

                if (result != null && result.IsSuccess)
                {
                    MessageBox.Show("Заказ успешно удалён.");
                    await LoadOrders();
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

        private void button_refresh_Click(object sender, EventArgs e)
        {
            _ = LoadOrders();
        }

        private async Task LoadOrders()
        {
            try
            {
                var result = await _apiClient.GetAsync<CollectionResult<ApplicationDto>>("api/applications");

                if (result != null && result.IsSuccess && result.Data != null)
                {
                    this.dGV.Rows.Clear();

                    foreach (var order in result.Data)
                    {
                        var statusEnum = (StatusEnum)Convert.ToInt32(order.Status);
                        var statusDisplayName = GetEnumDisplayName(statusEnum);

                        dGV.Rows.Add(
                            order.Id,
                            order.Name ?? "Не указано",
                            statusDisplayName,
                            order.DateAdd.Value.ToString("dd.MM.yyyy HH:mm:ss"),
                            order.DateEdit.Value.ToString("dd.MM.yyyy HH:mm:ss")
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

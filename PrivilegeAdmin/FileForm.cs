using PrivilegeAPI;
using PrivilegeAPI.Dto;
using PrivilegeAPI.Result;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrivilegeAdmin
{
    public partial class FileForm : Form
    {
        private readonly MyHttpClient _apiClient;
        public FileForm(MyHttpClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
        }

        private void FileForm_Load(object sender, EventArgs e)
        {
            _ = LoadFiles();
        }

        private async void button_add_Click(object sender, EventArgs e)
        {
            ApplicationAddEditForm form = new ApplicationAddEditForm(_apiClient);
            form.StartPosition = FormStartPosition.CenterParent;

            if (form.ShowDialog() == DialogResult.OK)
            {
                await LoadFiles();
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
                await LoadFiles();
            }
        }

        private async void button_del_Click(object sender, EventArgs e)
        {
            if (dGV.SelectedRows.Count == 0)
            {
                MessageBox.Show("Пожалуйста, выберите заказ для удаления.");
                return;
            }

            var confirmResult = MessageBox.Show("Вы уверены, что хотите удалить выбранный заказ?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

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
                var result = await _apiClient.DeleteAsync<ApplicationDto, BaseResult<ApplicationDto>>("api/order", orderToDelete);

                if (result != null && result.IsSuccess)
                {
                    MessageBox.Show("Заказ успешно удалён.");
                    await LoadFiles();
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
            _ = LoadFiles();
        }

        private async Task LoadFiles()
        {
            try
            {
                
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
    }
}

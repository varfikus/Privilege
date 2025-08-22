using FluentFTP;
using Microsoft.Extensions.Hosting;
using PrivilegeAPI.Models;
using System.Net;
using System.Text;

namespace PrivilegeAPI.Services
{
    public class FtpSettings
    {
        public string Server { get; set; }
        public int Port { get; set; } = 21;
        public string Username { get; set; }
        public string Password { get; set; }
        public bool UseTls { get; set; } = true;
    }

    public class FtpService
    {
        private readonly AsyncFtpClient _ftpClient;

        public FtpService(FtpSettings settings)
        {
            _ftpClient = new AsyncFtpClient
            {
                Host = settings.Server,
                Port = settings.Port,
                Credentials = new NetworkCredential(settings.Username, settings.Password),
            };

            _ftpClient.Config.EncryptionMode = settings.UseTls ? FtpEncryptionMode.Explicit : FtpEncryptionMode.None;
            _ftpClient.Config.ValidateAnyCertificate = settings.UseTls;
        }

        public async Task ConnectAsync()
        {
            if (!_ftpClient.IsConnected)
                await _ftpClient.Connect();
        }

        private async Task EnsureConnectedAsync()
        {
            if (!_ftpClient.IsConnected)
                await _ftpClient.Connect();
        }

        public async Task<bool> SaveFileAsync(string remotePath, string content)
        {
            try
            {
                await EnsureConnectedAsync();
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                var status = await _ftpClient.UploadBytes(bytes, remotePath, FtpRemoteExists.Overwrite, true);
                return status == FtpStatus.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении файла: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DownloadFileAsync(string remotePath, string localPath)
        {
            try
            {
                await EnsureConnectedAsync();
                var status = await _ftpClient.DownloadFile(localPath, remotePath, FtpLocalExists.Overwrite, FtpVerify.None);
                return status == FtpStatus.Success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при скачивании файла: " + ex.Message);
                return false;
            }
        }

        public async Task<Stream?> OpenReadAsync(string remotePath)
        {
            try
            {
                await EnsureConnectedAsync();

                if (!await _ftpClient.FileExists(remotePath))
                    return null;

                var stream = await _ftpClient.OpenRead(remotePath);
                return stream;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при открытии потока для чтения: " + ex.Message);
                return null;
            }
        }

        public async Task<string?> ReadFileAsync(string remotePath)
        {
            try
            {
                await EnsureConnectedAsync();
                if (!await _ftpClient.FileExists(remotePath))
                    return null;

                var data = await _ftpClient.DownloadBytes(remotePath, 0);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при чтении файла: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteFileAsync(string remotePath)
        {
            try
            {
                await EnsureConnectedAsync();
                if (!await _ftpClient.FileExists(remotePath))
                    return false;
                await _ftpClient.DeleteFile(remotePath);
                    return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении файла: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> FileExistsAsync(string remotePath)
        {
            try
            {
                await EnsureConnectedAsync();
                return await _ftpClient.FileExists(remotePath);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при проверке файла: " + ex.Message);
                return false;
            }
        }

        public async Task<List<string>> ListDirectoryAsync(string remotePath)
        {
            try
            {
                await EnsureConnectedAsync();
                var items = await _ftpClient.GetNameListing(remotePath);
                return new List<string>(items);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении списка каталога: " + ex.Message);
                return new List<string>();
            }
        }

        public async Task DisconnectAsync()
        {
            if (_ftpClient.IsConnected)
                await _ftpClient.Disconnect();
        }
    }
}

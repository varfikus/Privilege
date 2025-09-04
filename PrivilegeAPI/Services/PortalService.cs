using FluentFTP;
using PrivilegeAPI.Context;
using System.Net;
using System.Runtime;
using System.Text;

namespace PrivilegeAPI.Services
{
    public class PortalService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly FtpSettings _ftpSettings;

        public PortalService(IServiceScopeFactory scopeFactory, FtpSettings ftpSettings)
        {
            _scopeFactory = scopeFactory;
            _ftpSettings = ftpSettings;
        }

        public FtpService CreateFtpService(string server = null, int? port = null, string username = null, string password = null, bool useTls = false)
        {
            var settings = new FtpSettings
            {
                Server = server ?? _ftpSettings.Server,
                Port = port ?? _ftpSettings.Port,
                Username = username ?? _ftpSettings.Username,
                Password = password ?? _ftpSettings.Password,
                UseTls = useTls
            };

            return new FtpService(settings);
        }

        public async Task ConnectAsync()
        {
            var ftpService = CreateFtpService("192.168.69.38", 21, "gosuslugi_ishodiashie", "LcrF00MCjT", false);
            await ftpService.ConnectAsync();
        }

        public async Task DisconnectAsync()
        {
            var ftpService = CreateFtpService("192.168.69.38", 21, "gosuslugi_ishodiashie", "LcrF00MCjT", false);
            await ftpService.DisconnectAsync();
        }

        public async Task<bool> SaveFileAsync(string remotePath, string content)
        {
            var ftpService = CreateFtpService("192.168.69.38", 21, "gosuslugi_ishodiashie", "LcrF00MCjT", false);
            try
            {
                await ftpService.ConnectAsync();
                var result = await ftpService.SaveFileAsync(remotePath, content);
                await ftpService.DisconnectAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving file: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> DownloadFileAsync(string remotePath, string localPath)
        {
            var ftpService = CreateFtpService("192.168.69.38", 21, "gosuslugi_ishodiashie", "LcrF00MCjT", false);
            try
            {
                await ftpService.ConnectAsync();
                var result = await ftpService.DownloadFileAsync(remotePath, localPath);
                await ftpService.DisconnectAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error downloading file: " + ex.Message);
                return false;
            }
        }

        public async Task<List<string>> ListDirectoryAsync(string remotePath)
        {
            var ftpService = CreateFtpService("192.168.69.38", 21, "gosuslugi_ishodiashie", "LcrF00MCjT", false);
            try
            {
                await ftpService.ConnectAsync();
                var items = await ftpService.ListDirectoryAsync(remotePath);
                await ftpService.DisconnectAsync();
                return items;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error with getting catalog list: " + ex.Message);
                return new List<string>();
            }
        }

        public async Task<int> AddToDbAsync(Models.File file)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                try
                {
                    dbContext.Files.Add(file);
                    await dbContext.SaveChangesAsync();
                    return file.Id;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error adding file to database: " + ex.Message);
                    return 0;
                }
            }
        }
    }
}

using System.Net;
using System.Text;
using System.Xml.Linq;

namespace PrivilegeAPI.Services
{
    public class FtpService
    {
        private async Task SaveXmlToFtp(string xmlContent, int applicationId)
        {
            try
            {
                string ftpServer = "127.0.0.1";
                string ftpUser = "admin";
                string ftpPassword = "admin";
                int Port = 21;
                string ftpDirectory = "/Applications";
                string ftpPath = $"ftp://{ftpServer}{ftpDirectory}/{DateTime.Now:yyyyMMddHHmmss}_Application_{applicationId}.xml";

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpPath);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.UseBinary = true;
                request.KeepAlive = false;
                request.EnableSsl = false;

                byte[] xmlBytes = Encoding.UTF8.GetBytes(xmlContent);
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(xmlBytes, 0, xmlBytes.Length);
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save XML to FTP: {ex.Message}", ex);
            }
        }

        public static async Task<bool> SaveFileFtpAsync(XDocument xdoc, string ftpPath)
        {
            try
            {
                string ftpServer = "127.0.0.1";
                string ftpUser = "admin";
                string ftpPassword = "admin";
                int ftpPort = 21;

                ftpPath = ftpPath.Replace("\\", "/");
                if (!ftpPath.StartsWith("/"))
                    ftpPath = "/" + ftpPath;

                string fileName = Path.GetFileName(ftpPath);
                if (string.IsNullOrWhiteSpace(fileName))
                    throw new ArgumentException("FTP-путь не должен оканчиваться на слэш и должен содержать имя файла.");

                string fullUri = $"ftp://{ftpServer}:{ftpPort}{ftpPath}";

                byte[] fileContents;
                using (var memoryStream = new MemoryStream())
                {
                    xdoc.Save(memoryStream);
                    fileContents = memoryStream.ToArray();
                }

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(fullUri);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.UseBinary = true;
                request.KeepAlive = false;
                request.EnableSsl = false;

                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    await requestStream.WriteAsync(fileContents, 0, fileContents.Length);
                }

                using (var response = (FtpWebResponse)await request.GetResponseAsync())
                {
                    return response.StatusCode == FtpStatusCode.ClosingData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении XML на FTP: " + ex.Message);
                return false;
            }
        }
    }
}

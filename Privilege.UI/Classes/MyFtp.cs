using System.Net;
using FluentFTP;
using Privilege.UI.Classes.Json.Sub;

namespace Privilege.UI.Classes
{
    class MyFtp
    {
        /// <summary>
        /// Загрузка файла на FTP-сервер
        /// </summary>
        /// <param name="fileName">Путь к файлу на ПК</param>
        /// <param name="fileNameFtp">Путь к файлу на FTP-сервере</param>
        /// <returns></returns>
        public static bool UploadToFtp(string fileName, string fileNameFtp)
        {
            FtpClient client = new FtpClient(Connection.Ftp.Ip)
            {
                Credentials = new NetworkCredential(Connection.Ftp.User, Connection.Ftp.Password),
                Port = Connection.Ftp.Port
            };

            try
            {
                client.Connect();
                client.UploadFile(fileName, fileNameFtp, FtpExists.Overwrite, true);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }

        /// <summary>
        /// Загрузка файла с FTP-сервера
        /// </summary>
        /// <param name="fileName">Путь к файлу на ПК</param>
        /// <param name="fileNameFtp">Путь к файлу на FTP-сервере</param>
        /// <returns></returns>
        public static bool DownloadFromFtp(string fileName, string fileNameFtp)
        {
            FtpClient client = new FtpClient(Connection.Ftp.Ip)
            {
                Credentials = new NetworkCredential(Connection.Ftp.User, Connection.Ftp.Password),
                Port = Connection.Ftp.Port
            };

            try
            {
                client.Connect();
                client.DownloadFile(fileName, fileNameFtp);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }

        /// <summary>
        /// Проверить соединение к FTP
        /// </summary>
        /// <returns></returns>
        public static bool CheckConnection()
        {
            FtpClient client = new FtpClient(Connection.Ftp.Ip)
            {
                Credentials = new NetworkCredential(Connection.Ftp.User, Connection.Ftp.Password),
                Port = Connection.Ftp.Port
            };

            try
            {
                client.Connect();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }

        /// <summary>
        /// Проверить соединение к FTP
        /// </summary>
        /// <returns></returns>
        public static bool CheckConnection(JsonConnectionFtp param)
        {
            FtpClient client = new FtpClient(param.Ip)
            {
                Credentials = new NetworkCredential(param.User, param.Password),
                Port = param.Port
            };

            try
            {
                client.Connect();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (client.IsConnected)
                    client.Disconnect();
            }
        }
    }
}

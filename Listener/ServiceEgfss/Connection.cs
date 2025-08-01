using FluentFTP;
using ServiceMinsoc.Properties;

namespace ServiceMinsoc
{
    public static class Connection
    {
        /// <summary>
        /// Строка подключения.
        /// </summary>
        public static string Conn { get; private set; }

        /// <summary>
        /// Создать строку подключения.
        /// </summary>
        public static void Create()
        {
            Conn = string.Empty;
            if (Settings.Default.Db_Ip != "") Conn += "server=" + Settings.Default.Db_Ip + ";";
            if (Settings.Default.Db_UserName != "") Conn += " userid=" + Settings.Default.Db_UserName + ";";
            if (Settings.Default.Db_Password != "") Conn += " password=" + Settings.Default.Db_Password + ";";
            if (Settings.Default.Db_Name != "") Conn += " database=" + Settings.Default.Db_Name + ";";
            if (Settings.Default.Db_Port != "") Conn += " port=" + Settings.Default.Db_Port + ";";
            Conn += " charset=utf8;";
        }

        /// <summary>
        /// Проверка доступности базы данных.
        /// </summary>
        /// <returns>true - база данных доступна</returns>
        public static bool CheckConnection()
        {
            string query = "select 1";
            MySql.Data.MySqlClient.MySqlDataAdapter da = new MySql.Data.MySqlClient.MySqlDataAdapter(query, Conn);
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                da.Fill(dt);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверить соединение к FTP-серверу
        /// </summary>
        /// <returns>true - подключение установлено, false - ошибка при соединении</returns>
        public static bool CheckFtpConnection()
        {
            FtpClient client = new FtpClient(Settings.Default.Ftp_Url, Settings.Default.Ftp_User, Settings.Default.Ftp_Pass)
            { Port = Settings.Default.Ftp_Port };
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

using System;
using System.Security.Cryptography;
using System.Text;
using Privilege.UI.Classes.Json;
using Privilege.UI.Classes.Json.Sub;
using MySql.Data.MySqlClient;

namespace Privilege.UI.Classes
{
    static class Connection
    {
        /// <summary>
        /// Строка подключения
        /// </summary>
        public static string Conn { get; private set; }

        /// <summary>
        /// Настройки FTP
        /// </summary>
        public static JsonConnectionFtp Ftp { get; private set; }
        

        /// <summary>
        /// Создать строку подключения
        /// </summary>
        public static void CreateConnection()
        {
            Conn = string.Empty;

            SettingsFile sf = new SettingsFile();
            JsonSettings settings = sf.LoadSettings();

            if (settings.Conn.Ip != "") Conn += "server=" + settings.Conn.Ip + ";";
            if (settings.Conn.User != "") Conn += " userid=" + settings.Conn.User + ";";
            if (settings.Conn.Password != "") Conn += " password=" + settings.Conn.Password + ";";
            if (settings.Conn.Name != "") Conn += " database=" + settings.Conn.Name + ";";
            if (settings.Conn.Port != "") Conn += " port=" + settings.Conn.Port + ";";
            Conn += " charset=utf8;";

            Ftp = settings.ConnFtp;
        }

        /// <summary>
        /// Преобразование текста в MD5 формат.
        /// </summary>
        /// <param name="value">Входящий текст.</param>
        /// <returns></returns>
        public static string ComputeMd5(string value)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(value);
            byte[] hash = md5.ComputeHash(inputBytes);
            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// Проверка доступности базы данных.
        /// </summary>
        /// <returns>true - база данных доступна</returns>
        public static bool CheckConnection()
        {
            string query = "select 1";
            MySqlDataAdapter da = new MySqlDataAdapter(query, Conn);
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                da.Fill(dt);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Запись в таблицу логов
        /// </summary>
        /// <param name="documentId">ID документа.</param>
        /// <param name="text">Текст для записи.</param>
        /// <returns>Успешность записи (true/false)</returns>
        public static bool AddLogs(string documentId, string text)
        {
            MySqlConnection sqlConn = new MySqlConnection(Conn);
            sqlConn.Open();
            MySqlCommand cmd = sqlConn.CreateCommand();

            try
            {
                cmd.CommandText = "INSERT INTO logs " +
                                  "( user_id,  info,  document_id) " +
                                  "VALUES " +
                                  "(@user_id, @info, @document_id)";

                cmd.Parameters.AddWithValue("@user_id", "OperatorId");
                cmd.Parameters.AddWithValue("@info", text);
                cmd.Parameters.AddWithValue("@document_id", documentId);

                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                sqlConn.Close();
            }
        }
    }
}

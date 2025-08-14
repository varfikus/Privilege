using System.Runtime.Serialization;
using PrivilegeUI.Classes.Json.Sub;

namespace PrivilegeUI.Classes.Json
{
    [DataContract]
    public class JsonSettings
    {
        /// <summary>
        /// Параметры подключения к БД
        /// </summary>
        [DataMember(Name = "conn")]
        public JsonConnection Conn { get; set; }

        /// <summary>
        /// Параметры подключения к FTP
        /// </summary>
        [DataMember(Name = "conn_ftp", EmitDefaultValue = false)]
        public JsonConnectionFtp ConnFtp { get; set; }

        /// <summary>
        /// Настройки пользователя
        /// </summary>
        [DataMember(Name = "user", EmitDefaultValue = false)]
        public JsonUser User { get; set; }

        /// <summary>
        /// Период обновления таблицы
        /// </summary>
        [DataMember(Name = "table_refresh", EmitDefaultValue = false)]
        public int TableRefresh { get; set; }

        public JsonSettings()
        {
            Conn = new JsonConnection();
            ConnFtp = new JsonConnectionFtp();
            User = new JsonUser();
            TableRefresh = 1;
        }

        public JsonSettings(JsonConnection conn, JsonConnectionFtp connFtp, JsonUser user)
        {
            Conn = conn;
            ConnFtp = connFtp;
            User = user;
        }

        public JsonSettings(JsonConnection conn, JsonConnectionFtp connFtp, JsonUser user, int tableRefresh)
        {
            Conn = conn;
            ConnFtp = connFtp;
            User = user;
            TableRefresh = tableRefresh;
        }
    }
}

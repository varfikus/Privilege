using System.Runtime.Serialization;

namespace Privilege.UI.Classes.Json.Sub
{
    /// <summary>
    /// Параметры подключения к FTP
    /// </summary>
    [DataContract(Name = "conn_ftp")]
    public class JsonConnectionFtp
    {
        /// <summary>
        /// IP-адрес
        /// </summary>
        [DataMember(Name = "ip")]
        public string Ip { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        [DataMember(Name = "user")]
        public string User { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(Name = "password")]
        public string Password { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        [DataMember(Name = "port")]
        public int Port { get; set; }

        public JsonConnectionFtp()
        {
            Ip = "";
            User = "";
            Password = "";
            Port = 0;
        }

        public JsonConnectionFtp(string ip, string user, string password, int port)
        {
            Ip = ip;
            User = user;
            Password = password;
            Port = port;
        }
    }
}

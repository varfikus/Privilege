using System.Runtime.Serialization;

namespace Privilege.UI.Classes.Json.Sub
{
    /// <summary>
    /// Параметры подключения к БД
    /// </summary>
    [DataContract(Name = "connection")]
    public class JsonConnection
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
        /// Имя БД
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Порт
        /// </summary>
        [DataMember(Name = "port")]
        public string Port { get; set; }

        public JsonConnection()
        {
            Ip = "";
            User = "";
            Password = "";
            Name = "";
            Port = "";
        }

        public JsonConnection(string ip, string user, string password, string name, string port)
        {
            Ip = ip;
            User = user;
            Password = password;
            Name = name;
            Port = port;
        }
    }
}

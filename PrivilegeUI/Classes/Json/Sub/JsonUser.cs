using System.Runtime.Serialization;

namespace PrivilegeUI.Classes.Json.Sub
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    [DataContract(Name = "user")]
    public class JsonUser
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(Name = "login", EmitDefaultValue = false)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(Name = "password", EmitDefaultValue = false)]
        public string Password { get; set; }

        /// <summary>
        /// Сохранить логин и пароль
        /// </summary>
        [DataMember(Name = "save_pass")]
        public bool SavePass { get; set; }

        /// <summary>
        /// ФИО оператора
        /// </summary>
        [DataMember(Name = "fio", EmitDefaultValue = false)]
        public string Fio { get; set; }

        /// <summary>
        /// Телефон оператора
        /// </summary>
        [DataMember(Name = "tel", EmitDefaultValue = false)]
        public string Tel { get; set; }

        /// <summary>
        /// Выбранный сертификат
        /// </summary>
        [DataMember(Name = "sert")]
        public string Sert { get; set; }

        public JsonUser()
        {
            Login = "";
            Password = "";
            Fio = "";
            Tel = "";
            Sert = "";
        }

        public JsonUser(string login, string password, bool savePass, string fio, string tel, string sert)
        {
            Login = login;
            Password = password;
            SavePass = savePass;
            Fio = fio;
            Tel = tel;
            Sert = sert;
        }
    }
}

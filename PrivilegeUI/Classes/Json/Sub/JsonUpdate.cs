using System.Runtime.Serialization;

namespace Privilege.UI.Classes.Json.Sub
{
    /// <summary>
    /// Информация об обновлении
    /// </summary>
    [DataContract]
    class JsonUpdate
    {
        /// <summary>
        /// Версия приложения
        /// </summary>
        [DataMember(Name = "version")]
        public string Version { get; set; }

        /// <summary>
        /// Версия инсталлера приложения
        /// </summary>
        [DataMember(Name = "install", EmitDefaultValue = false)]
        public string Install { get; set; }

        /// <summary>
        /// Критичность обновления
        /// </summary>
        [DataMember(Name = "critical")]
        public bool Critical { get; set; }
    }
}

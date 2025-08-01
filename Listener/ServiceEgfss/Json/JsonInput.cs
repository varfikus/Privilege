using System.Runtime.Serialization;

namespace ServiceMinsoc.Json
{
    [DataContract]
    class JsonInput
    {
        /// <summary>
        /// Ссылка на файл
        /// </summary>
        [DataMember(Name = "xml")] public string Xml { get; set; }

        /// <summary>
        /// Ссылка на файл
        /// </summary>
        [DataMember(Name = "link")]
        public string Link { get; set; }

        /// <summary>
        /// ID услуги
        /// </summary>
        [DataMember(Name = "orderId")] public string OpderId { get; set; }
    }
}

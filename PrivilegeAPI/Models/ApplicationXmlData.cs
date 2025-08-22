namespace PrivilegeAPI.Models
{
    public class ApplicationXmlData
    {
        // Регистрационные данные
        public string RegistrationDate { get; set; }       // <reg><datareg>
        public string RegistrationNumber { get; set; }     // <reg><regnumber>
        public string VhodNumber { get; set; }            // <reg><vhodregnumber>
        public string VhodDate { get; set; }              // <reg><vhodregdate>
        public string GrifRestriction { get; set; }       // <grifrestriction>

        // Заголовок заявки
        public string Header { get; set; }                // <header>

        // Адресаты
        public List<Recipient> Recipients { get; set; } = new List<Recipient>();

        // Содержание заявки
        public string ContentText { get; set; }           // <content>

        // Приложения
        public List<string> Attachments { get; set; } = new List<string>(); // <applications><application>

        // Коды формы и организаций
        public string FormCode { get; set; }              // <kodformdoc>
        public string PortalCode { get; set; }            // <kodorg>
        public string SubPortalCode { get; set; }         // <kodorgsub>
    }
}

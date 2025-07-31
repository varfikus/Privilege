namespace Privilege.UI.Classes
{
    static class UserInfo
    {
        /// <summary>
        /// ID пользователя
        /// </summary>
        public static string Id { get; set; }

        /// <summary>
        /// Логин пользователяы
        /// </summary>
        public static string Login { get; set; }

        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public static string Name { get; set; }

        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public static string Tel { get; set; }

        /// <summary>
        /// Доступ к учреждениям
        /// </summary>
        public static string RuleOffice { get; set; }

        /// <summary>
        /// Доступ к услугам
        /// </summary>
        public static string RuleService { get; set; }

        /// <summary>
        /// Сертификат пользователя
        /// </summary>
        public static string Sert { get; set; }

        /// <summary>
        /// Время обновления главной таблицы
        /// </summary>
        public static int TableRefresh { get; set; }
    }
}

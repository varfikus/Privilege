using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace PrivilegeAPI
{
    /// <summary>
    /// Класс для временного хранения информации о сертификате
    /// </summary>
    public class MyCert
    {
        /// <summary>
        /// Полное имя владельца сертификата
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Сокращенное имя владельца сертификата
        /// </summary>
        public string ShortName { get; set; }
        /// <summary>
        /// Отпечаток сертификата
        /// </summary>
        public string Thumbprint { get; set; }

        private string rootThumbprint;

        /// <summary>
        /// Отпечаток корневого сертификата, которым подписанн данный сертификат
        /// </summary>
        public string GetRootThumbprint()
        {
            return rootThumbprint;
        }

        /// <summary>
        /// Отпечаток корневого сертификата, которым подписанн данный сертификат
        /// </summary>
        public void SetRootThumbprint(string value)
        {
            rootThumbprint = value;
        }

        /// <summary>
        /// Сертификат для открытия двойным кликом в основном окне
        /// </summary>
        public X509Certificate2 Cert { get; set; }
        /// <summary>
        /// Сертификат используемый для шифрования и расшифровки файла
        /// </summary>
        public X509Certificate2 CertForCrypt { get; set; }
        /// <summary>
        /// Флаг, указывающий на валидность сертификата
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Инизиализирует пустой экземпляр класса
        /// </summary>
        public MyCert()
        {
            Name = "";
            ShortName = "";
            Thumbprint = null;
            SetRootThumbprint(null);
            Cert = null;
            CertForCrypt = null;
            IsValid = false;
        }

        /// <summary>
        /// Инизиализирует экземпляр класса с оновными его свойствами
        /// </summary>
        /// <param name="name">Полное имя владельца сертификата</param>
        /// <param name="thumbprint">Отпечаток сертификата</param>
        public MyCert(string name, string thumbprint)
        {
            Name = name;
            Thumbprint = thumbprint;
        }

        /// <summary>
        /// Открывает сертификат в специальном окне просмотра сертификата операционной системы
        /// </summary>
        public void DisplayCert()
        {
            byte[] certBytes = Cert.GetRawCertData();
            string name = "XFileName50900X.cer";
            using (BinaryWriter writer = new BinaryWriter(File.Open(name, FileMode.OpenOrCreate)))
            {
                writer.Write(certBytes);
            }
            Process.Start(name);
        }

        public byte[] GetCert()
        {
            byte[] certBytes = Cert.GetRawCertData();
            return certBytes;
        }

        public override string ToString()
        {
            if (CertForCrypt == null)
                return Name;
            else
            {
                return CertForCrypt.GetName() + "; ";
            }
        }
    }
}

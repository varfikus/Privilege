using System;
using System.IO;
using System.Security.Cryptography;

namespace Privilege.UI.Classes.Encryption
{
    public static class CryptoAes1
    {
        /// <summary>
        /// Шифрование строки
        /// </summary>
        /// <param name="clearText">Текст</param>
        /// <returns></returns>
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "%_tRon)0";
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x65, 0x67, 0x66, 0x73, 0x73, 0x20, 0x5F, 0x63, 0x6C, 0x69, 0x65, 0x6E, 0x74 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        /// <summary>
        /// Расшифровать строку
        /// </summary>
        /// <param name="cipherText">Шифрованный текст</param>
        /// <returns></returns>
        public static string Decrypt(string cipherText)
        {
            if (!IsBase64String(cipherText))
                return "";
            string EncryptionKey = "%_tRon)0";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey,
                    new byte[] { 0x65, 0x67, 0x66, 0x73, 0x73, 0x20, 0x5F, 0x63, 0x6C, 0x69, 0x65, 0x6E, 0x74 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        try
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        catch
                        {
                            return "";
                        }
                    }
                    cipherText = System.Text.Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private static bool IsBase64String(string base64)
        {
            if (string.IsNullOrEmpty(base64) || base64.Length % 4 != 0
                || base64.Contains(" ") || base64.Contains("\t") || base64.Contains("\r") || base64.Contains("\n"))
                return false;

            try
            {
                Convert.FromBase64String(base64);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

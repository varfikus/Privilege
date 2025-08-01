using System;
using System.IO;
using System.Text;

namespace ServiceMinsoc
{
    public static class MyBase64
    {
        /// <summary>
        /// Конвертировать файл в base64 строку.
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public static string FileToBase64(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            byte[] bytes = File.ReadAllBytes(path);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Сохранить base64 строку в файл.
        /// </summary>
        /// <param name="base64Str">Строка формата base64</param>
        /// <param name="path">Путь для сохранения.</param>
        public static void SaveToFile(string base64Str, string path)
        {
            byte[] bytes = Convert.FromBase64String(base64Str);
            File.WriteAllBytes(path, bytes);
        }

        public static string Encode(string text)
        {
            if (text == null)
            {
                return null;
            }

            byte[] textAsBytes = Encoding.UTF8.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static string Decode(string encodedText)
        {
            if (encodedText == null)
            {
                return "";
            }

            try
            {
                byte[] textAsBytes = Convert.FromBase64String(encodedText);
                return Encoding.UTF8.GetString(textAsBytes);
            }
            catch
            {
                return "";
            }
        }

        public static string Encode(string text, Encoding encoding)
        {
            if (text == null)
            {
                return null;
            }

            byte[] textAsBytes = encoding.GetBytes(text);
            return Convert.ToBase64String(textAsBytes);
        }

        public static string Decode(string encodedText, Encoding encoding)
        {
            if (encodedText == null)
            {
                return null;
            }

            byte[] textAsBytes = Convert.FromBase64String(encodedText);
            return encoding.GetString(textAsBytes);
        }
    }
}

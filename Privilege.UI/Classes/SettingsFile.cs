using System.IO;
using Privilege.UI.Classes.Encryption;
using Privilege.UI.Classes.Json;

namespace Privilege.UI.Classes
{
    class SettingsFile
    {
        /// <summary>
        /// Путь к файлу
        /// </summary>
        private readonly string _path;

        public SettingsFile()
        {
            _path = @"settings.dat";
        }

        public SettingsFile(string path)
        {
            _path = path;
        }

        /// <summary>
        /// Загрузить настройки
        /// </summary>
        /// <returns></returns>
        public JsonSettings LoadSettings()
        {
            JsonSettings settings = new JsonSettings();
            if (!File.Exists(_path))
                return settings;

            string text = GetStringFromFile(_path);
            string decrypt = CryptoAes1.Decrypt(text);
            if (decrypt == "")
                return settings;
            settings = JsonWorker.DeserializSettings(decrypt);
            return settings;
        }

        /// <summary>
        /// Сохранить настройки в файл
        /// </summary>
        /// <param name="settings">Настройки</param>
        /// <returns></returns>
        public bool SaveSettings(JsonSettings settings)
        {
            if (settings == null)
                settings = new JsonSettings();

            string text = JsonWorker.SerializSettings(settings);
            string enc = CryptoAes1.Encrypt(text);
            return SaveStringToFile(enc, _path);
        }


        /// <summary>
        /// Загрузить текст из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Текст из файла</returns>
        public string GetStringFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            try
            {
                string text = File.ReadAllText(path);
                return text;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// Сохранить текст в файл
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="path">Пусть для сохранения</param>
        /// <returns>Успешность операции</returns>
        private bool SaveStringToFile(string text, string path)
        {
            try
            {
                StreamWriter file = new StreamWriter(path);
                file.Write(text);
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

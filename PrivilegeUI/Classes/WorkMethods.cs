using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MySql.Data.MySqlClient;

namespace Privilege.UI.Classes
{
    class WorkMethods
    {
        /// <summary>
        /// Преобразование текста в Stream.
        /// </summary>
        /// <param name="s">Текст</param>
        /// <returns></returns>
        public static Stream GenerateStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// Преобразование Stream в String
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GenerateStringFromStream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string text = reader.ReadToEnd();
            reader.Close();
            return text;
        }

        /// <summary>
        /// Загрузить файл в текст
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns>Текст из файла</returns>
        public static string GetStringFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";

            string text = File.ReadAllText(path);
            return text;
        }

        /// <summary>
        /// Вырезать сигнатуру из файла
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string GetSignaturesFromStream(Stream stream)
        {
            string doc = GenerateStringFromStream(stream);
            string startText = "<signaturesxml>";
            string endText = "</signaturesxml>";
            int startIndex = doc.IndexOf(startText) + startText.Length;
            int endIndex = doc.IndexOf(endText);

            if (startIndex == -1 || endIndex == -1)
                return "";

            string retText = doc.Substring(startIndex, endIndex - startIndex);

            return retText;
        }

        public static string GetFileParam()
        {
            if (!File.Exists("template\\param.xml"))
                return "";
            return GetStringFromFile("template\\param.xml");
        }

        /// <summary>
        /// Сохранить XML-документ по указанному пути
        /// </summary>
        /// <param name="xmlDocument">Документ</param>
        /// <param name="pathFile">Путь для сохранения</param>
        public static bool SaveFileXml(XmlDocument xmlDocument, string pathFile)
        {
            try
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(pathFile, new UTF8Encoding(false));
                xmlDocument.WriteTo(xmlTextWriter);
                xmlTextWriter.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Проверка статуса документа
        /// </summary>
        /// <returns></returns>
        public static bool CheckStatus(int id, string[] currentStatus)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "SELECT status " +
                              "FROM documents " +
                              "WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", id);

            try
            {
                string mes = cmd.ExecuteScalar().ToString();
                if (currentStatus.Contains(mes))
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("[" + id + "] " + "Ошибка при проверке статуса главной таблицы: " + ex.Message);
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// Открыть файл
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        /// <returns></returns>
        public static bool OpenFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return false;
            }
            string crgss = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "GUP CIT\\CryptoGSS\\CryptoGSS.exe");
            if (File.Exists(crgss))
            {
                Process.Start(crgss, fileName);
            }
            else
                Process.Start(fileName);

            return true;
        }

        /// <summary>
        /// Проверить наличие временной папки, и если нет - создать
        /// </summary>
        public static void CheckTempDirectory()
        {
            if (!Directory.Exists("temp"))
                Directory.CreateDirectory("temp");
        }

        /// <summary>
        /// Удаление файла по ID
        /// </summary>
        /// <param name="fileId">ID файла</param>
        /// <param name="docId">ID документа</param>
        private void DelFileSql(int fileId, int docId = 0)
        {
            MySqlConnection conn = new MySqlConnection(Connection.Conn);
            conn.Open();
            MySqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = "DELETE FROM files " +
                              "WHERE id = @id";
            cmd.Parameters.AddWithValue("@id", fileId);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Logger.Log.Warn("[" + docId + "] " + "Ошибка удаления файла из базы данных: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

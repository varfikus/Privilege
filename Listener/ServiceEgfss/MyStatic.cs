using System.IO;

namespace ServiceMinsoc
{
    public static class MyStatic
    {
        /// <summary>
        /// Наименование программы.
        /// </summary>
        public static string ProgramName = "ServiceBTS";

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
        /// Загрузить Stream из файла
        /// </summary>
        /// <param name="path">Путь к файлу</param>
        /// <returns></returns>
        public static Stream GetStreamFromFile(string path)
        {
            if (string.IsNullOrEmpty(path))
                return null;

            byte[] bytes = File.ReadAllBytes(path);
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// Сохранить поток в файл
        /// </summary>
        /// <param name="stream">Поток</param>
        /// <param name="path">Путь для сохранения (с именем файла)</param>
        public static void SaveStreamToFile(Stream stream, string path)
        {
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                stream.CopyTo(fileStream);
            }
        }

        /// <summary>
        /// Вырезать сигнатуру из файла
        /// </summary>
        /// <param name="stream">Поток</param>
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

        /// <summary>
        /// Сохранить текст в XML-файл
        /// </summary>
        /// <param name="text">Текст</param>
        /// <param name="path">Путь сохранения</param>
        /// <returns></returns>
        public static bool SaveStringToFile(string text, string path)
        {
            using (StreamWriter sw = new StreamWriter(path))
            {
                try
                {
                    sw.WriteLine(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}

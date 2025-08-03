using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Privilege.UI.Classes.Json.Sub;

namespace Privilege.UI.Classes.Json
{
    class JsonWorker
    {
        #region Settings

        public static JsonSettings DeserializSettings(string text)
        {
            try
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(text)))
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(JsonSettings));
                    JsonSettings jsonStr = (JsonSettings)jsonFormatter.ReadObject(ms);
                    return jsonStr;
                }
            }
            catch
            {
                return null;
            }
        }

        public static string SerializSettings(JsonSettings settings)
        {
            try
            {
                DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(JsonSettings));
                MemoryStream memoryStream = new MemoryStream();
                jsonFormatter.WriteObject(memoryStream, settings);
                memoryStream.Position = 0;
                StreamReader streamReader = new StreamReader(memoryStream);

                string json = streamReader.ReadToEnd();

                streamReader.Close();
                memoryStream.Close();

                return json;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion


        #region Update

        /// <summary>
        /// Преобразование строки для проверки новой версии программы
        /// </summary>
        /// <param name="input">Строка</param>
        /// <returns></returns>
        public static JsonUpdate DeserializVersion(string input)
        {
            try
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(input)))
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(JsonUpdate));
                    JsonUpdate jsonStr = (JsonUpdate)jsonFormatter.ReadObject(ms);
                    return jsonStr;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}

using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace ServiceMinsoc.Json
{
    class JsonWorker
    {
        /// <summary>
        /// Преобразование строки в JSON-класс версий
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static JsonInput Deserializ(string request)
        {
            try
            {
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(request)))
                {
                    DataContractJsonSerializer jsonFormatter = new DataContractJsonSerializer(typeof(JsonInput));
                    JsonInput jsonStr = (JsonInput)jsonFormatter.ReadObject(ms);
                    return jsonStr;
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Ошибка при преобразовании входящей в json: " + ex.Message);
                return null;
            }
        }
    }
}

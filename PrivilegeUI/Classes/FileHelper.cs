using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivilegeUI.Classes
{
    public static class FileHelper
    {
        /// <summary>
        /// Удаляет самый старый файл в папке, если общее количество файлов превышает maxFiles.
        /// </summary>
        /// <param name="directory">Папка, которую нужно проверить.</param>
        /// <param name="maxFiles">Максимальное количество файлов, по умолчанию 10.</param>
        public static void EnsureFileLimit(string directory, int maxFiles = 10)
        {
            if (!Directory.Exists(directory))
                return;

            var files = new DirectoryInfo(directory)
                .GetFiles()
                .OrderBy(f => f.CreationTimeUtc)
                .ToList();

            while (files.Count > maxFiles)
            {
                try
                {
                    files.First().Delete();
                    files.RemoveAt(0);
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Проверяет и ограничивает число файлов, а затем возвращает полный путь для нового файла.
        /// </summary>
        /// <param name="fileName">Имя нового файла.</param>
        /// <returns>Полный путь к файлу.</returns>
        public static string PrepareTempFilePath(string fileName)
        {
            string tempDir = Path.GetTempPath();
            EnsureFileLimit(tempDir, 10);
            return Path.Combine(tempDir, fileName);
        }
    }
}

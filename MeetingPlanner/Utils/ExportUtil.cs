using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MeetingPlanner.Model;

namespace MeetingPlanner.Utils
{
    public class ExportUtil
    {
        private static string _path;
        
        /// <summary>
        /// Экспорт коллекции встреч в текстовый файл
        /// </summary>
        /// <param name="models">Коллекция встреч</param>
        /// <returns>Путь до текстового файла</returns>
        public static string ExportInTxt(List<MeetingModel> models)
        {
            InitExportDirectory();
            var text = Serialize(models);

            var path = Path.Combine(_path, DefineFileName(models));
            
            File.WriteAllText(path, text);
            
            return path;
        }

        /// <summary>
        /// Сериализация коллекции в строку
        /// </summary>
        /// <param name="models">Коллекция встреч</param>
        /// <returns>Серилизованная строка</returns>
        public static string Serialize(List<MeetingModel> models)
        {
            var strBuilder = new StringBuilder();
            var iter = 1;
            foreach (var model in models)
            {
                strBuilder.Append($"-------------\n" +
                                  $"Номер - {iter++}\n" +
                                  $"Название - {model.Name}\n" +
                                  $"Начало - {model.StartDate.ToString("g")}\n" +
                                  $"Конец - {model.EndDate.ToString("g")}\n" +
                                  $"Напоминание - {model.NotifyTime?.ToString("g")}\n" +
                                  $"-------------\n\n");
            }

            return strBuilder.ToString();
        }
        
        /// <summary>
        /// Сериализация модели встречи в строку
        /// </summary>
        /// <param name="models">Модель встречи</param>
        /// <returns>Серилизованная строка</returns>
        public static string Serialize(MeetingModel model)
        {
            var strBuilder = new StringBuilder();
            
            strBuilder.Append($"-------------\n" +
                                $"Название - {model.Name}\n" +
                                $"Начало - {model.StartDate.ToString("g")}\n" +
                                $"Конец - {model.EndDate.ToString("g")}\n" +
                                $"Напоминание - {model.NotifyTime?.ToString("g")}\n" +
                                $"-------------\n\n");
            
            return strBuilder.ToString();
        }
        
        private static string DefineFileName(List<MeetingModel> models)
        {
            return $"{models.First().StartDate.ToString("d")}.txt";
        }
        
        private static void InitExportDirectory()
        {
            if(_path == null) 
                _path = "Export";
            
            if (!Directory.Exists(_path))
                Directory.CreateDirectory(_path);
        }
    }
}
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MeetingPlanner.Model;

namespace MeetingPlanner.Utils
{
    public static class ExportUtil
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
            var text = Serializer.Serialize(models);
            var path = Path.Combine(_path, DefineFileName(models));
            File.WriteAllText(path, text);
            
            return path;
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
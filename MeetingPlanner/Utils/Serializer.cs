using System.Collections.Generic;
using System.Text;
using MeetingPlanner.Model;

namespace MeetingPlanner.Utils
{
    /// <summary>
    /// Сериализация
    /// </summary>
    public static class Serializer
    {
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
        /// <param name="model">Модель встречи</param>
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
    }
}
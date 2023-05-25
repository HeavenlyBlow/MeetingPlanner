using System;
using System.ComponentModel;
using System.Globalization;

namespace MeetingPlanner.Utils
{
    public static class Converter
    {
        
        /// <summary>
        /// Конвертирует строку в нужный тип
        /// </summary>
        /// <param name="input">Обрабатываемая строка</param>
        /// <param name="isSuccess">Обработка успешна?</param>
        /// <typeparam name="T">Тип</typeparam>
        /// <returns>Возвращаемый объект нужного типа</returns>
        public static T Convert<T>(string input, out bool isSuccess)
        {
            isSuccess = false;
            try
            {
                if (typeof(T) == typeof(DateTime))
                {
                    isSuccess = DateTime.TryParseExact(input, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out DateTime answer);
                    
                    if(isSuccess)
                        return (T)System.Convert.ChangeType(answer, typeof(T));
                    
                    isSuccess = DateTime.TryParseExact(input, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out answer);

                    return (T)System.Convert.ChangeType(answer, typeof(T));
                }

                var converter = TypeDescriptor.GetConverter(typeof(T));
                
                if(converter != null)
                {
                    var answer = (T)converter.ConvertFromString(input);
                    isSuccess = true;
                    
                    return answer;
                }
                
                return default(T);
            }
            catch (FormatException)
            {
                return default(T);
            }
        }
    }
}
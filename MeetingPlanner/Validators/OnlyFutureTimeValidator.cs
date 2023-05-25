using System;

namespace MeetingPlanner.Validators
{
    /// <summary>
    /// Валидатор проверяющий находится ли значение не в прошлом
    /// </summary>
    public class OnlyFutureTimeValidator : IValidator<DateTime>
    {
        ///<inheritdoc/>
        public string ErrorMessage { get; } = "Время встречи не может быть в прошлом";
        
        ///<inheritdoc/>
        public bool Validate(DateTime time)
        {
            return time > DateTime.Now;
        }
        
    }
}
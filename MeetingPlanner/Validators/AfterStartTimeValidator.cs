using System;

namespace MeetingPlanner.Validators
{
    /// <summary>
    /// Валидатор проверяющий что заданное время больше чем время начала встречи
    /// </summary>
    public class AfterStartTimeValidator : IValidator<DateTime>
    {
        private readonly DateTime _startTime;
        
        ///<inheritdoc/>
        public string ErrorMessage { get; } = "Время конца встречи задано ранее времени начала";
        
        public AfterStartTimeValidator(DateTime startTime)
        {
            _startTime = startTime;
        }
        
        ///<inheritdoc/>
        public bool Validate(DateTime value)
        {
            return value > _startTime;
        }
    }
}
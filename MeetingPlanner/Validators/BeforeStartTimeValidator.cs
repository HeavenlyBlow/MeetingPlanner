using System;

namespace MeetingPlanner.Validators
{
    /// <summary>
    /// Валидатор проверяющий что заданное время находится до старта встречи
    /// </summary>
    public class BeforeStartTimeValidator : IValidator<DateTime>
    {
        private readonly DateTime _startTime;
        
        ///<inheritdoc/>
        public string ErrorMessage { get; } = "Заданно время после начала события";
        
        public BeforeStartTimeValidator(DateTime startTime)
        {
            _startTime = startTime;
        }
        
        ///<inheritdoc/>
        public bool Validate(DateTime value)
        {
            return value < _startTime;
        }
    }

}
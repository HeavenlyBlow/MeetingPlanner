using System;
using System.Collections.Generic;
using System.Linq;
using MeetingPlanner.Model;

namespace MeetingPlanner.Validators
{
    /// <summary>
    /// Валидатор проверяющий что заданная встреча не пересекается с другими
    /// </summary>
    public class FreeTimeValidator : IValidator<DateTime>
    {
        ///<inheritdoc/>
        public string ErrorMessage { get; } = "На это время назначена другая встреча";
        private List<MeetingModel> _models;
        private DateTime? _otherDate;
        public FreeTimeValidator(List<MeetingModel> models, DateTime? otherDate = null)
        {
            _models = models;
            _otherDate = otherDate;
        }
        
        ///<inheritdoc/>
        public bool Validate(DateTime value)
        {
            if (!_models.Any())
                return true;

            _otherDate = _otherDate ?? value;
            var startDate = _otherDate > value ? value : _otherDate;
            var endDate = _otherDate < value ? value : _otherDate;
            
            foreach (var model in _models)
            {
                if ((endDate > model.StartDate && startDate < model.StartDate)
                    || (startDate < model.EndDate && endDate > model.EndDate)
                    || (startDate > model.StartDate && endDate < model.EndDate)
                    || (startDate < model.StartDate && endDate > model.EndDate))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
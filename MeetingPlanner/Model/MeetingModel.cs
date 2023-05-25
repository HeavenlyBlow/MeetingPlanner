using System;

namespace MeetingPlanner.Model
{
    /// <summary>
    /// Модель встречи
    /// </summary>
    public class MeetingModel
    {
        public Guid Id { get;}
        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public bool IsNeedNotify { get; private set; }
        public DateTime? NotifyTime { get; private set; }
        public bool IsNeedNotifyNow => NotifyTime != null
                                       && !_isNotified
                                       && DateTime.Now > NotifyTime;

        private bool _isNotified = false;
        
        public MeetingModel(string name, DateTime startDate,DateTime endDate,  bool isNeedNotify = false, 
            DateTime? notifyTime = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            StartDate = startDate;
            IsNeedNotify = isNeedNotify;
            NotifyTime = notifyTime;
            EndDate = endDate;
        }

        public void SetIsNotified()
        {
            _isNotified = true;
        }
        public void ChangeName(string name)
        {
            Name = name;
        }
        
        public void ChangeStartDate(DateTime dateTime)
        {
            StartDate = dateTime;
        }
        
        public void ChangeEndDate(DateTime dateTime)
        {
            EndDate = dateTime;
        }
        
        public void ChangeIsNeedNotify()
        {
            IsNeedNotify = !IsNeedNotify;
            
            if(!IsNeedNotify)
                ChangeNotifyTime(null);
        }
        
        public void ChangeNotifyTime(DateTime? dateTime)
        {
            NotifyTime = dateTime;

            if (NotifyTime != null)
                _isNotified = false;
        }
    }
}
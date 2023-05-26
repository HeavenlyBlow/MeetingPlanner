using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MeetingPlanner.Model;
using MeetingPlanner.Utils;

namespace MeetingPlanner
{
    /// <summary>
    /// Планирощик встреч
    /// </summary>
    public sealed class Planner : IDisposable
    {
        private readonly List<MeetingModel> _meetings = new List<MeetingModel>();
        private Timer _timer;
        public Planner()
        {
            InitTimer();
        }

        #region Event
        
        public delegate void ReminderHandler(MeetingModel model);
        public event ReminderHandler Remind;

        private void OnRemind(MeetingModel model)
        {
            model.SetIsNotified();
            Remind?.Invoke(model);
        }
        #endregion

        #region NotifyTimer

        private void InitTimer()
        {
            _timer = new Timer(CheckReminderTime, null, 0, 10000);
        }
        private void CheckReminderTime(object obj)
        {
            _meetings
                .Where(meeting => meeting.IsNeedNotify && meeting.IsNeedNotifyNow)
                .ToList()
                .ForEach(OnRemind);
        }

        #endregion

        #region CRUD

        /// <summary>
        /// Добвить встречу
        /// </summary>
        /// <param name="meeting">Модель встречи</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddMeeting(MeetingModel meeting)
        {
            if (meeting is null)
                throw new ArgumentNullException(nameof(meeting),"Переданная встреча ровняется null");
            
            _meetings.Add(meeting);
        }

        /// <summary>
        /// Удалить встречу
        /// </summary>
        /// <param name="id">Идентификатор встречи</param>
        public void DeleteMeeting(Guid id)
        {
            var model = FindMeeting(id);
            _meetings.Remove(model);
        }
        
        /// <summary>
        /// Получить отсортированный списк встреч по дате
        /// </summary>
        /// <returns>Лист встреч</returns>
        public List<MeetingModel> GetMeetings() => _meetings.OrderBy(model => model.StartDate).ToList();
        
        /// <summary>
        /// Экспортировать всречу
        /// </summary>
        /// <param name="meetingDateTime">Дата эспортируемых встреч</param>
        /// <returns>Путь до экспортируемого файла</returns>
        public string ExportMeeting(DateTime meetingDateTime)
        {
            return ExportUtil.ExportInTxt(FindMeetings(meetingDateTime));
        }
        
        private MeetingModel FindMeeting(Guid id)
        {
            return _meetings.Single(model => model.Id.Equals(id));
        }
        
        private List<MeetingModel> FindMeetings(DateTime meetingDateTime)
        {
            return _meetings.FindAll(c => c.StartDate.Date == meetingDateTime.Date);
        }
        
        #endregion

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
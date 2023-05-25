using System;
using System.Collections.Generic;
using System.Linq;
using MeetingPlanner.Model;
using MeetingPlanner.Utils;
using MeetingPlanner.Validators;

namespace MeetingPlanner
{
    /// <summary>
    /// Ядро программы
    /// </summary>
    public class Core
    {
        private readonly Planner _planner = new Planner();
        public Core()
        {
            _planner.Remind += PlannerOnRemind;
            ShowMenu();
        }
        
        #region EventHandler

        private void PlannerOnRemind(MeetingModel model)
        {
            var message = ExportUtil.Serialize(model);
            Console.WriteLine($"\nНапоминание о встрече:\n{message}\n");
        }


        #endregion
        
        #region Create
        private void CreateMeeting()
        {
            ClearConsole();
            var isSuccess = false;
            Console.WriteLine("Создание встречи\n");
            var meetings = _planner.GetMeetings();
            var name = TryGetInput<string>("Введите название встречи", out isSuccess);

            IValidator<DateTime> freeTimeValidator = new FreeTimeValidator(meetings);
            IValidator<DateTime> onlyFutureTimeValidator = new OnlyFutureTimeValidator();
            
            if(!isSuccess) 
                return;
            
            var startDate = TryGetInput<DateTime>("Введите дату и время начала в формате dd.MM.yyyy HH:mm",
                out isSuccess, validators: new []{onlyFutureTimeValidator, freeTimeValidator});
            
            if(!isSuccess)
                return;
            
            IValidator<DateTime> afterStartTimeValidator = new AfterStartTimeValidator(startDate);
            freeTimeValidator = new FreeTimeValidator(meetings, startDate);
            
            var endDate = TryGetInput<DateTime>("Введите дату и время окончания в формате dd.MM.yyyy HH:mm",
                out isSuccess, validators: new []{onlyFutureTimeValidator, freeTimeValidator, afterStartTimeValidator});

            if (!isSuccess)
                return;

            var isNeedNotify = TryGetInput<bool>("Создать напоминания?\n" +
                                                 "Введите true или false\n", out isSuccess);
            
            if(!isSuccess)
                return;

            DateTime? notifyTime = null;
            
            if (isNeedNotify)
            {
                IValidator<DateTime> beforeStartTimeValidator = new BeforeStartTimeValidator(startDate);

                notifyTime = TryGetInput<DateTime>("Введите дату и время напоминания в формате dd.MM.yyyy HH:mm",
                    out isSuccess, validators: new []{onlyFutureTimeValidator, beforeStartTimeValidator});
                
                if(!isSuccess)
                    return;
            }
            
            _planner.AddMeeting(new MeetingModel(name, startDate, endDate,isNeedNotify, notifyTime));
            
        }
        #endregion
        
        #region Update
        private void UpdateMeeting()
        {
            var models = _planner.GetMeetings();
            var meetingsStr = ExportUtil.Serialize(models);
            var isSuccess = false;
            var message = "Список встреч \n" + $"{meetingsStr}\n Введите номер встречи, которую хотите изменить";
            var meetingNumber = TryGetInput<ushort>(message, out isSuccess);
            
            if(!isSuccess)
                return;

            var model = models[meetingNumber - 1];
            
            message = "Что вы хотите изменить?\n" +
                      "1. Имя\n" +
                      "2. Дату начала\n" +
                      "3. Дату конца\n" +
                      (model.IsNeedNotify ? "4. Выключить напоминание\n5. Изменить время напоминания\n" : "4. Включить напоминание\n") +
                      "0. Выход\n";

            var propertyNumber = TryGetInput<ushort>(message, out isSuccess);
            
            if(!isSuccess)
                return;

            switch (propertyNumber)
            {
                case 1:
                    UpdateMeetingName(model);
                    break;
                case 2:
                    UpdateMeetingStartDate(model);
                    break;
                case 3:
                    UpdateMeetingEndDate(model);
                    break;
                case 4:
                    UpdateMeetingNotifying(model);
                    break;
                case 5 when model.IsNeedNotify:
                    UpdateMeetingNotifyTime(model);
                    break;
            }
            

        }
        private void UpdateMeetingName(MeetingModel model)
        {
            var name = TryGetInput<string>("Введите название встречи", out bool isSuccess);
            
            if(!isSuccess)
                return;

            model.ChangeName(name);
        }
        private void UpdateMeetingStartDate(MeetingModel model)
        {
            IValidator<DateTime> freeTimeValidator = new FreeTimeValidator(_planner.GetMeetings());
            IValidator<DateTime> onlyFutureTimeValidator = new OnlyFutureTimeValidator();
            
            var startDate = TryGetInput<DateTime>("Введите дату и время начала в формате dd.MM.yyyy HH:mm\n " +
                                                  "При изменнии даты начала не забудьте изменить дату конца встречи!",
                out bool isSuccess, validators: new []{onlyFutureTimeValidator, freeTimeValidator});
            
            if(!isSuccess)
                return;

            model.ChangeStartDate(startDate);
        }
        private void UpdateMeetingEndDate(MeetingModel model)
        {
            IValidator<DateTime> freeTimeValidator = new FreeTimeValidator(_planner.GetMeetings(), model.StartDate);
            IValidator<DateTime> onlyFutureTimeValidator = new OnlyFutureTimeValidator();
            IValidator<DateTime> afterStartTimeValidator = new AfterStartTimeValidator(model.StartDate);
            
            var endDate = TryGetInput<DateTime>("Введите дату и время окончания в формате dd.MM.yyyy HH:mm",
                out bool isSuccess, validators: new []{onlyFutureTimeValidator, freeTimeValidator, afterStartTimeValidator});

            if (!isSuccess)
                return;
            
            model.ChangeEndDate(endDate);
        }
        private void UpdateMeetingNotifying(MeetingModel model)
        {
            var isNeedChangeNotifying = TryGetInput<bool>((model.IsNeedNotify ? "Отключить напоминание" : "Включить напоминание" ) +
                                                 "Введите true или false\n" +
                                                 (!model.IsNeedNotify ? "Не забудьте задать время напоминания\n" : "" ), out var isSuccess);
            
            if(!isSuccess)
                return;
            
            if(isNeedChangeNotifying)
                model.ChangeIsNeedNotify();
        }
        private void UpdateMeetingNotifyTime(MeetingModel model)
        {
            IValidator<DateTime> onlyFutureTimeValidator = new OnlyFutureTimeValidator();
            IValidator<DateTime> beforeStartTimeValidator = new BeforeStartTimeValidator(model.StartDate);

           var notifyTime = TryGetInput<DateTime>("Введите дату и время напоминания в формате dd.MM.yyyy HH:mm",
                out bool isSuccess, validators: new []{onlyFutureTimeValidator, beforeStartTimeValidator});
                
            if(!isSuccess)
                return;
            
            model.ChangeNotifyTime(notifyTime);
        }

        #endregion

        #region Delete
        private void DeleteMeeting()
        {
            var models = _planner.GetMeetings();
            var meetingsStr = ExportUtil.Serialize(models);
            var isSuccess = false;
            var message = "Список встреч \n" + $"{meetingsStr}\n Введите номер встречи, которую хотите удалить";
            var meetingNumber = TryGetInput<ushort>(message, out isSuccess);
            
            if(!isSuccess)
                return;

            var model = models[meetingNumber - 1];

            _planner.DeleteMeeting(model.Id);
        }

        #endregion

        #region Export

        private void ExportMeeting()
        {
            ClearConsole();
            
            var dateTime = TryGetInput<DateTime>("Введите дату для экспорта встреч в формате dd.MM.yyyy",
                out bool isSuccess);
            
            if(!isSuccess)
                return;

            ClearConsole();
            var path = _planner.ExportMeeting(dateTime);
            Console.WriteLine($"Файл экпортирован в дерикторию: {path}\nДля возврата в главное меню нажмите любую кнопку");
            Console.ReadKey();
        }

        #endregion

        #region ConsoleManage

        private void ShowMenu()
        {
            ClearConsole();
            var models = _planner.GetMeetings();
            var meetingsStr = ExportUtil.Serialize(models);
            var anyModels = models.Any();
            var message = "Список встреч \n" +
                          $"{meetingsStr}\n" +
                          "1. Создать встречу\n" +
                          (anyModels ? "2. Изменить встречу\n" : "") +
                          (anyModels ? "3. Удалить встречу\n" : "") +
                          (anyModels ? "4. Экспортировать встречи" : "") +
                          "\n Введите цифру пункта меню";
            
            var digit = TryGetInput<ushort>(message, out var isSuccess);
            
            if(!isSuccess) 
                return;

            switch (digit)
            {
                case 1: CreateMeeting();
                    break;
                
                case 2: UpdateMeeting();
                    break;
                
                case 3: DeleteMeeting();
                    break;
                
                case 4 when (models.Any()):
                    ExportMeeting();
                    break;
            }
            
            ShowMenu();

        }
        private void ClearConsole()
        {
            Console.Clear();
        }
        private void IncorrectInput(string message = null)
        {
            ClearConsole();
            if (message != null)
            {
                Console.WriteLine($"{message}\n");
            }
            Console.WriteLine("Ошибка ввода\n Для возврата нажмите любую клавишу...");
            Console.ReadKey();
            ShowMenu();
        }
        
        private T TryGetInput<T>(string message, out bool isSuccess, 
            bool isRequired = true, IEnumerable<IValidator> validators = null)
        {
            ClearConsole();
            Console.WriteLine(message);
            var input = Console.ReadLine()?.Trim();
            isSuccess = false;
            
            if (string.IsNullOrEmpty(input))
            {
                if (isRequired)
                    IncorrectInput();
                
                return default(T);
            }
            
            var converted = Converter.Convert<T>(input, out isSuccess);

            if (!isSuccess)
                IncorrectInput();

            if (validators != null)
            {
                var errors = validators
                    .Cast<IValidator<T>>()
                    .Where(validator => !validator.Validate(converted))
                    .Select(validator => validator.ErrorMessage)
                    .ToList();

                if (!errors.Any()) 
                    return converted;
                
                var strErrors = string.Join(", ", errors);
                isSuccess = false;
                IncorrectInput(strErrors);
            }
            
            return converted;
        }

        #endregion
        
        
    }
}
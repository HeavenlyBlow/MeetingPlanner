namespace MeetingPlanner.Validators
{
    /// <summary>
    /// Интерфейс валидатора
    /// </summary>
    /// <typeparam name="T">Валидируемый тип</typeparam>
    public interface IValidator<T> : IValidator
    {
        /// <summary>
        /// Сообщение об ошибке при непрохождении валидации
        /// </summary>
        public string ErrorMessage { get; }
        /// <summary>
        /// Метод проверки
        /// </summary>
        /// <param name="value">Проверяемое значение</param>
        /// <returns>Результат провери</returns>
        public bool Validate(T value);
    }

    public interface IValidator { }
}
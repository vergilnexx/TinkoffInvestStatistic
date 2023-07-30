namespace TinkoffInvest.Contracts.Enums
{
    /// <summary>
    /// Тип финансового инструмента.
    /// </summary>
    public enum InstrumentType
    {
        /// <summary>
        /// Неопределенно
        /// </summary>
        None = 0,

        /// <summary>
        /// Акция
        /// </summary>
        Share = 1,

        /// <summary>
        /// Облигация
        /// </summary>
        Bond = 2,

        /// <summary>
        /// Валюта
        /// </summary>
        Currency = 3,

        /// <summary>
        /// Фонд
        /// </summary>
        Etf = 4
    }
}

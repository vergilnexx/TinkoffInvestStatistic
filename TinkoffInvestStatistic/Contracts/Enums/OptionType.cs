using System.ComponentModel;

namespace TinkoffInvestStatistic.Contracts.Enums
{
    /// <summary>
    /// Тип настройки.
    /// </summary>
    public enum OptionType
    {
        /// <summary>
        /// Неопределенно.
        /// </summary>
        Undefined = 0,

        /// <summary>
        /// Скрывать данные при входе.
        /// </summary>
        [Description("Скрывать данные при входе")]
        IsHideMoney = 1,

        /// <summary>
        /// Отображать заблокированные позиции.
        /// </summary>
        [Description("Отображать заблокированные позиции")]
        IsShowBlockedPositions = 2,
    }
}

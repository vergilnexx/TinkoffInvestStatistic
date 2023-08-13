using SQLite;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Domain
{
    /// <summary>
    /// Данные настроек.
    /// </summary>
    public class OptionData
    {
        /// <summary>
        /// Номер счета.
        /// </summary>
        [PrimaryKey]
        public OptionType Type { get; set; }

        /// <summary>
        /// Значение.
        /// </summary>
        public string Value { get; set; }
    }
}

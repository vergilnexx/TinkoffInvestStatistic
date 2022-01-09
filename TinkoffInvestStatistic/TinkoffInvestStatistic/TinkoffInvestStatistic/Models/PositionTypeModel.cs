using Contracts.Enums;
using Infrastructure.Helpers;
using TinkoffInvestStatistic.Utility;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель типа инструмента.
    /// </summary>
    public class PositionTypeModel
    {
        /// <summary>
        /// Наименоавние типа.
        /// </summary>
        public string TypeName => Type.GetDescription();

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Текущий процент от суммы по всем инструментам.
        /// </summary>
        public decimal CurrentPercent { get; set; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public decimal CurrentSum { get; set; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public string CurrentSumText => CurrencyUtility.ToCurrencyString(CurrentSum, Currency.Rub);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип./param>
        public PositionTypeModel(PositionType type)
        {
            Type = type;
        }
    }
}

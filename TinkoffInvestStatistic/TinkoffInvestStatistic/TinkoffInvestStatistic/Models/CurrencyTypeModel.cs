using Contracts.Enums;
using Infrastructure.Helpers;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель о валюте.
    /// </summary>
    public class CurrencyTypeModel
    {
        /// <summary>
        /// Наименоавние валюты.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public decimal CurrentSum { get; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public string CurrentSumText => CurrencyUtility.ToCurrencyString(CurrentSum, Currency.Rub);

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; }

        /// <summary>
        /// Текущий процент от суммы по всем инструментам.
        /// </summary>
        public decimal CurrentPercent { get; }

        /// <summary>
        /// Цвет процента.
        /// </summary>
        public Color CurrentPercentColor => DifferencePercentUtility.GetPercentColor(CurrentPercent, PlanPercent);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="currentSum">Сумма.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        /// <param name="currentPercent">Текущий процент.</param>
        public CurrencyTypeModel(Currency currency, decimal currentSum, decimal planPercent, decimal currentPercent)
        {
            Name = Currency.GetDescription();
            Currency = currency;
            CurrentSum = currentSum;
            PlanPercent = planPercent;
            CurrentPercent = currentPercent;
        }
    }
}

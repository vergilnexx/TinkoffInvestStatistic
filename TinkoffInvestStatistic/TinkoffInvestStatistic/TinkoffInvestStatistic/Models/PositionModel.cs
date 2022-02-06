using Contracts.Enums;
using Infrastructure.Helpers;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель позиции.
    /// </summary>
    public class PositionModel
    {
        /// <summary>
        /// Финансовый идентификатор.
        /// </summary>
        public string Figi { get; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker { get; set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; }

        /// <summary>
        /// Тип.
        /// </summary>
        public string TypeName => Type.GetDescription();

        /// <summary>
        /// Баланс.
        /// </summary>
        public decimal PositionCount { get; set; }

        /// <summary>
        /// Заблокировано.
        /// </summary>
        public decimal Blocked { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public decimal SumInCurrency { get; set; }

        /// <summary>
        /// Разница в валюте.
        /// </summary>
        public decimal DifferenceSumInCurrency { get; set; }

        /// <summary>
        /// Текст разницы в валюте.
        /// </summary>
        public string DifferenceSumInCurrencyText => CurrencyUtility.ToCurrencyString(DifferenceSumInCurrency, Currency);

        /// <summary>
        /// Разница.
        /// </summary>
        public decimal DifferenceSum { get; set; }

        /// <summary>
        /// Текст разницы.
        /// </summary>
        public string DifferenceSumText => Currency != Currency.Rub ? $" / {CurrencyUtility.ToCurrencyString(DifferenceSum, Currency.Rub)}" : string.Empty;

        /// <summary>
        /// Цвет текста разницы в валюте.
        /// </summary>
        public Color DifferenceSumInCurrencyTextColor { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Текст суммы.
        /// </summary>
        public string SumText => Currency != Currency.Rub ? $" / {CurrencyUtility.ToCurrencyString(Sum, Currency.Rub)}" : string.Empty;

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Текущий процент.
        /// </summary>
        public decimal CurrentPercent { get; set; }

        /// <summary>
        /// Цвет процента.
        /// </summary>
        public Color CurrentPercentColor => DifferencePercentUtility.GetPercentColor(CurrentPercent, PlanPercent);

        /// <summary>
        /// Текст суммы в валюте.
        /// </summary>
        public string SumInCurrencyText => CurrencyUtility.ToCurrencyString(SumInCurrency, Currency);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="type">Тип позиции.</param>
        public PositionModel(string figi, PositionType type)
        {
            Figi = figi;
            Type = type;
        }
    }
}

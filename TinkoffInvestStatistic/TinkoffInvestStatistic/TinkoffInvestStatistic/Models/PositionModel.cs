using Infrastructure.Helpers;
using System.Globalization;
using TinkoffInvestStatistic.Contracts.Enums;
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
        public string DifferenceSumInCurrencyText { get; set; }

        /// <summary>
        /// Разница.
        /// </summary>
        public decimal DifferenceSum { get; set; }

        /// <summary>
        /// Текст разницы.
        /// </summary>
        public string DifferenceSumText { get; set; }

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
        public string SumText { get; set; }

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public string PlanPercent { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercentValue => NumericUtility.TryParse(PlanPercent);

        /// <summary>
        /// Текущий процент.
        /// </summary>
        public decimal CurrentPercent { get; set; }

        /// <summary>
        /// Цвет процента.
        /// </summary>
        public Color CurrentPercentColor => DifferencePercentUtility.GetPercentColor(CurrentPercent, PlanPercentValue);

        /// <summary>
        /// Текст суммы в валюте.
        /// </summary>
        public string SumInCurrencyText { get; set; }

        /// <summary>
        /// Признак, что позиция заблокирована.
        /// </summary>
        public bool IsBlocked { get; set; }

        /// <summary>
        /// Цвет процента.
        /// </summary>
        public Color NameColor => IsBlocked ? Color.Red : Color.WhiteSmoke;

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

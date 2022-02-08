using Contracts.Enums;

namespace Domain
{
    /// <summary>
    /// Данные о валюте.
    /// </summary>
    public class CurrencyData
    {
        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        public CurrencyData(Currency currency)
        {
            Currency = currency;
            PlanPercent = 0;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        public CurrencyData(Currency currency, decimal planPercent)
        {
            Currency = currency;
            PlanPercent = planPercent;
        }
    }
}

using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Деньги в валюте.
    /// </summary>
    public class CurrencyMoney
    {
        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Сумма.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="sum">Сумма.</param>
        public CurrencyMoney(Currency currency, decimal sum)
        {
            Currency = currency;
            Sum = sum;
        }
    }
}

using Contracts.Enums;

namespace Contracts
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
        /// Количество.
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="value">Количество.</param>
        public CurrencyMoney(Currency currency, decimal value)
        {
            Currency = currency;
            Value = value;
        }
    }
}

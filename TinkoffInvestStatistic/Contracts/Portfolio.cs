using System.Collections.Generic;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Портфель.
    /// </summary>
    public class Portfolio
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        public string AccountId { get; }

        /// <summary>
        /// Полная стоимость портфеля.
        /// </summary>
        public CurrencyMoney TotalAmount { get; }

        /// <summary>
        /// Полная стоимость акций в портфеле.
        /// </summary>
        public CurrencyMoney TotalAmountStocks { get; set; }

        /// <summary>
        /// Полная стоимость облигаций в портфеле.
        /// </summary>
        public CurrencyMoney TotalAmountBonds { get; set; }

        /// <summary>
        /// Полная стоимость фондов в портфеле.
        /// </summary>
        public CurrencyMoney TotalAmountEtf { get; set; }

        /// <summary>
        /// Полная стоимость валют в портфеле.
        /// </summary>
        public CurrencyMoney TotalAmountCurrencies { get; set; }

        /// <summary>
        /// Позиции.
        /// </summary>
        public IReadOnlyCollection<Position> Positions { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="totalAmount">Полная стоимость портфеля.</param>
        /// <param name="positions">Позиции.</param>
        public Portfolio(string accountId, CurrencyMoney totalAmount, IReadOnlyCollection<Position> positions)
        {
            AccountId = accountId;
            TotalAmount = totalAmount;
            Positions = positions;
        }
    }
}

using TinkoffInvestStatistic.Utility;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель счета.
    /// </summary>
    public class AccountModel
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        public string AccountId { get; }

        /// <summary>
        /// Тип счета.
        /// </summary>
        public string AccountType { get; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public decimal CurrentSum { get; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public string CurrentSumText => CurrencyUtility.ToCurrencyString(CurrentSum, Contracts.Enums.Currency.Rub);

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="accountType">Тип счета.</param>
        /// <param name="currentSum">Текущая сумма по счету.</param>
        public AccountModel(string accountId, string accountType, decimal currentSum)
        {
            AccountId = accountId;
            AccountType = accountType;
            CurrentSum = currentSum;
        }
    }
}

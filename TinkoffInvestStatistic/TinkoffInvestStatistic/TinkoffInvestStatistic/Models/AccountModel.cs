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
        /// Наименование счета.
        /// </summary>
        public string Name { get; }

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
        /// <param name="name">Наименование счета.</param>
        /// <param name="accountType">Тип счета.</param>
        /// <param name="currentSum">Текущая сумма по счету.</param>
        public AccountModel(string accountId, string name, string accountType, decimal currentSum)
        {
            AccountId = accountId;
            Name = name;
            AccountType = accountType;
            CurrentSum = currentSum;
        }
    }
}

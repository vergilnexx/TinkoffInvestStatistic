using Contracts.Enums;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace Domain
{
    /// <summary>
    /// Данные о валюте.
    /// </summary>
    public class CurrencyData
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Номер счета.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public CurrencyData() 
        { 
        }

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
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="currency">Валюта.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        public CurrencyData(string accountNumber, Currency currency, decimal planPercent)
        {
            AccountNumber = accountNumber;
            Currency = currency;
            PlanPercent = planPercent;
        }
    }
}

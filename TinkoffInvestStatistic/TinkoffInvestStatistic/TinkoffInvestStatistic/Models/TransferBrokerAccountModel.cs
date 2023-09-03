using TinkoffInvestStatistic.Utility;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель данных зачислений по счету брокера.
    /// </summary>
    public class TransferBrokerAccountModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Наименование.</param>
        /// <param name="sum">Сумма.</param>
        public TransferBrokerAccountModel(string name, decimal sum)
        {
            Name = name;
            Sum = sum;
        }

        public string Name { get; }

        /// <summary>
        /// Сумма зачислений.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Сумма зачислений для отображения.
        /// </summary>
        public string SumText => NumericUtility.ToCurrencyString(Sum, Contracts.Enums.Currency.Rub);

        /// <summary>
        /// Зачислить.
        /// </summary>
        public decimal Amount { get; set; }
    }
}
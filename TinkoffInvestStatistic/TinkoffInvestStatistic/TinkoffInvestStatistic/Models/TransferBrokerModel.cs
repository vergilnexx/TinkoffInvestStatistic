using System;
using System.Collections.Generic;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель данных зачислений по брокеру.
    /// </summary>
    public class TransferBrokerModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        public TransferBrokerModel(string brokerName)
        {
            BrokerName = brokerName ?? string.Empty;
        }

        /// <summary>
        /// Наименование брокера.
        /// </summary>
        public string BrokerName { get; }

        /// <summary>
        /// Текстовое представление суммы.
        /// </summary>
        public string SumText { get; set; }

        /// <summary>
        /// Данные по счетам.
        /// </summary>
        public IReadOnlyCollection<TransferBrokerAccountModel> AccountData { get; set; } = Array.Empty<TransferBrokerAccountModel>();
    }
}

using System;
using System.Collections.Generic;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Данные зачислений по брокерам.
    /// </summary>
    public class TransferBroker
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        public TransferBroker(string brokerName)
        {
            BrokerName = brokerName ?? string.Empty;
        }

        /// <summary>
        /// Наименование брокера.
        /// </summary>
        public string BrokerName { get; }

        /// <summary>
        /// Данные счета.
        /// </summary>
        public IReadOnlyCollection<TransferBrokerAccount> AccountData { get; set; } = Array.Empty<TransferBrokerAccount>();
    }
}

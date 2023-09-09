using System;

namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Данные экспорта зачислений.
    /// </summary>
    public class TransferExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TransferExportData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="brokerName"> Наименование брокера.</param>
        public TransferExportData(string brokerName) : this()
        {
            BrokerName = brokerName;
        }

        /// <summary>
        /// Наименование брокера.
        /// </summary>
        public string BrokerName { get; set; } = string.Empty;

        /// <summary>
        /// Данные счета.
        /// </summary>
        public TransferAccountExportData[] AccountData { get; set; } = Array.Empty<TransferAccountExportData>();
    }
}

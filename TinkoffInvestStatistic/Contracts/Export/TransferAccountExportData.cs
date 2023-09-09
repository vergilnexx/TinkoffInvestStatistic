namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Данные экспорта зачислений по счету.
    /// </summary>
    public class TransferAccountExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public TransferAccountExportData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Наименование счета.</param>
        /// <param name="sum">Сумма зачислений.</param>
        public TransferAccountExportData(string name, decimal sum) : this()
        {
            Name = name;
            Sum = sum;
        }

        /// <summary>
        /// Наименование счета.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Сумма зачислений.
        /// </summary>
        public decimal Sum { get; set; }
    }
}

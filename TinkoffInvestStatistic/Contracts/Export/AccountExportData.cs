using System;

namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Данные счетого для экспорта.
    /// </summary>
    public class AccountExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public AccountExportData()
        {
            PositionTypes = Array.Empty<PositionTypeExportData>();
            Currencies = Array.Empty<CurrencyExportData>();
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        public AccountExportData(string accountId) : this()
        {
            AccountId = accountId;
        }

        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        public string? AccountId { get; }

        /// <summary>
        /// Данные типов позиций для экспорта.
        /// </summary>
        public PositionTypeExportData[] PositionTypes { get; set; }

        /// <summary>
        /// Данные валют для экспорта.
        /// </summary>
        public CurrencyExportData[] Currencies { get; set; }
    }
}

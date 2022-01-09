﻿using System.Globalization;

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
        public string AccountId { get; set; }

        /// <summary>
        /// Тип счета.
        /// </summary>
        public string AccountType { get; set; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public decimal CurrentSum { get; set; }

        /// <summary>
        /// Текущая сумма по счету.
        /// </summary>
        public string CurrentSumText => CurrentSum.ToString("C", CultureInfo.GetCultureInfo("ru-RU"));

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="accountId">Идентификатор счета.</param>
        /// <param name="accountType">Тип счета.</param>
        public AccountModel(string accountId, string accountType)
        {
            AccountId = accountId;
            AccountType = accountType;
        }
    }
}

﻿using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Данные счет по валютно.
    /// </summary>
    public class AccountCurrencyData
    {
        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Сумма в рублях.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="sum">Сумма.</param>
        public AccountCurrencyData(Currency currency, decimal sum)
            : this(currency, planPercent: 0m, sum)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <param name="planPercent">Планируемый процент.</param>
        /// <param name="sum">Сумма.</param>
        public AccountCurrencyData(Currency currency, decimal planPercent, decimal sum)
        {
            Currency = currency;
            PlanPercent = planPercent;
            Sum = sum;
        }
    }
}
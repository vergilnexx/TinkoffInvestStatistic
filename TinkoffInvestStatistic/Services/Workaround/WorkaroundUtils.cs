using System;
using System.Collections.Generic;
using System.Linq;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Services.Workaround
{
    /// <summary>
    /// Методы для обхода проблем API банков.
    /// </summary>
    public static class WorkaroundUtils
    {
        /// <summary>
        /// Возвращает сумму в валюте которой нет в API банка.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <returns>Курс валюты в рублях</returns>
        public static decimal? GetCurrencySumInRubbles(IReadOnlyCollection<CurrencyMoney> currencies, Currency? currency)
        {
            return null;
        }
    }
}

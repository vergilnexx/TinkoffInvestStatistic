using Contracts;
using Contracts.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (currency == Currency.Hkd)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 7.85m;
            }
            return null;
        }
    }
}

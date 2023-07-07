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
            if (currency == Currency.Hkd)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 7.85m;
            }
            if (currency == Currency.Cny)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 6.79m;
            }
            if (currency == Currency.Chf)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 0.93m;
            }
            if (currency == Currency.Gbp)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 0.83m;
            }
            if (currency == Currency.Try)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 18.83m;
            }
            if (currency == Currency.Jpy)
            {
                var usd = currencies.FirstOrDefault(c => c.Currency == Currency.Usd);
                if (usd == null)
                {
                    throw new ApplicationException("Не найдена валюта типа: " + Currency.Usd);
                }
                return usd.Sum / 132.82m;
            }
            return null;
        }
    }
}

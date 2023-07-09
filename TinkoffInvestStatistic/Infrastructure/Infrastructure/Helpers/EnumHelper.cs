using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using TinkoffInvestStatistic.Contracts.Attributes;
using TinkoffInvestStatistic.Contracts.Enums;

namespace Infrastructure.Helpers
{
    /// <summary>
    /// Helper для работы с <see cref="DescriptionAttribute"/>
    /// </summary>
    public static class EnumHelper
    {
        /// <summary>
        /// Возвращает значение атрибута описания.
        /// </summary>
        /// <typeparam name="T">Тип перечисления.</typeparam>
        /// <param name="source">Источник.</param>
        /// <returns>Описание.</returns>
        public static string GetDescription<T>(this T source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return source.ToString();
            }
        }

        /// <summary>
        /// Возвращает фин.уникальный идентификатор валюты.
        /// </summary>
        /// <param name="currency">Валюта.</param>
        /// <returns>Фин.уникальный идентификатор валюты.</returns>
        public static string GetFigi(this Currency currency)
        {
            FieldInfo fi = currency.GetType().GetField(currency.ToString());

            FigiAttribute[] attributes = (FigiAttribute[])fi.GetCustomAttributes(typeof(FigiAttribute), inherit: false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Возвращает валюту по фин.уникальный идентификатор валюты.
        /// </summary>
        /// <param name="currencyFigi">фин.уникальный идентификатор валюты.</param>
        /// <returns>Валюта.</returns>
        public static Currency? GetCurrencyByFigi(string currencyFigi)
        {
            var currencies = Enum.GetValues(typeof(Currency)).Cast<Currency>().ToArray();
            return currencies.FirstOrDefault(c => GetFigi(c) == currencyFigi);
        }
    }
}

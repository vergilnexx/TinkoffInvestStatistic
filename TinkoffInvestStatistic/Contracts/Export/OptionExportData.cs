﻿using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts.Export
{
    /// <summary>
    /// Данные экспорта настроек.
    /// </summary>
    public class OptionExportData
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public OptionExportData() 
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип настройки.</param>
        /// <param name="value">Значение.</param>
        public OptionExportData(OptionType type, string value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        /// Тип настройки.
        /// </summary>
        public OptionType Type { get; }

        /// <summary>
        /// Значение.
        /// </summary>
        public string? Value { get; }

    }
}

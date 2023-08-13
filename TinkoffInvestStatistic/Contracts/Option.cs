using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Настройка.
    /// </summary>
    public class Option
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип настройки.</param>
        /// <param name="value">Значение.</param>
        public Option(OptionType type, string value)
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
        public string Value { get; }

        /// <summary>
        /// Возвращает значение в виде признака.
        /// </summary>
        /// <returns>Признак.</returns>
        public bool? ToBoolean()
        {
            if (bool.TryParse(Value, out bool boolValue))
            {
                return boolValue;
            }

            return null;
        }
    }
}

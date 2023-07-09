using TinkoffInvestStatistic.Contracts.Enums;

namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Позиция.
    /// </summary>
    public class Position
    {
        /// <summary>
        /// Financial Instrument Global Identifier.
        /// </summary>
        public string Figi { get; private set; }

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; private set; }

        /// <summary>
        /// Валюта.
        /// </summary>
        public Currency Currency { get; set; }

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker { get; set; } = string.Empty;

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Количество позиций.
        /// </summary>
        public decimal PositionCount { get; set; }

        /// <summary>
        /// Планируемый процент.
        /// </summary>
        public decimal PlanPercent { get; set; }

        /// <summary>
        /// Текущее изменение цены.
        /// </summary>
        public CurrencyMoney? ExpectedYield { get; set; }

        /// <summary>
        /// Цена покупки.
        /// </summary>
        public CurrencyMoney? AveragePositionPrice { get; set; }

        /// <summary>
        /// Текущая цена в валюте.
        /// </summary>
        public CurrencyMoney CurrentPrice { get; set; }

        /// <summary>
        /// Сумма в рублях.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public decimal SumInCurrency { get; set; }

        /// <summary>
        /// Сумма разницы.
        /// </summary>
        public decimal DifferenceSum { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="type">Тип инструмента.</param>
        public Position(string figi, PositionType type)
        {
            Figi = figi;
            Type = type;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="figi">Финансовый идентификатор.</param>
        /// <param name="type">Тип инструмента.</param>
        /// <param name="name">Наименование.</param>
        /// <param name="sum">Сумма в валюте.</param>
        /// <param name="currency">Валюта.</param>-
        public Position(string figi, PositionType type, string name, decimal sum, Currency currency)
        {
            Figi = figi;
            Type = type;
            Name = name;
            Sum = sum;
            SumInCurrency = sum;
        }
    }
}

using Contracts.Enums;

namespace Contracts
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
        /// Заблокировано.
        /// </summary>
        public decimal Blocked { get; set; }

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
        /// Цена покупки без налогов.
        /// </summary>
        public CurrencyMoney? AveragePositionPriceNoNkd { get; set; }

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Сумма в валюте.
        /// </summary>
        public decimal SumInCurrency { get; set; }

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
    }
}

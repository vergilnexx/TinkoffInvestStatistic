using System;

namespace Domain
{
    /// <summary>
    /// Данные по счету.
    /// </summary>
    public class AccountData
    {
        /// <summary>
        /// Номер счета.
        /// </summary>
        public string Number { get; private set; }

        /// <summary>
        /// Данные по инструментам.
        /// </summary>
        public InstrumentData[] Instruments { get; set; } = Array.Empty<InstrumentData>();

        /// <summary>
        /// Информация о позициях.
        /// </summary>
        public PositionData[] Positions { get; set; } = Array.Empty<PositionData>();

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="number">Номер счета.</param>
        public AccountData(string number)
        {
            this.Number = number;
        }
    }
}

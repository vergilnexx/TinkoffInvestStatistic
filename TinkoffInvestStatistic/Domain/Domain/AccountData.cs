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
        public InstrumentData[] Instruments { get; set; }

        /// <summary>
        /// Информация о позициях.
        /// </summary>
        public PositionData[] Positions { get; set; }

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

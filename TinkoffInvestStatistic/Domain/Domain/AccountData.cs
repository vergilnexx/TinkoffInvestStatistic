using SQLite;

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
        [PrimaryKey]
        public string Number { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AccountData()
        {
        }

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

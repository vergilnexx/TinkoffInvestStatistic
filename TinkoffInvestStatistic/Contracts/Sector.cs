namespace TinkoffInvestStatistic.Contracts
{
    /// <summary>
    /// Сектор.
    /// </summary>
    public class Sector
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Наименование.</param>
        public Sector(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="name">Наименование.</param>
        public Sector(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

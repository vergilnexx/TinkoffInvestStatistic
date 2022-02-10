namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель сектора.
    /// </summary>
    public class SectorModel
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
        public SectorModel(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="name">Наименование.</param>
        public SectorModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

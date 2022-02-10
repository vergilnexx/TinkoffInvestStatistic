using SQLite;

namespace Domain
{
    /// <summary>
    /// Сектор.
    /// </summary>
    public class SectorData
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public SectorData()
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Наименование.</param>
        public SectorData(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="id">Идентификатор.</param>
        /// <param name="name">Наименование.</param>
        public SectorData(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}

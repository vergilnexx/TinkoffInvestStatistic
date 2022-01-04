using Contracts.Enums;
using Infrastructure.Helpers;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель типа инструмента.
    /// </summary>
    public class PositionTypeModel
    {
        /// <summary>
        /// Наименоавние типа.
        /// </summary>
        public string TypeName => Type.GetDescription();

        /// <summary>
        /// Тип.
        /// </summary>
        public PositionType Type { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="type">Тип./param>
        public PositionTypeModel(PositionType type)
        {
            Type = type;
        }
    }
}

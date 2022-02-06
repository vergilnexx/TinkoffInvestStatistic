using Contracts.Enums;
using Infrastructure.Helpers;
using System.Collections.Generic;

namespace TinkoffInvestStatistic.Models
{
    /// <summary>
    /// Модель группированного списка позиций.
    /// </summary>
    public class GroupedPositionsModel : List<PositionModel>
    {
        /// <summary>
        /// Наименование группы.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Тип группы.
        /// </summary>
        public PositionType Type { get; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupedPositionsModel(PositionType type, List<PositionModel> animals) : base(animals)
        {
            Type = type;
            Name = type.GetDescription();
        }
    }
}

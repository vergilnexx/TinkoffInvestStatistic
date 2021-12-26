using System;
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
        public string Name { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public GroupedPositionsModel(string name, List<PositionModel> animals) : base(animals)
        {
            Name = name;
        }
    }
}

using Contracts;
using Contracts.Enums;
using Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class InstrumentService : IInstrumentService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Instrument>> GetPositionTypes(string accountNumber)
        {
            var positionTypes = Enum
                                  .GetValues(typeof(PositionType))
                                  .Cast<PositionType>()
                                  .ToArray();

            var filledPositionTypes = await DataStorageService.Instance.MergePositionTypesData(accountNumber, positionTypes);

            var positionService = DependencyService.Resolve<IPositionService>();
            foreach (var positionType in filledPositionTypes)
            {
                positionType.Sum = await positionService.GetPositionsSumAsync(accountNumber, positionType.Type);
            }

            return filledPositionTypes;
        }

        /// <inheritdoc/>
        public Task SavePlanPercents(string accountNumber, InstrumentData[] data)
        {
            return DataStorageService.Instance.SetPositionTypesData(accountNumber, data);
        }
    }
}

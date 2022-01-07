using Contracts.Enums;
using Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    /// <inheritdoc/>
    public class InstrumentService : IInstrumentService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<InstrumentData>> GetPositionTypes(string accountNumber)
        {
            var positionTypes = Enum
                                  .GetValues(typeof(PositionType))
                                  .Cast<PositionType>()
                                  .ToArray();

            var filledPositionTypes = await DataStorageService.Instance.MergePositionTypesData(accountNumber, positionTypes);

            return filledPositionTypes;
        }

        /// <inheritdoc/>
        public Task SavePlanPercents(string accountNumber, InstrumentData[] data)
        {
            return DataStorageService.Instance.SetPositionTypesData(accountNumber, data);
        }
    }
}

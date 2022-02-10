using Contracts;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    ///<inheritdoc/>
    public class SectorService : ISectorService
    {
        ///<inheritdoc/>
        public Task<Sector> GetSectorAsync(int sectorId)
        {
            return DataStorageService.Instance.GetSectorAsync(sectorId);
        }

        ///<inheritdoc/>
        public Task<IReadOnlyCollection<Sector>> GetSectorsAsync()
        {
            return DataStorageService.Instance.GetSectorsAsync();
        }

        ///<inheritdoc/>
        public Task AddSectorAsync(Sector sector)
        {
            return DataStorageService.Instance.AddSectorAsync(sector);
        }

        ///<inheritdoc/>
        public Task UpdateSectorAsync(Sector sector)
        {
            return DataStorageService.Instance.UpdateSectorAsync(sector);
        }
    }
}

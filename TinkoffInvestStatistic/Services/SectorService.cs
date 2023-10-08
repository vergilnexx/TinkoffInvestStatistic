using Domain;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;

namespace Services
{
    ///<inheritdoc/>
    public class SectorService : ISectorService
    {
        ///<inheritdoc/>
        public async Task<Sector> GetSectorAsync(int sectorId)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var data = await dataAccessService.GetSectorAsync(sectorId);
            return new Sector(data.Id, data.Name);
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<Sector>> GetSectorsAsync()
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var data = await dataAccessService.GetSectorsAsync();
            return data.Select(s => new Sector(s.Id, s.Name)).ToArray();
        }

        ///<inheritdoc/>
        public async Task AddSectorAsync(Sector sector)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.AddSectorAsync(new SectorData(sector.Id, sector.Name));
        }

        ///<inheritdoc/>
        public async Task UpdateSectorAsync(Sector sector)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.UpdateSectorAsync(new SectorData(sector.Id, sector.Name));
        }
    }
}

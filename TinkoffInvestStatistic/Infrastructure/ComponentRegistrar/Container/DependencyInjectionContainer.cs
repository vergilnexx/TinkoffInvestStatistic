using Clients.TinkoffInvest;
using Infrastructure.Clients;
using Infrastructure.Services;
using Services;
using System.Collections.Generic;
using TinkoffInvest.Mappers;
using Xamarin.Forms;

namespace Infrastructure.Container
{
    public static class DependencyInjectionContainer
    {
        public static void Configure()
        {
            ConfigureServices();

            ConfigureClients();

            ConfigureMappers();
        }

        private static void ConfigureServices()
        {
            DependencyService.Register<IAccountService, AccountService>();
            DependencyService.Register<IPositionService, PositionService>();
            DependencyService.Register<IInstrumentService, InstrumentService>();
            DependencyService.Register<IDataStorageAccessService, FileService>();
        }

        private static void ConfigureClients()
        {
            DependencyService.Register<IBankBrokerApiClient, TinkoffInvestClient>();
        }

        private static void ConfigureMappers()
        {
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Account, Contracts.Account>,
                TinkoffAccountToAccountMapper>();
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.Position>>,
                TinkoffPositionToPositionMapper>();
        }
    }
}

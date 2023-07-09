using Clients.TinkoffInvest;
using Infrastructure.Clients;
using Infrastructure.Services;
using Services;
using TinkoffInvest.Contracts.Instruments;
using TinkoffInvest.Contracts.Portfolio;
using TinkoffInvest.Mappers;
using TinkoffInvestStatistic.Contracts;
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
            DependencyService.Register<ICurrencyService, CurrencyService>();
            DependencyService.Register<IDataStorageAccessService, DatabaseService>();
            DependencyService.Register<ISectorService, SectorService>();
        }

        private static void ConfigureClients()
        {
            DependencyService.Register<IBankBrokerApiClient, TinkoffInvestClient>();
        }

        private static void ConfigureMappers()
        {
            DependencyService.Register<IMapper<TinkoffInvest.Contracts.Accounts.Account, TinkoffInvestStatistic.Contracts.Account>,
                TinkoffAccountToAccountMapper>();
            DependencyService.Register<IMapper<PortfolioReponse, Portfolio>,
                TinkoffPortfolioToPortfolioMapper>();
            DependencyService.Register<IMapper<InstrumentResponse, TinkoffInvestStatistic.Contracts.Position>,
                TinkoffPositionToPositionMapper>();
        }
    }
}

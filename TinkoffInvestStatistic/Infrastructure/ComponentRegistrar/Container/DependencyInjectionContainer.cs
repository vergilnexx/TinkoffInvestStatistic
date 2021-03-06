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
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Account, Contracts.Account>,
                TinkoffAccountToAccountMapper>();
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.Position>>,
                TinkoffPortfolioToPositionMapper>();
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<Contracts.CurrencyMoney>>,
                TinkoffPortfolioToCurrencyMoneyMapper>();
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.PortfolioCurrencies, IReadOnlyCollection<Contracts.CurrencyMoney>>,
                TinkoffPortfolioCurrenciesToCurrencyMoneyMapper>();
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.MarketInstrumentList, IReadOnlyCollection<Contracts.Position>>,
                TinkoffMarketInstrumentListToPositionListMapper>();
        }
    }
}

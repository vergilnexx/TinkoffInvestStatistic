using Clients.TinkoffInvest;
using Infrastructure.Clients;
using Infrastructure.Services;
using Services;
using System.Collections.Generic;
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
            //DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<CurrencyMoney>>,
            //    TinkoffPortfolioToCurrencyMoneyMapper>();
            //DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.PortfolioCurrencies, IReadOnlyCollection<CurrencyMoney>>,
            //    TinkoffPortfolioCurrenciesToCurrencyMoneyMapper>();
            //DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.MarketInstrumentList, IReadOnlyCollection<Position>>,
            //    TinkoffMarketInstrumentListToPositionListMapper>();
        }
    }
}

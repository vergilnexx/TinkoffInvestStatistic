using Clients.TinkoffInvest;
using Infrastructure.Clients;
using Infrastructure.Services;
using Services;
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
        }

        private static void ConfigureClients()
        {
            DependencyService.Register<IBankBrokerApiClient, TinkoffInvestClient>();
        }

        private static void ConfigureMappers()
        {
            DependencyService.Register<IMapper<Tinkoff.Trading.OpenApi.Models.Account, Contracts.Account>,
                TinkoffAccountToAccountMapper>();
        }
    }
}

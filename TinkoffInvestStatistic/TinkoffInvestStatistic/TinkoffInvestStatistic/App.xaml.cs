using Infrastructure.Container;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ConfigureUtility();
            ConfigureViewModels();

            DependencyInjectionContainer.Configure();

            MainPage = new AppShell();
        }

        private static void ConfigureUtility()
        {
            DependencyService.RegisterSingleton(new ChartUtility());
            DependencyService.RegisterSingleton(new ChartColors());
        }

        private static void ConfigureViewModels()
        {
            DependencyService.Register<PortfolioViewModel>();
            DependencyService.Register<AccountsViewModel>();
            DependencyService.Register<LoginViewModel>();
            DependencyService.Register<NewItemViewModel>();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

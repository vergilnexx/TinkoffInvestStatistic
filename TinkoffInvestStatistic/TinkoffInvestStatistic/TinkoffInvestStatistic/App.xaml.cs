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
            DependencyService.RegisterSingleton(ChartUtility.Instance ?? new ChartUtility());
            DependencyService.RegisterSingleton(ChartColors.Instance ?? new ChartColors());
        }

        private static void ConfigureViewModels()
        {
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

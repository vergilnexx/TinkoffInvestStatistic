using Infrastructure.Container;
using Infrastructure.Services;
using Services;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Utility;
using Xamarin.Forms;

namespace TinkoffInvestStatistic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Application.Current.UserAppTheme = OSAppTheme.Dark;

            ConfigureUtility();

            DependencyInjectionContainer.Configure();

            MainPage = new AppShell();
        }

        private static void ConfigureUtility()
        {
            DependencyService.RegisterSingleton(new ChartUtility());
            DependencyService.RegisterSingleton(new ChartColorsUtility());

            DependencyService.RegisterSingleton(new DataStorageService());
        }

        protected override void OnStart()
        {
            base.OnStart();
            Task.Run(() => DataStorageService.Instance.LoadAccountData());
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

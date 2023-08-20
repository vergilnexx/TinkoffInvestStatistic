using Infrastructure.Container;
using Infrastructure.Services;
using Services;
using System.Threading.Tasks;
using System.Threading;
using TinkoffInvestStatistic.Service;
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
            DependencyService.Register<IMessageService, MessageService>();
            DependencyService.RegisterSingleton<IHideShowMoneyService>(new HideShowMoneyService());

            DependencyService.RegisterSingleton(new ChartUtility());
            DependencyService.RegisterSingleton(new ChartColorsUtility());

            DependencyService.RegisterSingleton(new DataStorageService());
        }

        protected override void OnStart()
        {
            InitSettings();
        }

        protected override void OnResume()
        {
            InitSettings();
        }

        private void InitSettings()
        {
            var service = DependencyService.Get<ISettingService>();

            using var cancelTokenSource = new CancellationTokenSource();
            var cancellation = cancelTokenSource.Token;
            InitShowHideMoneySetting(service, cancellation);
        }

        private static void InitShowHideMoneySetting(ISettingService service, CancellationToken cancellation)
        {
            var isHideMoneyString = Task
                                    .Run(() => service.GetAsync(Contracts.Enums.OptionType.IsHideMoney, cancellation))
                                    .GetAwaiter()
                                    .GetResult();

            var hideShowMoneyService = DependencyService.Get<IHideShowMoneyService>();
            if (bool.TryParse(isHideMoneyString, out var isHideMoney))
            {
                hideShowMoneyService.SetShow(!isHideMoney);
            }
        }
    }
}

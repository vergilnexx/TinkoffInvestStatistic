using Infrastructure.Container;
using Infrastructure.Services;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Controls;
using Services;
using System;
using System.Threading.Tasks;
using System.Threading;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.Utility;

namespace TinkoffInvestStatistic
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Application.Current!.UserAppTheme = AppTheme.Dark;

            ConfigureUtility();

            DependencyInjectionContainer.Configure();

            MainPage = new AppShell();
        }

        private static void ConfigureUtility()
        {
            DependencyService.Register<IMessageService, MessageService>();
            DependencyService.RegisterSingleton<IHideShowMoneyService>(new HideShowMoneyService());
            DependencyService.RegisterSingleton<IAuthenticateService>(new AuthenticateService());

            DependencyService.RegisterSingleton(new ChartUtility());
            DependencyService.RegisterSingleton(new ChartColorsUtility());
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
            try
            {
                var service = DependencyService.Get<ISettingService>();
                if (service == null)
                {
                    return;
                }

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                InitShowHideMoneySetting(service, cancellation);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"InitSettings failed: {ex}");
            }
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

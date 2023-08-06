﻿using Infrastructure.Container;
using Services;
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

        protected override void OnSleep()
        {
        }
    }
}

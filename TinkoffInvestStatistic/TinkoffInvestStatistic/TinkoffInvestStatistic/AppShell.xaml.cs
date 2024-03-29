﻿using System;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(AccountStatisticPage), typeof(AccountStatisticPage));
            Routing.RegisterRoute(nameof(PositionTypesPage), typeof(PositionTypesPage));
            Routing.RegisterRoute(nameof(PortfolioPage), typeof(PortfolioPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(ExportPage), typeof(ExportPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}

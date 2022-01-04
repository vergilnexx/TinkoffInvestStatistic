﻿using Infrastructure.Helpers;
using Infrastructure.Services;
using Microcharts;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {
        private AccountModel _selectedItem;

        public ObservableCollection<AccountModel> Accounts { get; }
        public Chart StatisticChart { get; private set; }
        public Command LoadAccountsCommand { get; }
        public Command<AccountModel> ItemTapped { get; }

        public AccountsViewModel()
        {
            Title = "Счета";
            Accounts = new ObservableCollection<AccountModel>();
            LoadAccountsCommand = new Command(async () => await ExecuteLoadAccountsCommand());
            ItemTapped = new Command<AccountModel>(OnAccountSelected);
        }

        private async Task ExecuteLoadAccountsCommand()
        {
            IsBusy = true;

            try
            {
                Accounts.Clear();
                var service = DependencyService.Get<IAccountService>();
                var accounts = await service.GetAccountsAsync();
                foreach (var item in accounts)
                {
                    var model = new AccountModel();

                    model.AccountId = item.ID;
                    model.AccountType = item.Type.GetDescription();

                    Accounts.Add(model);
                }

                await LoadStatisticChartAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async Task LoadStatisticChartAsync()
        {
            StatisticChart = await ChartUtility.Instance.GetChartAsync(this);
            OnPropertyChanged(nameof(StatisticChart));
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public AccountModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnAccountSelected(value);
            }
        }

        async void OnAccountSelected(AccountModel item)
        {
            if (item == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(PortfolioPage)}" +
                $"?{nameof(PortfolioViewModel.AccountId)}={item.AccountId}" +
                $"&{nameof(PortfolioViewModel.AccountTitle)}={item.AccountId/*item.AccountType.GetDescription()*/}");
        }
    }
}
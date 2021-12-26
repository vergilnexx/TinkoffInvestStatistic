using Infrastructure.Helpers;
using Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    public class AccountsViewModel : BaseViewModel
    {
        private AccountModel _selectedItem;

        public ObservableCollection<AccountModel> Accounts { get; }
        public Command LoadAccountsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<AccountModel> ItemTapped { get; }

        public AccountsViewModel()
        {
            Title = "Счета";
            Accounts = new ObservableCollection<AccountModel>();
            LoadAccountsCommand = new Command(async () => await ExecuteLoadAccountsCommand());

            ItemTapped = new Command<AccountModel>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadAccountsCommand()
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
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewItemPage));
        }

        async void OnItemSelected(AccountModel item)
        {
            if (item == null)
            {
                return;
            }

            // This will push the ItemDetailPage onto the navigation stack
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.AccountId}");
        }
    }
}
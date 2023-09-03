using Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.Utility;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Данные представления страницы зачислений.
    /// </summary>
    public class TransferViewModel : BaseViewModel
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        internal TransferViewModel()
        {
            Brokers = new ObservableCollection<TransferBrokerModel>();
            LoadCommand = new Command(async () => await LoadAsync());
            SaveCommand = new Command(async() => await SaveAsync());
            AddBrokerAccountCommand = new Command<TransferBrokerModel>(AddBrokerAccountAsync);
        }

        /// <summary>
        /// Данные о зачислениях по брокерам.
        /// </summary>
        public ObservableCollection<TransferBrokerModel> Brokers { get; private set; }

        /// <summary>
        /// Команда на сохранение.
        /// </summary>
        public ICommand SaveCommand { get; }

        /// <summary>
        /// Команда на загрузку.
        /// </summary>
        public ICommand LoadCommand { get; }

        /// <summary>
        /// Команда на добавление брокерского счета.
        /// </summary>
        public Command<TransferBrokerModel> AddBrokerAccountCommand { get; }

        public async Task OnAppearing()
        {
            Title = "Зачисления";
            Brokers.Clear();

            var service = DependencyService.Get<IAuthenticateService>();
            var isAuthenticated = await service.AuthenticateAsync("Увидеть зачисления");
            if (!isAuthenticated)
            {
                IsRefreshing = false;
                await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
                return;
            }

            IsRefreshing = true;
        }

        private async Task SaveAsync()
        {
            try
            {
                var service = DependencyService.Get<ITransferService>();

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                foreach (var broker in Brokers)
                {
                    var amounts = broker.AccountData.Select(ad => new TransferBrokerAccount(ad.Name, ad.Amount)).ToArray();
                    await service.SaveAsync(broker.BrokerName, amounts, cancellation);
                }
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }

            // Перезагружаем данные.
            IsRefreshing = true;
        }

        private async Task LoadAsync()
        {
            IsRefreshing = true;
            Brokers.Clear();

            try
            {
                var service = DependencyService.Get<ITransferService>();

                using var cancelTokenSource = new CancellationTokenSource();
                var cancellation = cancelTokenSource.Token;
                var brokers = await service.GetListAsync(cancellation);
                foreach (var broker in brokers)
                {
                    var data = new TransferBrokerModel(broker.BrokerName);
                    var sum = broker.AccountData.Sum(ad => ad.Sum);
                    data.SumText = NumericUtility.ToCurrencyString(sum, Contracts.Enums.Currency.Rub);
                    data.AccountData = broker.AccountData.Select(ad => new TransferBrokerAccountModel(ad.Name, ad.Sum)).ToArray();
                    Brokers.Add(data);
                }
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        private async void AddBrokerAccountAsync(TransferBrokerModel data)
        {
            string brokerName = data.BrokerName;

            var name = await _messageService.ShowPromptAsync("Добавление счета", "Брокер: " + brokerName);
            if (name == null)
            {
                return;
            }

            if (name.Length > 200)
            {
                await _messageService.ShowAsync("Название счета не должно быть больше 200 символов.");
                return;
            }


            var service = DependencyService.Get<ITransferService>();
            using var cancelTokenSource = new CancellationTokenSource();
            var cancellation = cancelTokenSource.Token;
            await service.AddTransferBrokerAccountAsync(brokerName, name, cancellation);

            // Перезагружаем данные.
            IsRefreshing = true;
        }
    }
}

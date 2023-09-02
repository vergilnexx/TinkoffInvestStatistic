using Infrastructure.Services;
using Plugin.Fingerprint;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffInvestStatistic.Models;
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
        }

        /// <summary>
        /// Данные о зачислениях по брокерам.
        /// </summary>
        public ObservableCollection<TransferBrokerModel> Brokers { get; private set; }

        /// <summary>
        /// Команда на сохранение.
        /// </summary>
        public ICommand SaveCommand { get; private set; }

        /// <summary>
        /// Команда на загрузку.
        /// </summary>
        public ICommand LoadCommand { get; private set; }

        public async Task OnAppearing()
        {
            Title = "Зачисления";
            Brokers.Clear();

            var isAuthenticated = await AuthenticateAsync();
            if (!isAuthenticated)
            {
                IsRefreshing = false;
                await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
                return;
            }

            IsRefreshing = true;
        }

        private async Task<bool> AuthenticateAsync()
        {
            var availability = await CrossFingerprint.Current.IsAvailableAsync();
            if (!availability)
            {
                return false;
            }

            var authResult = await Device.InvokeOnMainThreadAsync(() => CrossFingerprint.Current.AuthenticateAsync(
                new Plugin.Fingerprint.Abstractions.AuthenticationRequestConfiguration("Увидеть зачисления", string.Empty)));
            if (!authResult.Authenticated)
            {
                return false;
            }

            return true;
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
                    await service.SaveAsync(broker.BrokerName, broker.Amount, cancellation);
                }
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }

            // Перезагружаем данные.
            await LoadAsync();
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
                    Brokers.Add(new TransferBrokerModel(broker.BrokerName, broker.Sum));
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
    }
}

using Contracts.Enums;
using Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.Service;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления добавления позиции.
    /// </summary>
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    [QueryProperty(nameof(PositionType), nameof(PositionType))]
    public class AddPositionViewModel : BaseViewModel
    {
        private string ticker;
        private PositionType _positionType;
        private PositionModel _selectedItem;

        public ObservableCollection<PositionModel> Positions { get; }

        /// <summary>
        /// Счет.
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// Тип инструментов.
        /// </summary>
        public int PositionType
        {
            get
            {
                return (int)_positionType;
            }
            set
            {
                _positionType = (PositionType)value;
                Title = "Добавление " + GetPositionTypeForTitle(_positionType);
            }
        }
        public Command<PositionModel> ItemTapped { get; }

        /// <summary>
        /// Тикер.
        /// </summary>
        public string Ticker
        {
            get => ticker;
            set => SetProperty(ref ticker, value);
        }

        public PositionModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnPositionSelected(value);
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        public AddPositionViewModel()
        {
            Title = "Добавление позиции";
            ItemTapped = new Command<PositionModel>(OnPositionSelected);
            Positions = new ObservableCollection<PositionModel>();
        }

        public async Task TickerChangedAsync()
        {
            if (Ticker == null || Ticker.Length < 3)
            {
                return;
            }

            try
            {
                var service = DependencyService.Get<IPositionService>();
                var positions = await service.GetPositionByTickerAsync(_positionType, Ticker);
                Positions.Clear();
                foreach (var position in positions)
                {
                    var item = new PositionModel(position.Figi, _positionType);
                    item.Name = position.Name;
                    item.Ticker = position.Ticker;
                    Positions.Add(item);
                }
                OnPropertyChanged(nameof(Positions));
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        private static string GetPositionTypeForTitle(PositionType positionType)
        {
            return positionType switch
            {
                Contracts.Enums.PositionType.Stock => "акции",
                Contracts.Enums.PositionType.Currency => "валюты",
                Contracts.Enums.PositionType.Bond => "облигации",
                Contracts.Enums.PositionType.Etf => "фонда",
                _ => throw new ArgumentOutOfRangeException(nameof(positionType)),
            };
        }

        async void OnPositionSelected(PositionModel item)
        {
            if (item == null)
            {
                return;
            }

            var service = DependencyService.Get<IPositionService>();
            await service.AddPlannedPositionAsync(AccountId, item.Type, item.Figi, item.Name, item.Ticker);

            await Shell.Current.Navigation.PopAsync();
        }
    }
}

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
    /// <summary>
    /// Модель представления словаря секторов.
    /// </summary>
    public class SectorDictionaryViewModel : BaseViewModel
    {
        private SectorModel _selectedItem;

        public ObservableCollection<SectorModel> Sectors { get; }
        public Command LoadSectorsCommand { get; }
        public Command AddSectorCommand { get; }
        public Command<SectorModel> SectorTapped { get; }

        /// <summary>
        /// 
        /// </summary>
        public SectorDictionaryViewModel()
        {
            Title = "Секторы";
            Sectors = new ObservableCollection<SectorModel>();
            LoadSectorsCommand = new Command(async () => await ExecuteLoadSectorsCommand());

            SectorTapped = new Command<SectorModel>(OnSectorsSelected);

            AddSectorCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadSectorsCommand()
        {
            IsBusy = true;

            try
            {
                Sectors.Clear();
                var service = DependencyService.Get<ISectorService>();
                var sectors = await service.GetSectorsAsync();
                foreach (var sector in sectors)
                {
                    var model = new SectorModel(sector.Id, sector.Name);
                    Sectors.Add(model);
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

        public SectorModel SelectedItem
        {
            get => _selectedItem;
            set
            {
                SetProperty(ref _selectedItem, value);
                OnSectorsSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(SectorDetailPage));
        }

        async void OnSectorsSelected(SectorModel sector)
        {
            if (sector == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(SectorDetailPage)}?{nameof(SectorDetailViewModel.SectorId)}={sector.Id}");
        }
    }
}

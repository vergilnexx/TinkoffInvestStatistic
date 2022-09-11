using Contracts;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    /// <summary>
    /// Модель представления деталей сектора.
    /// </summary>
    public class SectorDetailViewModel : BaseViewModel, IQueryAttributable
    {
        private string name;

        public SectorDetailViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !string.IsNullOrWhiteSpace(name);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        public int SectorId { get; set; }

        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            if(query.Keys.Contains(nameof(SectorId)))
            {
                int.TryParse(query[nameof(SectorId)], out int sectorId);
                SectorId = sectorId;
            }
        }

        public void OnAppearing()
        {
            LoadSector(SectorId);
        }

        public async void LoadSector(int sectorId)
        {
            try
            {
                var service = DependencyService.Get<ISectorService>();
                var sector = await service.GetSectorAsync(sectorId);
                Name = sector.Name;
            }
            catch (Exception ex)
            {
                await _messageService.ShowAsync(ex.Message);
                Debug.WriteLine(ex);
            }
        }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            var sector = new Sector(SectorId, Name);

            var service = DependencyService.Get<ISectorService>();
            if(SectorId == default)
            {
                await service.AddSectorAsync(sector);
            }
            else
            {
                await service.UpdateSectorAsync(sector);
            }

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

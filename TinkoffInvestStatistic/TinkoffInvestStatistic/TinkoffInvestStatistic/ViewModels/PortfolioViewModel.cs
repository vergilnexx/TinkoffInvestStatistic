using Infrastructure.Helpers;
using Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Models;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    [QueryProperty(nameof(AccountTitle), nameof(AccountTitle))]
    public class PortfolioViewModel : BaseViewModel
    {
        private string _accountId;

        public ObservableCollection<GroupedPositionsModel> Positions { get; }
        public Command LoadPositionsCommand { get; }

        public PortfolioViewModel()
        {
            Positions = new ObservableCollection<GroupedPositionsModel>();
            LoadPositionsCommand = new Command(async () => await LoadPositionsByAccountId());
        }

        public string AccountId
        {
            get
            {
                return _accountId;
            }
            set
            {
                _accountId = value;
            }
        }

        public string AccountTitle
        {
            get
            {
                return Title;
            }
            set
            {
                Title = value;
            }
        }

        async Task LoadPositionsByAccountId()
        {
            IsBusy = true;

            try
            {
                Positions.Clear();
                var service = DependencyService.Get<IPositionService>();
                var grouped = await service.GetGroupedPositionsAsync(AccountId);
                foreach (var group in grouped)
                {
                    var items = group.Value.Select(p => new PositionModel
                    {
                        Name = p.Name,
                        Type = p.Type.GetDescription(),
                        Balance = p.Balance,
                        Blocked = p.Blocked,
                        Ticker = p.Ticker,
                        Lots = p.Lots
                    }).ToList();
                    var model = new GroupedPositionsModel(group.Key.GetDescription(), items);

                    Positions.Add(model);
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
        }
    }
}

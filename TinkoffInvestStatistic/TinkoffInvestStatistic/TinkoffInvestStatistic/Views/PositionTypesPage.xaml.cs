using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [QueryProperty(nameof(AccountId), nameof(AccountId))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionTypesPage : BaseDataPage
    {
        PositionTypeViewModel _viewModel;

        public string AccountId
        {
            get => _viewModel?.AccountId ?? string.Empty;
            set
            {
                if (_viewModel != null)
                {
                    _viewModel.AccountId = Uri.UnescapeDataString(value ?? string.Empty);
                }
            }
        }

        public PositionTypesPage()
        {
            InitializeComponent();

            _viewModel = new PositionTypeViewModel();
            BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        public override void RefreshView()
        {
            _viewModel.IsRefreshing = true;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        public void SetAccountId(string? accountId)
        {
            _viewModel.AccountId = accountId ?? string.Empty;
        }

        private void PlanPercent_Completed(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.SavePlanPercentAsync());
        }
    }
}
using System;
using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using TinkoffInvestStatistic.Views.Base;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyPage : BaseDataPage
    {
        CurrencyViewModel _viewModel;

        public CurrencyPage()
        {
            InitializeComponent();
            _viewModel = new CurrencyViewModel();
            BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        public override void RefreshView()
        {
            _viewModel.IsRefreshing = true;
        }

        /// <inheritdoc/>
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
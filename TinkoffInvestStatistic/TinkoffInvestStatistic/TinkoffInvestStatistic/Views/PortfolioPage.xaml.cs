using System.ComponentModel;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class PortfolioPage : ContentPage
    {
        PortfolioViewModel _viewModel = DependencyService.Resolve<PortfolioViewModel>();

        public PortfolioPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
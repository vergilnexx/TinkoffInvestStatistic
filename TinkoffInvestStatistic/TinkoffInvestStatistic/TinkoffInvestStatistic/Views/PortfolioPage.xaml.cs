using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class PortfolioPage : ContentPage
    {
        PortfolioViewModel _viewModel = new PortfolioViewModel();

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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _viewModel = null;
        }
    }
}
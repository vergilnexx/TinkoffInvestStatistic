using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    /// <summary>
    /// Страница счетов.
    /// </summary>
    public partial class AccountsPage : ContentPage
    {
        AccountsViewModel _viewModel = new AccountsViewModel();

        public AccountsPage()
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
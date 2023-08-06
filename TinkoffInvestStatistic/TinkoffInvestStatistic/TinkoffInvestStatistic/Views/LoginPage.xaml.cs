using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel _viewModel = new LoginViewModel();

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.OnAppearing();
        }
    }
}
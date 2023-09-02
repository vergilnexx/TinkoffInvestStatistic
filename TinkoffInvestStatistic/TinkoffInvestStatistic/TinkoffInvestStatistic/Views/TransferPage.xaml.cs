using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    /// <summary>
    /// Страница зачислений.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferPage : ContentPage
    {
        TransferViewModel _viewModel;

        public TransferPage()
        {
            InitializeComponent();
            _viewModel = new TransferViewModel();
            BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
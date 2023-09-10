using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    /// <summary>
    /// Страница уведомлений по зачислениям.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransferNotificationPage : ContentPage
    {
        TransferNotificationViewModel _viewModel;

        public TransferNotificationPage()
        {
            InitializeComponent();
            _viewModel = new TransferNotificationViewModel();
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
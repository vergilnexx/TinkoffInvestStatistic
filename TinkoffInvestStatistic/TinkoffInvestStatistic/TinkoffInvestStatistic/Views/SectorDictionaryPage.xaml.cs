using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SectorDictionaryPage : ContentPage
    {
        SectorDictionaryViewModel _viewModel;

        public SectorDictionaryPage()
        {
            InitializeComponent();
            BindingContext = _viewModel = new SectorDictionaryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
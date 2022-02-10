using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    /// <summary>
    /// Детали сектора.
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SectorDetailPage : ContentPage
    {
        SectorDetailViewModel _viewModel;

        public SectorDetailPage()
        {
            InitializeComponent();
            _viewModel = new SectorDetailViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
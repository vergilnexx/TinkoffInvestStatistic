using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPositionPage : ContentPage
    {
        private AddPositionViewModel _viewModel = new AddPositionViewModel();

        public AddPositionPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }

        private void TickerChanged(object sender, System.EventArgs e)
        {
            Task.Run(() => _viewModel.TickerChangedAsync());
        }
    }
}
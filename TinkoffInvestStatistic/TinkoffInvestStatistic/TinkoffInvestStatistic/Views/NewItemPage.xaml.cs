using TinkoffInvestStatistic.Models;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class NewItemPage : ContentPage
    {
        NewItemViewModel _viewModel = DependencyService.Resolve<NewItemViewModel>();

        public Item Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
    }
}
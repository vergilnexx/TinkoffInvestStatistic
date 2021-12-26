using System.ComponentModel;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        ItemDetailViewModel _viewModel = DependencyService.Resolve<ItemDetailViewModel>();

        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = _viewModel;
        }
    }
}
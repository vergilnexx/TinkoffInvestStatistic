using System.Threading.Tasks;
using TinkoffInvestStatistic.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TinkoffInvestStatistic.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        SettingViewModel _viewModel;

        public SettingPage()
        {
            InitializeComponent();

            _viewModel = new SettingViewModel();
            BindingContext = _viewModel;
        }

        /// <inheritdoc/>
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }

        /// <summary>
        /// Событие нажатия на настройку: Отображать/скрывать данные при входе.
        /// </summary>
        public async void OnShowHideMoneyCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            await Task.Run(() => _viewModel.SaveOptionAsync(Contracts.Enums.OptionType.IsHideMoney, e.Value.ToString()));
        }
    }
}
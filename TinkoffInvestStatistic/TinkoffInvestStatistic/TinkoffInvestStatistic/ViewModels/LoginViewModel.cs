using System.Threading.Tasks;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await AuthenticateAsync());
        }

        public async Task OnAppearing()
        {
            await AuthenticateAsync();
        }

        private async Task AuthenticateAsync()
        {
            IsRefreshing = true;
            //var service = DependencyService.Get<IAuthenticateService>();
            //var isAuthenticated = await service.AuthenticateAsync("Вход");
            //if (isAuthenticated)
            //{
                await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
            //}
            IsRefreshing = false;
        }
    }
}

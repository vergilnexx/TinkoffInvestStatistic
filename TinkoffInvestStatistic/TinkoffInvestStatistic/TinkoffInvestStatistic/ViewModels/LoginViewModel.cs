using System.Threading.Tasks;
using System;
using TinkoffInvestStatistic.Service;
using TinkoffInvestStatistic.ViewModels.Base;
using TinkoffInvestStatistic.Views;
using Microsoft.Maui.Controls;

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
            try
            {
                var service = DependencyService.Get<IAuthenticateService>();
                if (service == null)
                {
                    return;
                }

                var isAuthenticated = await service.AuthenticateAsync("Вход");
                if (isAuthenticated)
                {
                    await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login auth failed: {ex}");
            }
            IsRefreshing = false;
        }
    }
}

using Plugin.Fingerprint;
using System.Threading.Tasks;
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
            LoginCommand = new Command(async () => await CheckAuthorizationAsync());
        }

        public async Task OnAppearing()
        {
            await CheckAuthorizationAsync();
        }

        private async Task CheckAuthorizationAsync()
        {
            IsRefreshing = true;
            var availability = await CrossFingerprint.Current.IsAvailableAsync();

            if (!availability)
            {
                IsRefreshing = false;
                return;
            }

            var authResult = await Device.InvokeOnMainThreadAsync(() => CrossFingerprint.Current.AuthenticateAsync(
                new Plugin.Fingerprint.Abstractions.AuthenticationRequestConfiguration("Вход", string.Empty)));
            if(authResult.Authenticated)
            {
                // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
                await Shell.Current.GoToAsync($"//{nameof(AccountsPage)}");
            }
            else
            {
                IsRefreshing = false;
            }
        }
    }
}

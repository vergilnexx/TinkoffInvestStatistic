using Plugin.Fingerprint;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TinkoffInvestStatistic.Service
{
    /// <inheritdoc/>
    internal class AuthenticateService : IAuthenticateService
    {
        /// <inheritdoc/>
        public async Task<bool> AuthenticateAsync(string queryName)
        {
            var availability = await CrossFingerprint.Current.IsAvailableAsync();
            if (!availability)
            {
                return false;
            }

            var authResult = await Device.InvokeOnMainThreadAsync(() => CrossFingerprint.Current.AuthenticateAsync(
                new Plugin.Fingerprint.Abstractions.AuthenticationRequestConfiguration(queryName, string.Empty)));
            if (!authResult.Authenticated)
            {
                return false;
            }

            return true;
        }
    }
}
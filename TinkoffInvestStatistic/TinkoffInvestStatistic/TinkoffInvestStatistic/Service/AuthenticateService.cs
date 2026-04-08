using Plugin.Fingerprint;
using Plugin.Fingerprint.Abstractions;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace TinkoffInvestStatistic.Service;

/// <inheritdoc/>
internal class AuthenticateService : IAuthenticateService
{
    /// <inheritdoc/>
    public async Task<bool> AuthenticateAsync(string queryName)
    {
        // Temporary bypass: disable biometric auth and allow login.
        await Task.CompletedTask;
        return true;

        /*
        try
        {
            var fingerprint = CrossFingerprint.Current;
            if (!await fingerprint.IsAvailableAsync(true))
            {
                return false;
            }

            var result = await fingerprint.AuthenticateAsync(
                new AuthenticationRequestConfiguration(queryName, "Подтвердите вход")
                {
                    CancelTitle = "Отмена",
                },
                CancellationToken.None);

            return result.Authenticated;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Biometric auth failed: {ex}");
            return false;
        }
        */
    }
}

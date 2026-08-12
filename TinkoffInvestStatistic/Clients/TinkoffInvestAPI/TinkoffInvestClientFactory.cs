using Infrastructure.Clients;
using System;
using Microsoft.Maui.Controls;

namespace TinkoffInvest
{
    /// <inheritdoc/>
    public class TinkoffInvestClientFactory : ITinkoffInvestClientFactory
    {
        /// <inheritdoc/>
        public IBankBrokerApiClient Get()
        {
            var client = DependencyService.Resolve<CachedTinkoffInvestClient>();
            if (client == null)
            {
                throw new ApplicationException("Не удалось получить клиент для работы с 'Тинькофф инвестиции'");
            }

            return client;
        }
    }
}

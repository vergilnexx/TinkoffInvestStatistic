using Contracts;
using Contracts.Enums;
using System;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="Tinkoff.Trading.OpenApi.Models.Account"/> в <see cref="Account"/>
    /// </summary>
    public class TinkoffAccountToAccountMapper : IMapper<Tinkoff.Trading.OpenApi.Models.Account, Contracts.Account>
    {
        /// <inheritdoc/>
        public Contracts.Account Map(Tinkoff.Trading.OpenApi.Models.Account type)
        {
            var result = new Contracts.Account();
            
            result.ID = type.BrokerAccountId;
            result.Type = MapType(type.BrokerAccountType);

            return result;
        }

        private AccountType MapType(BrokerAccountType brokerAccountType)
        {
            switch (brokerAccountType)
            {
                case BrokerAccountType.Tinkoff:
                    return AccountType.BrokerAccount;
                case BrokerAccountType.TinkoffIis:
                    return AccountType.Iis;
                default:
                    throw new ArgumentOutOfRangeException(nameof(brokerAccountType));
            }
        }
    }
}
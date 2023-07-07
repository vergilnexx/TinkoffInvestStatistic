using System;
using TinkoffInvest.Contracts.Accounts;
using TinkoffInvestStatistic.Contracts;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffContracts = TinkoffInvest.Contracts;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="TinkoffContracts.Accounts.Account"/> в <see cref="Account"/>
    /// </summary>
    public class TinkoffAccountToAccountMapper : IMapper<TinkoffContracts.Accounts.Account, TinkoffInvestStatistic.Contracts.Account>
    {
        /// <inheritdoc/>
        public TinkoffInvestStatistic.Contracts.Account Map(TinkoffContracts.Accounts.Account brokerAccount)
        {
            var result = new TinkoffInvestStatistic.Contracts.Account();
            
            result.ID = brokerAccount.Id;
            result.Name = brokerAccount.Name;
            result.Type = MapType(brokerAccount.AccountType);

            return result;
        }

        private AccountType MapType(TinkoffContracts.Enums.AccountType brokerAccountType)
        {
            return brokerAccountType switch
            {
                TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF => AccountType.BrokerAccount,
                TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF_IIS => AccountType.Iis,
                TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_INVEST_BOX => AccountType.InvestBox,
                _ => throw new ArgumentOutOfRangeException(nameof(brokerAccountType)),
            };
        }
    }
}
using TinkoffInvest.Contracts.Accounts;
using TinkoffInvestStatistic.Contracts;
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
            result.Type = EnumMapper.MapAccountType(brokerAccount.AccountType);

            return result;
        }
    }
}
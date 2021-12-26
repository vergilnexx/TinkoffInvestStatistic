using Contracts;
using Contracts.Enums;
using Infrastructure.Clients;
using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class PositionService : IPositionService
    {
        /// <inheritdoc/>
        public async Task<Dictionary<PositionType, Position[]>> GetGroupedPositionsAsync(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var position = await bankBrokerClient.GetPositionsAsync(accountId);
            return position.GroupBy(p => p.Type).ToDictionary(g => g.Key, g => g.ToArray());
        }
    }
}

using Infrastructure.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;

namespace Services
{
    /// <inheritdoc/>
    public class TransferService : ITransferService
    {
        /// <summary>
        /// Список джоступных брокеров.
        /// </summary>
        public readonly static IReadOnlyCollection<string> BrokersName = new[]
        {
            "БКС",
            "ВТБ",
            "Тинькофф"
        };

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TransferBroker>> GetListAsync(CancellationToken cancellation)
        {
            var result = new List<TransferBroker>();

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var brokers = await dataAccessService.GetTransfersAsync(cancellation);
            foreach (var broker in brokers)
            {
                var dto = new TransferBroker(broker.BrokerName);
                var accountDatas = await dataAccessService.GetTransfersBrokerAccountsAsync(broker.Id, cancellation);
                dto.AccountData = accountDatas.Select(ad => new TransferBrokerAccount(ad.Name, ad.Sum)).ToArray();
                result.Add(dto);
            }
            return result;
        }

        /// <inheritdoc/>
        public async Task SaveAsync(string brokerName, IReadOnlyCollection<TransferBrokerAccount> amounts, CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var broker = await dataAccessService.GetTransferAsync(brokerName, cancellation);
            if (broker == null)
            {
                await dataAccessService.SaveTransferAsync(brokerName, cancellation);
                broker = await dataAccessService.GetTransferAsync(brokerName, cancellation);
            }

            var accountDatas = await dataAccessService.GetTransfersBrokerAccountsAsync(broker.Id, cancellation);
            foreach (var account in accountDatas)
            {
                var amount = amounts.FirstOrDefault(a => a.Name == account.Name);
                var sum = account.Sum + (amount?.Sum ?? 0m);
                await dataAccessService.SaveTransferBrokerAccountAsync(account.Id, sum, cancellation);
            }
        }

        /// <inheritdoc/>
        public async Task AddTransferBrokerAccountAsync(string brokerName, string name, CancellationToken cancellation)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.AddTransferBrokerAccountAsync(brokerName, name, cancellation);
        }
    }
}

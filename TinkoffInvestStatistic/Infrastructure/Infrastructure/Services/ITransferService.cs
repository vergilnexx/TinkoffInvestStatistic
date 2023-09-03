using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис работы с зачислениями.
    /// </summary>
    public interface ITransferService
    {
        /// <summary>
        /// Возвращает список зачислений по брокерам.
        /// </summary>
        /// <returns>Список зачислений по брокерам.</returns>
        public Task<IReadOnlyCollection<TransferBroker>> GetListAsync(CancellationToken cancellation);

        /// <summary>
        /// Сохранение данных 
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        /// <param name="amounts">Данные зачислений по счетам.</param>
        /// <param name="cancellation">Токен отмены.</param>
        public Task SaveAsync(string brokerName, IReadOnlyCollection<TransferBrokerAccount> amounts, CancellationToken cancellation);

        /// <summary>
        /// Добавление счета брокеру.
        /// </summary>
        /// <param name="brokerName">Наименование брокера.</param>
        /// <param name="name">Наименование счета.</param>
        /// <param name="cancellation">Токен отмены.</param>
        Task AddTransferBrokerAccountAsync(string brokerName, string name, CancellationToken cancellation);
    }
}

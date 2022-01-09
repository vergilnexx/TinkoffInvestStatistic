using Contracts;
using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    /// <summary>
    /// Сервис по работе с инструментами.
    /// </summary>
    public interface IInstrumentService
    {
        /// <summary>
        /// Возвращеает список типов инструментов.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <returns>Список инструментов.</returns>
        Task<IReadOnlyCollection<Instrument>> GetPositionTypes(string accountNumber);

        /// <summary>
        /// Сохраняет данные планируемого процента для счета.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные для сохранения.</param>
        Task SavePlanPercents(string accountNumber, InstrumentData[] data);
    }
}

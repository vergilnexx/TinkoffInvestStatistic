using Contracts;
using Contracts.Enums;
using Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Services
{
    /// <summary>
    /// Сервис хранения данных
    /// </summary>
    public sealed class DataStorageService
    {
        /// <summary>
        /// Данные по счетам.
        /// </summary>
        public AccountData[]? Accounts { get; private set; }

        /// <summary>
        /// Экземпляр.
        /// </summary>
        public static DataStorageService Instance { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <exception cref="Exception"></exception>
        public DataStorageService()
        {
            if (Instance != null)
            {
                throw new Exception("Only one instance of DataStorageService is allowed!");
            }
            else
            {
                Instance = this;
            }
        }

        /// <summary>
        /// Загрузка данных.
        /// </summary>
        public async Task LoadAccountData()
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            Accounts = await dataAccessService.GetAccountDataAsync();
        }

        /// <summary>
        /// Объединение данных полученных из внешнего источника с локальными данными.
        /// </summary>
        /// <param name="externalAccounts">Внешние данные по счетам.</param>
        /// <returns>Данные по счетам.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<Account[]> MergeAccountData(IReadOnlyCollection<Account> externalAccounts)
        {
            if (externalAccounts == null)
            {
                throw new ArgumentNullException(nameof(externalAccounts), "Полученные данные не могут быть неопределенными.");
            }

            var result = (Accounts ?? Array.Empty<AccountData>()).ToList();
            foreach (var externalAccount in externalAccounts)
            {
                var account = result.FirstOrDefault(x => x.Number == externalAccount.ID);
                if(account == null)
                {
                    account = new AccountData(externalAccount.ID);
                    result.Add(account);
                }
            }

            Accounts = result.ToArray();
            await SaveAccountDataAsync();
            return externalAccounts.ToArray();
        }

        /// <summary>
        /// Возвращает заполненные данные по инструментам.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionTypes">Типы инструментов.</param>
        /// <returns>Заполненные данные по инструментам</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public async Task<Instrument[]> MergePositionTypesData(string accountNumber, PositionType[] positionTypes)
        {
            if (positionTypes == null)
            {
                throw new ArgumentNullException(nameof(positionTypes), "Полученные данные не могут быть неопределенными.");
            }

            if (Accounts == null)
            {
                throw new ApplicationException("Данные по счетам не могут быть неопределенными при получении данных об инструментах по счету - " + accountNumber);
            }

            var account = Accounts.FirstOrDefault(a => a.Number == accountNumber);
            if (account == null)
            {
                throw new ApplicationException("Данные по счету " + accountNumber + " не найдены");
            }

            var result = new List<Instrument>();
            var data = new List<InstrumentData>();
            foreach (var type in positionTypes)
            {
                var instrumentData = account.Instruments.FirstOrDefault(i => i.Type == type);
                if(instrumentData == null)
                {
                    instrumentData = new InstrumentData(type);
                }

                data.Add(instrumentData);
                result.Add(new Instrument(type) { PlanPercent = instrumentData?.PlanPercent ?? 0 });
            }
            
            account.Instruments = data.ToArray();
            await SaveAccountDataAsync();
            return result.ToArray();
        }

        /// <summary>
        /// Возвращает заполненные данные по позициям счета.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positions">Позиции.</param>
        /// <returns>Список заполненных позиций.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public async Task<IEnumerable<Position>> MergePositionData(string accountNumber, IEnumerable<Position> positions)
        {
            if (positions == null || positions.Count() == 0)
            {
                throw new ArgumentNullException(nameof(positions), "Полученные данные не могут быть неопределенными.");
            }

            if (Accounts == null)
            {
                throw new ApplicationException("Данные по счетам не могут быть неопределенными при получении данных об инструментах по счету - " + accountNumber);
            }

            var account = Accounts.FirstOrDefault(a => a.Number == accountNumber);
            if (account == null)
            {
                throw new ApplicationException("Данные по счету " + accountNumber + " не найдены");
            }

            var result = new List<PositionData>();
            foreach (var position in positions)
            {
                var positionData = account.Positions.FirstOrDefault(i => i.Figi == position.Figi);
                if (positionData == null)
                {
                    positionData = new PositionData(position.Figi, position.Type);
                }
                else
                {
                    position.PlanPercent = positionData.PlanPercent;
                }
                result.Add(positionData);
            }

            account.Positions = result.ToArray();
            await SaveAccountDataAsync();
            return positions.ToArray();
        }

        /// <summary>
        /// Сохраняет данные по инструментам по конкретному счету.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные по инструментам.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public Task SetPositionTypesData(string accountNumber, InstrumentData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var account = Accounts.FirstOrDefault(a => a.Number == accountNumber);
            if (account == null)
            {
                throw new ApplicationException("Данные по счету " + accountNumber + " не найдены");
            }

            account.Instruments = data;
            return SaveAccountDataAsync();
        }

        /// <summary>
        /// Сохраняет данные позиций по конкретному счету.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="data">Данные позиций.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        public Task SetPositionData(string accountNumber, PositionData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var account = Accounts.FirstOrDefault(a => a.Number == accountNumber);
            if (account == null)
            {
                throw new ApplicationException("Данные по счету " + accountNumber + " не найдены");
            }

            // Заменяем только то, что изменяется, остальные оставляем без изменения.
            var changedPositionType = data.FirstOrDefault().Type;
            var notReplacedPositions = account.Positions.Where(a => a.Type != changedPositionType);
            account.Positions = notReplacedPositions.Union(data).ToArray();
            return SaveAccountDataAsync();
        }

        /// <summary>
        /// Сохранение данных.
        /// </summary>
        private async Task SaveAccountDataAsync()
        {
            if(Accounts == null)
            {
                return;
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.SaveAccountDataAsync(Accounts);
        }
    }
}

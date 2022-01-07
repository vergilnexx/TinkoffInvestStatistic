using Contracts;
using Domain;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
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
        /// <exception cref="ArgumentNullException">Ошибка при неверно заданном параметре <see cref="externalAccounts"/></exception>
        public async Task<Account[]> MergeAccountData(IReadOnlyCollection<Account> externalAccounts)
        {
            if (externalAccounts == null)
            {
                throw new ArgumentNullException(nameof(externalAccounts), "Данные, полученные не могут быть неопределенными.");
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
        /// Сохранение данных.
        /// </summary>
        /// <param name="data">Данные.</param>
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

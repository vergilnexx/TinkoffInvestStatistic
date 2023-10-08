using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using Infrastructure.Clients;
using Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinkoffInvestStatistic.Contracts;
using Xamarin.Forms;
using Infrastructure.Helpers;

namespace Services
{
    /// <inheritdoc/>
    public class PositionService : IPositionService
    {
        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Position>> GetPositionsByTypeAsync(string accountNumber, PositionType positionType)
        {
            var bankBrokerClientFactory = DependencyService.Resolve<ITinkoffInvestClientFactory>();
            var bankBrokerClient = bankBrokerClientFactory.Get();
            var portfolio = await bankBrokerClient.GetAccountsFullDataAsync(accountNumber);

            // Получаем курсы валют.
            var currencies = portfolio.Positions
                                .Where(p => p.Type == PositionType.Currency)
                                .Select(p => new { Currency = EnumHelper.GetCurrencyByFigi(p.Figi), Sum = p.CurrentPrice?.Sum ?? 0 })
                                .Where(p => p.Currency != Currency.Rub)
                                .Select(p => KeyValuePair.Create(p.Currency!.Value, p.Sum))
                                .Union(new[] { KeyValuePair.Create(Currency.Rub, 1m) })
                                .ToList();

            var positions = portfolio.Positions.Where(p => p.Type == positionType);
            foreach (var position in positions)
            {
                if (positionType == PositionType.Currency)
                {
                    position.Name = EnumHelper.GetCurrencyByFigi(position.Figi).ToString();
                }
                else
                {
                    var positionData = await bankBrokerClient.FindPositionByFigiAsync(position.Figi, positionType);
                    position.Name = positionData.Name;
                    position.Ticker = positionData.Ticker;
                }

                // Если цена не в рублях рассчитываем по текущему курсу.
                if (position.Currency != Currency.Rub)
                {
                    var currency = currencies.Find(c => c.Key == position.Currency);
                    var currencySum = currency.Value;

                    position.Sum = position.SumInCurrency * currencySum;
                    position.DifferenceSum = (position.ExpectedYield?.Sum ?? 0) * currencySum;
                }
                else
                {
                    position.Sum = position.SumInCurrency;
                    position.DifferenceSum = position.ExpectedYield?.Sum ?? 0;
                }
            }

            var plannedPositions = await GetPlannedPositionsAsync(accountNumber, positionType);
            var existedPositions = positions.Select(p => p.Figi).ToArray();
            plannedPositions = plannedPositions.Where(p => !Array.Exists(existedPositions, ef => p.Figi == ef)).ToArray(); // отсекаем те, которые уже куплены.
            positions = positions.Union(plannedPositions).ToArray();

            positions = await MergePositionDataAsync(accountNumber, positionType, positions.ToArray());

            return positions.ToArray();
        }

        /// <inheritdoc/>
        public Task SavePlanPercents(string accountNumber, PositionType positionType, PositionData[] data)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (data == null || data.Length == 0)
            {
                throw new ArgumentNullException(nameof(data), "Полученные данные не могут быть неопределенными.");
            }

            var isAnotherType = Array.Exists(data, a => a.Type == positionType);
            if (!isAnotherType)
            {
                throw new ApplicationException("Попытка изменить позиции другого типа.");
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            return dataAccessService.SavePositionsDataAsync(accountNumber, data);
        }

        /// <inheritdoc/>
        public async Task AddPlannedPositionAsync(string accountNumber, PositionType type, string figi, string name, string ticker)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            await dataAccessService.AddPlannedPositionAsync(accountNumber, type, figi, name, ticker);
        }

        /// <summary>
        /// Возвращает планируемые для покупки позиции.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positionType">Тип позиций.</param>
        /// <returns>Планируемые для покупки позиции.</returns>
        private async Task<IReadOnlyCollection<Position>> GetPlannedPositionsAsync(string accountNumber, PositionType positionType)
        {
            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var positions = await dataAccessService.GetPlannedPositionsAsync(accountNumber, positionType);
            return positions.Select(p => new Position(p.Figi, positionType, p.Name, default, Currency.Rub) { Ticker = p.Ticker }).ToArray();
        }

        /// <summary>
        /// Возвращает заполненные данные по позициям счета.
        /// </summary>
        /// <param name="accountNumber">Номер счета.</param>
        /// <param name="positions">Позиции.</param>
        /// <returns>Список заполненных позиций.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ApplicationException"></exception>
        private async Task<IEnumerable<Position>> MergePositionDataAsync(string accountNumber, PositionType positionType, IReadOnlyCollection<Position> positions)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                throw new ArgumentNullException(nameof(accountNumber), "Полученные данные не могут быть неопределенными.");
            }

            if (positions == null || !positions.Any())
            {
                return Enumerable.Empty<Position>();
            }

            var dataAccessService = DependencyService.Resolve<IDataStorageAccessService>();
            var positionEntities = await dataAccessService.GetPositionsAsync(accountNumber, positionType);

            var result = new List<PositionData>();
            foreach (var position in positions)
            {
                var positionData = positionEntities.FirstOrDefault(i => i.Figi == position.Figi);
                if (positionData == null)
                {
                    positionData = new PositionData(accountNumber, position.Figi, position.Type);
                }
                else
                {
                    position.PlanPercent = positionData.PlanPercent;
                }
                result.Add(positionData);
            }

            return positions.ToArray();
        }
    }
}

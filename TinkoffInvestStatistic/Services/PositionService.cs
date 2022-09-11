using Contracts;
using Contracts.Enums;
using Domain;
using Infrastructure.Clients;
using Infrastructure.Services;
using Services.Workaround;
using System;
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
        public async Task<IReadOnlyCollection<Position>> GetPositionsByTypeAsync(string accountId, PositionType positionType)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var positions = (await bankBrokerClient.GetAccountPositionsAsync(accountId)).Where(p => p.Type == positionType);
            var currencies = await bankBrokerClient.GetCurrenciesAsync();

            foreach (var position in positions)
            {
                position.SumInCurrency = position.PositionCount * (position.AveragePositionPrice?.Sum ?? 0) + 
                                            (position.ExpectedYield?.Sum ?? 0);

                // Если цена не в рублях рассчитываем по текущему курсу.
                if (position.AveragePositionPrice?.Currency != Currency.Rub)
                {
                    decimal? currencySum;
                    var currency = currencies.FirstOrDefault(c => position.AveragePositionPrice?.Currency == c.Currency);
                    if (currency == null)
                    {
                        currencySum = WorkaroundUtils.GetCurrencySumInRubbles(currencies, position.AveragePositionPrice?.Currency);
                        if (currencySum == null)
                        {
                            throw new ApplicationException("Не найдена валюта типа: " + position.AveragePositionPrice?.Currency);
                        }
                    }
                    else
                    {
                        currencySum = currency.Sum;
                    }

                    position.Sum = position.SumInCurrency * currencySum.Value;
                    position.DifferenceSum = position.ExpectedYield.Sum * currencySum.Value;
                }
                else
                {
                    position.Sum = position.SumInCurrency;
                    position.DifferenceSum = position.ExpectedYield.Sum;
                }
            }

            if(positionType == PositionType.Currency)
            {
                // Добавляем данные про кэш в рублях.
                var rubles = await AddFiatRubles(accountId, bankBrokerClient, positions);
                positions = positions.Union(rubles).ToArray();
            }

            var plannedPositions = await DataStorageService.Instance.GetPlannedPositionsAsync(accountId, positionType);
            var existedPositions = positions.Select(p => p.Figi).ToArray();
            plannedPositions = plannedPositions.Where(p => !existedPositions.Any(ef => p.Figi == ef)).ToArray(); // отсекаем те, которые уже куплены.
            positions = positions.Union(plannedPositions).ToArray();

            positions = await DataStorageService.Instance.MergePositionData(accountId, positionType, positions.ToArray());

            return positions.ToArray();
        }

        private static async Task<IEnumerable<Position>> AddFiatRubles(string accountId, IBankBrokerApiClient bankBrokerClient, IEnumerable<Position> positions)
        {
            var fiatPositions = await bankBrokerClient.GetFiatPositionsAsync(accountId);
            var rubles = fiatPositions
                .Where(fp => fp.Currency == Currency.Rub)
                .Select(fp => new Position(string.Empty, PositionType.Currency, "Рубль", fp.Sum));
            return rubles;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<Position>> GetPositionByTickerAsync(PositionType positionType, string ticker)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var positions = await bankBrokerClient.FindPositionsAsync(ticker);
            return positions
                    .Where(p => p.Type == positionType)
                    .ToArray();
        }

        /// <inheritdoc/>
        public async Task<decimal> GetPositionsSumAsync(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            IEnumerable<Position?> positions = await bankBrokerClient.GetAccountPositionsAsync(accountId);
            positions = positions.Where(p => p.Type != PositionType.Currency);
            var fiatPositions =  await bankBrokerClient.GetFiatPositionsAsync(accountId);
            var currencies = await bankBrokerClient.GetCurrenciesAsync();
            var result = GetSumByPositions(positions.ToArray(), fiatPositions, currencies);
            return result ?? 0;
        }

        /// <inheritdoc/>
        public async Task<decimal> GetPositionsSumAsync(string accountId, PositionType positionType)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            IEnumerable<Position?> positions = await bankBrokerClient.GetAccountPositionsAsync(accountId);
            var currencies = await bankBrokerClient.GetCurrenciesAsync();
            decimal result;
            if(positionType == PositionType.Currency)
            {
                var fiatPositions = await bankBrokerClient.GetFiatPositionsAsync(accountId);
                result = GetSumByPositions(Array.Empty<Position>(), fiatPositions, currencies) ?? 0;
            }
            else
            {
                positions = positions.Where(p => p.Type == positionType);
                result = GetSumByPositions(positions.ToArray(), Array.Empty<CurrencyMoney>(), currencies) ?? 0;
            }
            return result;
        }

        /// <inheritdoc/>
        public Task SavePlanPercents(string accountId, PositionType positionType, PositionData[] data)
        {
            return DataStorageService.Instance.SavePositionDataAsync(accountId, positionType, data);
        }

        /// <inheritdoc/>
        public Task AddPlannedPositionAsync(string accountId, PositionType type, string figi, string name, string ticker)
        {
            return DataStorageService.Instance.AddPlannedPositionAsync(accountId, type, figi, name, ticker);
        }

        protected decimal? GetSumByPositions(
            IReadOnlyCollection<Position> positions, 
            IReadOnlyCollection<CurrencyMoney> fiatPositions, 
            IReadOnlyCollection<CurrencyMoney> currencies)
        {
            var sum = 0m;
            var positionsByCurrency = positions.GroupBy(p => p.AveragePositionPrice?.Currency);
            foreach (var group in positionsByCurrency)
            {
                var sumInCurrency = group.Sum(p => p.PositionCount * (p.AveragePositionPrice?.Sum ?? 0) + (p.ExpectedYield?.Sum ?? 0));
                if(group.Key != Currency.Rub)
                {
                    decimal? currencySum;
                    var currency = currencies.FirstOrDefault(c => group.Key == c.Currency);
                    if (currency == null)
                    {
                        currencySum = WorkaroundUtils.GetCurrencySumInRubbles(currencies, group.Key);
                        if (currencySum == null)
                        {
                            throw new ApplicationException("Не найдена валюта типа: " + group.Key);
                        }
                    }
                    else
                    {
                        currencySum = currency.Sum;
                    }

                    sumInCurrency *= currencySum.Value;
                }

                sum += sumInCurrency;
            }

            foreach (var fiatPosition in fiatPositions)
            {
                if (fiatPosition.Currency != Currency.Rub)
                {
                    decimal? currencySum;
                    var currency = currencies.FirstOrDefault(c => fiatPosition.Currency == c.Currency);
                    if (currency == null)
                    {
                        currencySum = WorkaroundUtils.GetCurrencySumInRubbles(currencies, fiatPosition.Currency);
                        if (currencySum == null)
                        {
                            throw new ApplicationException("Не найдена валюта типа: " + fiatPosition.Currency);
                        }
                    }
                    else
                    {
                        currencySum = currency.Sum;
                    }

                    sum += fiatPosition.Sum * currencySum.Value;
                }
                else
                {
                    sum += fiatPosition.Sum;
                }
            }

            return sum;
        }

    }
}

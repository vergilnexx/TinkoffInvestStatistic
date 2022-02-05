using Contracts;
using Contracts.Enums;
using Domain;
using Infrastructure.Clients;
using Infrastructure.Services;
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
        public async Task<IReadOnlyCollection<Position>> GetGroupedPositionsAsync(string accountId, PositionType positionType)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            IEnumerable<Position> positions = await bankBrokerClient.GetPositionsAsync(accountId);
            var currencies = await bankBrokerClient.GetCurrenciesAsync();

            foreach (var position in positions)
            {
                position.SumInCurrency = position.PositionCount * (position.AveragePositionPrice?.Value ?? 0) + (position.ExpectedYield?.Value ?? 0);

                // Если цена не в рублях рассчитываем по текущему курсу.
                if (position.AveragePositionPrice?.Currency != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => position.AveragePositionPrice?.Currency == c.Currency);
                    if (currency == null)
                    {
                        throw new ApplicationException("Не найдена валюта типа: " + position.AveragePositionPrice?.Currency);
                    }

                    position.Sum = position.SumInCurrency * currency.Value;
                    position.DifferenceSum = position.ExpectedYield.Value * currency.Value;
                }
                else
                {
                    position.Sum = position.SumInCurrency;
                    position.DifferenceSum = position.ExpectedYield.Value;
                }
            }

            // Добавляем данные про кэш в рублях.
            var rubles = await AddFiatRubles(accountId, bankBrokerClient, positions);
            positions = positions.Union(rubles).ToArray();

            positions = await DataStorageService.Instance.MergePositionData(accountId, positions);

            return positions.Where(p => p.Type == positionType).ToArray();
        }

        private static async Task<IEnumerable<Position>> AddFiatRubles(string accountId, IBankBrokerApiClient bankBrokerClient, IEnumerable<Position> positions)
        {
            var fiatPositions = await bankBrokerClient.GetFiatPositionsAsync(accountId);
            var rubles = fiatPositions
                .Where(fp => fp.Currency == Currency.Rub)
                .Select(fp => new Position(string.Empty, PositionType.Currency, "Рубль", fp.Value));
            return rubles;
        }

        /// <inheritdoc/>
        public async Task<decimal> GetPositionsSumAsync(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            IEnumerable<Position?> positions = await bankBrokerClient.GetPositionsAsync(accountId);
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
            IEnumerable<Position?> positions = await bankBrokerClient.GetPositionsAsync(accountId);
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
        public Task SavePlanPercents(string accountId, PositionData[] data)
        {
            return DataStorageService.Instance.SetPositionData(accountId, data);
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
                var sumInCurrency = group.Sum(p => p.PositionCount * p.AveragePositionPrice.Value + p.ExpectedYield.Value);
                if(group.Key != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => group.Key == c.Currency);
                    if (currency == null)
                    {
                        throw new ApplicationException("Не найдена валюта типа: " + group.Key);
                    }

                    sumInCurrency *= currency.Value;
                }

                sum += sumInCurrency;
            }

            foreach (var fiatPosition in fiatPositions)
            {
                if (fiatPosition.Currency != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => fiatPosition.Currency == c.Currency);
                    if (currency == null)
                    {
                        throw new ApplicationException("Не найдена валюта типа: " + fiatPosition.Currency);
                    }

                    sum += fiatPosition.Value * currency.Value;
                }
                else
                {
                    sum += fiatPosition.Value;
                }
            }

            return sum;
        }
    }
}

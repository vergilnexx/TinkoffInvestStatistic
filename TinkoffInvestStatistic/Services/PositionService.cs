using TinkoffInvestStatistic.Contracts.Enums;
using Domain;
using Infrastructure.Clients;
using Infrastructure.Services;
using Services.Workaround;
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
        public async Task<IReadOnlyCollection<Position>> GetPositionsByTypeAsync(string accountId, PositionType positionType)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var portfolio = await bankBrokerClient.GetAccountsFullDataAsync(accountId);

            // Получаем курсы валют.
            var currencies = portfolio.Positions
                                .Where(p => p.Type == PositionType.Currency)
                                .Select(p => new { Currency = EnumHelper.GetCurrencyByFigi(p.Figi), Sum = p.CurrentPrice.Sum })
                                .Where(p => p.Currency != Currency.Rub)
                                .Select(p => KeyValuePair.Create(p.Currency!.Value, p.Sum))
                                .Union(new[] { KeyValuePair.Create(Currency.Rub, 1m) })
                                .ToList();

            var positions = portfolio.Positions.Where(p => p.Type == positionType);
            foreach (var position in positions)
            {
                if (positionType == PositionType.Currency)
                {
                    position.Name = (EnumHelper.GetCurrencyByFigi(position.Figi) ?? Currency.Rub).ToString();
                }
                else
                {
                    var positionData = await bankBrokerClient.FindPositionByFigiAsync(position.Figi, positionType);
                    position.Name = positionData.Name;
                    position.Ticker = positionData.Ticker;
                }
                position.SumInCurrency = position.SumInCurrency;

                // Если цена не в рублях рассчитываем по текущему курсу.
                if (position.Currency != Currency.Rub)
                {
                    var currency = currencies.FirstOrDefault(c => c.Key == position.Currency);
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

            var plannedPositions = await DataStorageService.Instance.GetPlannedPositionsAsync(accountId, positionType);
            var existedPositions = positions.Select(p => p.Figi).ToArray();
            plannedPositions = plannedPositions.Where(p => !existedPositions.Any(ef => p.Figi == ef)).ToArray(); // отсекаем те, которые уже куплены.
            positions = positions.Union(plannedPositions).ToArray();

            positions = await DataStorageService.Instance.MergePositionData(accountId, positionType, positions.ToArray());

            return positions.ToArray();
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

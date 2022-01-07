using Contracts;
using Contracts.Enums;
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
        public async Task<Dictionary<PositionType, Position[]>> GetGroupedPositionsAsync(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var positions = await bankBrokerClient.GetPositionsAsync(accountId);
            return positions.GroupBy(p => p.Type).ToDictionary(g => g.Key, g => g.ToArray());
        }

        /// <inheritdoc/>
        public async Task<decimal> GetPositionsSumAsync(string accountId)
        {
            var bankBrokerClient = DependencyService.Resolve<IBankBrokerApiClient>();
            var positions = await bankBrokerClient.GetPositionsAsync(accountId);
            var currencies = await bankBrokerClient.GetCurrenciesAsync();
            var result = GetSumByPositions(positions, currencies);
            return result ?? 0;
        }

        protected decimal? GetSumByPositions(IReadOnlyCollection<Position> positions, IReadOnlyCollection<CurrencyMoney> currencies)
        {
            var sum = 0m;
            var positionsByCurrency = positions.GroupBy(p => p.AveragePositionPrice?.Currency);
            foreach (var group in positionsByCurrency)
            {
                var sumInCurrency = Math.Round(group.Sum(p => p.PositionCount * p.AveragePositionPrice.Value + p.ExpectedYield.Value), 2, MidpointRounding.ToEven);
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
            return sum;
        }
    }
}

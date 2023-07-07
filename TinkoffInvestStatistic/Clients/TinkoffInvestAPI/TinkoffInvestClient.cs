using Infrastructure.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TinkoffInvest;
using TinkoffInvest.Mappers;
using Xamarin.Forms;
using Newtonsoft.Json;
using TinkoffContracts = TinkoffInvest.Contracts;
using TinkoffInvestStatistic.Contracts;
using System.Text;
using Newtonsoft.Json.Converters;
using TinkoffInvest.Contracts.Accounts;
using TinkoffInvest.Contracts.Portfolio;

namespace Clients.TinkoffInvest
{
    /// <summary>
    /// Клиент для работы с API "Тинькофф Инвестиции".
    /// </summary>
    public class TinkoffInvestClient : IBankBrokerApiClient
    {
        /// <summary>
        /// Токен.
        /// </summary>
        private static readonly string Token = "Bearer " + Secrets.TINKOFF_INVEST_TOKEN;

        /// <summary>
        /// Базовый URL API.
        /// </summary>
        private static readonly string BaseUrl = "https://invest-public-api.tinkoff.ru/rest/";
        private static readonly string ApiVersion = "tinkoff.public.invest.api.contract.v1.";

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Account>> GetAccountsAsync()
        {
            IReadOnlyCollection<TinkoffContracts.Accounts.Account> accounts;
            using (HttpClient client = new HttpClient())
            {
                var request = CreateRequest($"{BaseUrl}{ApiVersion}UsersService/GetAccounts");
                request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(
                        $"Произошла ошибка(код: {response.StatusCode}) при получение счетов: {JsonConvert.SerializeObject(data)}");
                }
                else
                {
                    var deserialized = JsonConvert.DeserializeObject<AccountsResponse>(data, new StringEnumConverter());
                    accounts = deserialized.Accounts;
                }
            }

            var mapper = DependencyService.Resolve<IMapper<TinkoffContracts.Accounts.Account, TinkoffInvestStatistic.Contracts.Account>>();
            var result = accounts.Select(a => mapper.Map(a)).ToArray();

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Position>> GetAccountPositionsAsync(string accountId)
        {
            PortfolioReponse portfolio;
            using (HttpClient client = new HttpClient() { BaseAddress = new Uri(BaseUrl) })
            {
                var request = CreateRequest($"{ApiVersion}OperationsService/GetPortfolio");
                var response = await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(
                        $"Произошла ошибка(код: {response.StatusCode}) при получение счетов: {JsonConvert.SerializeObject(data)}");
                }
                else
                {
                    portfolio = JsonConvert.DeserializeObject<PortfolioReponse>(data);
                }
            }

            var mapper = DependencyService.Resolve<IMapper<PortfolioReponse, IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Position>>>();
            var result = mapper.Map(portfolio);

            return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CurrencyMoney>> GetCurrenciesAsync()
        {
            return Array.Empty<CurrencyMoney>();
            //using var connection = ConnectionFactory.GetConnection(Token);
            //var context = connection.Context;

            //var currencies = await context.PortfolioAsync().ConfigureAwait(continueOnCapturedContext: false);
            //var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.Portfolio, IReadOnlyCollection<CurrencyMoney>>>();
            //var result = mapper.Map(currencies);

            //return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<CurrencyMoney>> GetFiatPositionsAsync(string accountId)
        {
            return Array.Empty<CurrencyMoney>();
            //using var connection = ConnectionFactory.GetConnection(Token);
            //var context = connection.Context;

            //var portfolio = await context.PortfolioCurrenciesAsync(accountId).ConfigureAwait(continueOnCapturedContext: false);
            //var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.PortfolioCurrencies, IReadOnlyCollection<CurrencyMoney>>>();
            //var result = mapper.Map(portfolio);

            //return result;
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Position>> FindPositionsAsync(string ticker)
        {
            return Array.Empty<TinkoffInvestStatistic.Contracts.Position>();
            //using var connection = ConnectionFactory.GetConnection(Token);
            //var context = connection.Context;

            //var instruments = await context.MarketSearchByTickerAsync(ticker).ConfigureAwait(continueOnCapturedContext: false);
            //var mapper = DependencyService.Resolve<IMapper<Tinkoff.Trading.OpenApi.Models.MarketInstrumentList, IReadOnlyCollection<Position>>>();
            //var result = mapper.Map(instruments);

            //return result;
        }

        private static HttpRequestMessage CreateRequest(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(url));
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Authorization", Token);
            return request;
        }
    }
}

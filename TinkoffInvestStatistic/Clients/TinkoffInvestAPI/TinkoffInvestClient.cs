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
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvest.Contracts.Instruments;

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
        private static readonly string BaseUrl = "https://invest-public-api.tinkoff.ru/rest/tinkoff.public.invest.api.contract.v1.";

        /// <summary>
        /// Список активных типов счетов.
        /// </summary>
        public static readonly IReadOnlyCollection<TinkoffContracts.Enums.AccountType> ActiveAccountTypes = new[]
        {
            TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF_IIS,
            TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF
        };

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Account>> GetAccountsAsync()
        {
            IReadOnlyCollection<TinkoffContracts.Accounts.Account> accounts;
            using (HttpClient client = new HttpClient())
            {
                var request = CreateRequest($"{BaseUrl}UsersService/GetAccounts");
                request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(
                        $"Произошла ошибка(код: {response.StatusCode}) при получении счетов: {JsonConvert.SerializeObject(data)}");
                }
                else
                {
                    var deserialized = JsonConvert.DeserializeObject<AccountsResponse>(data, new StringEnumConverter());
                    accounts = deserialized.Accounts;
                }
            }

            var mapper = DependencyService.Resolve<IMapper<TinkoffContracts.Accounts.Account, TinkoffInvestStatistic.Contracts.Account>>();
            var result = accounts.Where(a => ActiveAccountTypes.Contains(a.AccountType)).Select(a => mapper.Map(a)).ToArray();

            return result;
        }

        /// <inheritdoc/>
        public async Task<Portfolio> GetAccountsFullDataAsync(string accountId)
        {
            PortfolioReponse portfolio;
            using (HttpClient client = new HttpClient())
            {
                var request = CreateRequest($"{BaseUrl}OperationsService/GetPortfolio");
                var @params = JsonConvert.SerializeObject(new { accountId = accountId, currency = Currency.Rub });
                request.Content = new StringContent(@params, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(
                        $"Произошла ошибка(код: {response.StatusCode}) при получении данных счета №{accountId}: {JsonConvert.SerializeObject(data)}");
                }
                else
                {
                    portfolio = JsonConvert.DeserializeObject<PortfolioReponse>(data);
                }
            }

            var mapper = DependencyService.Resolve<IMapper<PortfolioReponse, Portfolio>>();
            var result = mapper.Map(portfolio);

            return result;
        }

        /// <inheritdoc/>
        public async Task<TinkoffInvestStatistic.Contracts.Position> FindPositionByFigiAsync(string figi, PositionType positionType)
        {
            InstrumentResponse instrument;
            using (HttpClient client = new HttpClient())
            {
                var request = CreateRequest(GetFindPositionUrl(positionType));
                var @params = JsonConvert.SerializeObject(new { id = figi, idType = "INSTRUMENT_ID_TYPE_FIGI" });
                request.Content = new StringContent(@params, Encoding.UTF8, "application/json");
                var response = await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                var data = await response.Content.ReadAsStringAsync().ConfigureAwait(continueOnCapturedContext: false);
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException(
                        $"Произошла ошибка(код: {response.StatusCode}) при получении данных позиции с figi '{figi}' с типом '{positionType}' : {JsonConvert.SerializeObject(data)}");
                }
                else
                {
                    instrument = JsonConvert.DeserializeObject<InstrumentResponse>(data);
                }
            }

            var mapper = DependencyService.Resolve<IMapper<InstrumentResponse, TinkoffInvestStatistic.Contracts.Position>>();
            var result = mapper.Map(instrument);

            return result;
        }

        private string GetFindPositionUrl(PositionType positionType)
        {
            return positionType switch
            {
                PositionType.Stock => $"{BaseUrl}InstrumentsService/ShareBy",
                PositionType.Bond => $"{BaseUrl}InstrumentsService/BondBy",
                PositionType.Etf => $"{BaseUrl}InstrumentsService/EtfBy",
                _ => throw new ArgumentOutOfRangeException()
            };
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

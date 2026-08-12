using Infrastructure.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TinkoffInvest;
using TinkoffInvest.Mappers;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using TinkoffContracts = TinkoffInvest.Contracts;
using TinkoffInvestStatistic.Contracts;
using System.Text;
using Newtonsoft.Json.Converters;
using TinkoffInvest.Contracts.Accounts;
using TinkoffInvest.Contracts.Portfolio;
using TinkoffInvestStatistic.Contracts.Enums;
using TinkoffInvest.Contracts.Instruments;
using System.Net.Sockets;

namespace Clients.TinkoffInvest
{
    /// <summary>
    /// Клиент для работы с API "Тинькофф Инвестиции".
    /// </summary>
    public class TinkoffInvestClient : IBankBrokerApiClient
    {
        private const int MaxSendAttempts = 3;
        private static readonly TimeSpan RetryDelay = TimeSpan.FromMilliseconds(250);
        private readonly HttpMessageHandler? _messageHandler;

        /// <summary>
        /// Токен.
        /// </summary>
        private static readonly string Token = "Bearer " + Secrets.TINKOFF_INVEST_TOKEN;

        /// <summary>
        /// Базовый URL API.
        /// </summary>
        private static readonly string BaseUrl = "https://invest-public-api.tbank.ru/rest/tinkoff.public.invest.api.contract.v1.";

        /// <summary>
        /// Список активных типов счетов.
        /// </summary>
        public static readonly IReadOnlyCollection<TinkoffContracts.Enums.AccountType> ActiveAccountTypes = new[]
        {
            TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF_IIS,
            TinkoffContracts.Enums.AccountType.ACCOUNT_TYPE_TINKOFF
        };

        public TinkoffInvestClient()
        {
        }

        internal TinkoffInvestClient(HttpMessageHandler messageHandler)
        {
            _messageHandler = messageHandler ?? throw new ArgumentNullException(nameof(messageHandler));
        }

        /// <inheritdoc/>
        public async Task<IReadOnlyCollection<TinkoffInvestStatistic.Contracts.Account>> GetAccountsAsync()
        {
            IReadOnlyCollection<TinkoffContracts.Accounts.Account> accounts;
            using (HttpClient client = CreateHttpClient())
            {
                var response = await SendAsyncWithRetry(
                    client,
                    () =>
                    {
                        var request = CreateRequest($"{BaseUrl}UsersService/GetAccounts");
                        request.Content = new StringContent("{}", Encoding.UTF8, "application/json");
                        return request;
                    }).ConfigureAwait(continueOnCapturedContext: false);
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
            using (HttpClient client = CreateHttpClient())
            {
                var @params = JsonConvert.SerializeObject(new { accountId = accountId, currency = Currency.Rub });
                var response = await SendAsyncWithRetry(
                    client,
                    () =>
                    {
                        var request = CreateRequest($"{BaseUrl}OperationsService/GetPortfolio");
                        request.Content = new StringContent(@params, Encoding.UTF8, "application/json");
                        return request;
                    }).ConfigureAwait(continueOnCapturedContext: false);
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
            using (HttpClient client = CreateHttpClient())
            {
                var @params = JsonConvert.SerializeObject(new { id = figi, idType = "INSTRUMENT_ID_TYPE_FIGI" });
                var response = await SendAsyncWithRetry(
                    client,
                    () =>
                    {
                        var request = CreateRequest(GetFindPositionUrl(positionType));
                        request.Content = new StringContent(@params, Encoding.UTF8, "application/json");
                        return request;
                    }).ConfigureAwait(continueOnCapturedContext: false);
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

        private HttpClient CreateHttpClient()
        {
            return _messageHandler == null
                ? new HttpClient()
                : new HttpClient(_messageHandler, disposeHandler: false);
        }

        private static async Task<HttpResponseMessage> SendAsyncWithRetry(
            HttpClient client,
            Func<HttpRequestMessage> requestFactory)
        {
            for (var attempt = 1; ; attempt++)
            {
                using var request = requestFactory();
                try
                {
                    return await client.SendAsync(request).ConfigureAwait(continueOnCapturedContext: false);
                }
                catch (HttpRequestException ex) when (
                    attempt < MaxSendAttempts &&
                    IsNameResolutionFailure(ex))
                {
                    await Task.Delay(RetryDelay * attempt).ConfigureAwait(continueOnCapturedContext: false);
                }
            }
        }

        private static bool IsNameResolutionFailure(HttpRequestException exception)
        {
            if (exception.HttpRequestError == HttpRequestError.NameResolutionError)
            {
                return true;
            }

            for (Exception? current = exception.InnerException; current != null; current = current.InnerException)
            {
                if (current.GetType().FullName == "Java.Net.UnknownHostException")
                {
                    return true;
                }

                if (current is SocketException socketException &&
                    socketException.SocketErrorCode is SocketError.HostNotFound or SocketError.TryAgain or SocketError.NoData)
                {
                    return true;
                }
            }

            return false;
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

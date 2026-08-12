using Clients.TinkoffInvest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Tests.Clients;

[TestClass]
public class TinkoffInvestClientConfigurationTests
{
    [TestMethod]
    public void BaseUrl_UsesCurrentTBankHost()
    {
        var baseUrlField = typeof(TinkoffInvestClient).GetField(
            "BaseUrl",
            BindingFlags.NonPublic | BindingFlags.Static);

        Assert.IsNotNull(baseUrlField);
        Assert.AreEqual(
            "https://invest-public-api.tbank.ru/rest/tinkoff.public.invest.api.contract.v1.",
            baseUrlField.GetValue(null));
    }

    [TestMethod]
    public async Task GetAccountsAsync_RetriesTransientNameResolutionFailure()
    {
        var handler = new NameResolutionFailureThenUnauthorizedHandler();
        var constructor = typeof(TinkoffInvestClient).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            binder: null,
            new[] { typeof(HttpMessageHandler) },
            modifiers: null);

        Assert.IsNotNull(
            constructor,
            "TinkoffInvestClient must provide an internal HttpMessageHandler constructor for deterministic network tests.");

        var client = (TinkoffInvestClient)constructor.Invoke(new object[] { handler });

        var exception = await Assert.ThrowsExceptionAsync<ApplicationException>(
            () => client.GetAccountsAsync());

        Assert.AreEqual(2, handler.CallCount);
        StringAssert.Contains(exception.Message, HttpStatusCode.Unauthorized.ToString());
    }

    private sealed class NameResolutionFailureThenUnauthorizedHandler : HttpMessageHandler
    {
        public int CallCount { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            CallCount++;
            if (CallCount == 1)
            {
                throw new HttpRequestException(
                    HttpRequestError.NameResolutionError,
                    "Temporary DNS failure.");
            }

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("{}"),
            });
        }
    }
}

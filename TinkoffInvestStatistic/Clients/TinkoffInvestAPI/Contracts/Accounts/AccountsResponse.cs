using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TinkoffInvest.Contracts.Accounts
{
    /// <summary>
    /// Ответ на запрос счетов.
    /// </summary>
    public class AccountsResponse
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        [JsonProperty(PropertyName = "accounts")]
        public IReadOnlyCollection<Account> Accounts { get; set; } = Array.Empty<Account>();
    }
}

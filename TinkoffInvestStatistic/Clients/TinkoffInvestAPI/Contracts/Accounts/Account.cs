using Newtonsoft.Json;
using TinkoffInvest.Contracts.Enums;

namespace TinkoffInvest.Contracts.Accounts
{
    /// <summary>
    /// Счет.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Идентификатор счета.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Наименование счета.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Тип счета.
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public AccountType AccountType { get; set; }
    }
}

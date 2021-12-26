using System.ComponentModel;

namespace Contracts.Enums
{
    /// <summary>
    /// Тип счета
    /// </summary>
    public enum AccountType
    {
        [Description("ИИС")]
        Iis = 1,

        [Description("Брокерский счет")]
        BrokerAccount = 2,
    }
}

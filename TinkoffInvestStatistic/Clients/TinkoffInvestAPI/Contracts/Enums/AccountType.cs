namespace TinkoffInvest.Contracts.Enums
{
    /// <summary>
    /// Тип счёта.
    /// </summary>
    public enum AccountType
    {
        /// <summary>
        /// Тип аккаунта не определён.
        /// </summary>
        ACCOUNT_TYPE_UNSPECIFIED = 0,

        /// <summary>
        /// Брокерский счёт Тинькофф.
        /// </summary>
        ACCOUNT_TYPE_TINKOFF = 1,

        /// <summary>
        /// ИИС счёт.
        /// </summary>
        ACCOUNT_TYPE_TINKOFF_IIS = 2,

        /// <summary>
        /// Инвесткопилка.
        /// </summary>
        ACCOUNT_TYPE_INVEST_BOX = 3	
    }
}

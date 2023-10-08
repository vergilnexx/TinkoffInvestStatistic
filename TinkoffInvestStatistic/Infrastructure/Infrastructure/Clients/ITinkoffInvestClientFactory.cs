namespace Infrastructure.Clients
{
    /// <summary>
    /// Фабрика клиентов к API Тинькофф инвестиции.
    /// </summary>
    public interface ITinkoffInvestClientFactory
    {
        /// <summary>
        /// Возвращает клиент брокера.
        /// </summary>
        /// <returns></returns>
        IBankBrokerApiClient Get();
    }
}

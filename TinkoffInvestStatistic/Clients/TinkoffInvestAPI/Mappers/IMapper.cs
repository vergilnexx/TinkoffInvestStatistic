namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Интерфейс маппера.
    /// </summary>
    /// <typeparam name="InnerType">Входной тип</typeparam>
    /// <typeparam name="OutputType">Выходной тип.</typeparam>
    public interface IMapper<InnerType, OutputType>
    {
        /// <summary>
        /// Маппит.
        /// </summary>
        /// <param name="type">Входной тип.</param>
        /// <returns>Выходной тип.</returns>
        OutputType Map(InnerType type);
    }
}

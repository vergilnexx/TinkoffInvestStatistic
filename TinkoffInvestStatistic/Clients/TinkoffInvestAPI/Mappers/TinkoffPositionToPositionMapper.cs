using TinkoffInvest.Contracts.Instruments;
using Position = TinkoffInvestStatistic.Contracts.Position;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="Position"/> в <see cref="Position"/>
    /// </summary>
    public class TinkoffPositionToPositionMapper : IMapper<InstrumentResponse, Position>
    {
        /// <inheritdoc/>
        public Position Map(InstrumentResponse instrument)
        {
            var result = new Position(instrument.Instrument.Figi)
            {
                Name = instrument.Instrument.Name,
                Ticker = instrument.Instrument.Ticker
            };
            return result;
        }
    }
}

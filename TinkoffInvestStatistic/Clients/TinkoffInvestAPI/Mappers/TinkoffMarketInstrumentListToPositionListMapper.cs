using System.Collections.Generic;
using System.Linq;
using Tinkoff.Trading.OpenApi.Models;

namespace TinkoffInvest.Mappers
{
    /// <summary>
    /// Маппер из <see cref="Tinkoff.Trading.OpenApi.Models.MarketInstrumentList"/> в <see cref="Contracts.Position"/>
    /// </summary>
    public class TinkoffMarketInstrumentListToPositionListMapper : IMapper<Tinkoff.Trading.OpenApi.Models.MarketInstrumentList, IReadOnlyCollection<Contracts.Position>>
    {
        /// <inheritdoc/>
        public IReadOnlyCollection<Contracts.Position> Map(Tinkoff.Trading.OpenApi.Models.MarketInstrumentList marketInstrumentList)
        {
            return marketInstrumentList.Instruments
                    .Select(i => Map(i))
                    .ToArray();
        }

        private Contracts.Position Map(MarketInstrument instrument)
        {
            var result = new Contracts.Position(instrument.Figi, EnumMapper.MapType(instrument.Type), instrument.Name, default);

            result.Ticker = instrument.Ticker;

            return result;
        }
    }
}

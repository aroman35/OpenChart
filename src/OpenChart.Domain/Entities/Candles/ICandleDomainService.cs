using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace OpenChart.Domain.Entities.Candles
{
    public interface ICandleDomainService
    {
        void CurrentInstrument(Exchange exchange, string securityCode);
        IEnumerable<Candle> ChangeTimeFrame(IEnumerable<Candle> sourceCandles, TimeFrame timeFrame);

        IAsyncEnumerable<Candle> ChangeTimeFrame(IAsyncEnumerable<Candle> sourceCandles, TimeFrame timeFrame,
            [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
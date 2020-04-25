using System;
using System.Collections.Generic;
using System.Threading;
using OpenChart.Ddd.Infrastructure;
using OpenChart.Domain.Entities;

namespace OpenChart.Domain.Services
{
    public interface ICandleDomainService : IDomainService<Candle, CandleDto>
    {
        void CurrentInstrument(IInstrumentInfo instrumentInfo);
        IEnumerable<Candle> ChangeTimeFrame(IEnumerable<Candle> sourceCandles, TimeSpan timeFrame);

        IAsyncEnumerable<Candle> ChangeTimeFrame(IAsyncEnumerable<Candle> sourceCandles, TimeSpan timeFrame,
            CancellationToken cancellationToken = default);
    }
}
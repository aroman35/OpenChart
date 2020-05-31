using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Entities.Candles;

namespace OpenChart.Application.Services
{
    public static class BusinessExtensions
    {
        public static IAsyncEnumerable<Candle> ChangeTimeFrame(
            this IAsyncEnumerable<Candle> candlesCollection,
            Exchange exchange,
            string securityCode,
            TimeFrame timeFrame,
            CancellationToken cancellationToken = default)
        {
            if (timeFrame.Equals(TimeFrame.M1) ||
                timeFrame.Equals(TimeFrame.H1) ||
                timeFrame.Equals(TimeFrame.D))
                return candlesCollection;

            var candleDomainService = new CandlesService();
            candleDomainService.CurrentInstrument(exchange, securityCode);
            return candleDomainService.ChangeTimeFrame(candlesCollection, timeFrame, cancellationToken);
        }

        public static async Task<string> WritToTsv(this IAsyncEnumerable<Candle> candlesCollection,
            CancellationToken cancellationToken = default)
        {
            await using var writer = new TsvWriter();
            return await writer.WriteCollectionAsync(candlesCollection, cancellationToken);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Entities.Candles;
using OpenChart.Domain.Exceptions;
using OpenChart.Domain.Extensions;

namespace OpenChart.Domain.Services
{
    public class CandleDomainService : ICandleDomainService
    {
        private Exchange ExchangeInfo;
        private string SecurityCode;

        public void CurrentInstrument(Exchange exchange, string securityCode)
        {
            ExchangeInfo = exchange;
            SecurityCode = securityCode;
        }

        public IEnumerable<Candle> ChangeTimeFrame(IEnumerable<Candle> sourceCandles, TimeFrame timeFrame)
        {
            CheckInstrument();

            var exchangeOffset = TimeSpan.FromMilliseconds(ExchangeInfo.TradeStart);

            var candlesWithUpdatedTimeFrame = sourceCandles
                .OrderBy(x => x)
                .GroupBy(x => x.TradeDateTime.Floor(timeFrame, exchangeOffset))
                .Select(Merge);

            return candlesWithUpdatedTimeFrame;
        }

        public IAsyncEnumerable<Candle> ChangeTimeFrame(IAsyncEnumerable<Candle> sourceCandles, TimeFrame timeFrame,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            CheckInstrument();
            var exchangeOffset = TimeSpan.FromMilliseconds(ExchangeInfo.TradeStart);

            var candlesWithUpdatedTimeFrame = sourceCandles
                .OrderBy()
                .GroupBy(x => x.TradeDateTime.Floor(timeFrame, exchangeOffset))
                .SelectAwaitWithCancellation(Merge);

            return candlesWithUpdatedTimeFrame;
        }

        private async ValueTask<Candle> Merge(IAsyncGrouping<DateTimeOffset, Candle> candlesGroup,
            CancellationToken cancellationToken)
        {
            var childCandles = await candlesGroup.ToLinkedListAsync(cancellationToken: cancellationToken);

            var resultCandle = new Candle
            {
                Date = candlesGroup.Key.ToUnixTimeMilliseconds(),
                Open = childCandles.First.Value.Open,
                Close = childCandles.Last.Value.Close,
                High = childCandles.Max(candle => candle.High),
                Low = childCandles.Min(candle => candle.Low),
                Volume = childCandles.Sum(candle => candle.Volume)
            };

            return resultCandle;
        }

        private Candle Merge(IGrouping<DateTimeOffset, Candle> candlesGroup)
        {
            var childCandles = candlesGroup.ToLinkedList();

            var resultCandle = new Candle
            {
                Date = candlesGroup.Key.ToUnixTimeMilliseconds(),
                Open = childCandles.First.Value.Open,
                Close = childCandles.Last.Value.Close,
                High = childCandles.Max(candle => candle.High),
                Low = childCandles.Min(candle => candle.Low),
                Volume = childCandles.Sum(candle => candle.Volume)
            };

            return resultCandle;
        }

        private void CheckInstrument()
        {
            if (string.IsNullOrEmpty(SecurityCode) || ExchangeInfo == null)
                throw new DomainException(GetType(), "Instrument is not set");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Exceptions;
using OpenChart.Domain.Extensions;

namespace OpenChart.Domain.Services
{
    public class CandleDomainService : ICandleDomainService
    {
        private Exchange ExchangeInfo;
        private string SecurityCode;

        public async IAsyncEnumerable<Candle> Create(IAsyncEnumerable<CandleDto> dto,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            await foreach (var candleDto in dto.WithCancellation(cancellationToken))
                yield return Create(candleDto);
        }

        public Candle Create(CandleDto dto)
        {
            CheckInstrument();

            var classCode = ExchangeInfo.ClassCode;
            var securityCode = SecurityCode;

            var candle = new Candle(dto, classCode, securityCode);

            return candle;
        }

        public IEnumerable<Candle> Create(IEnumerable<CandleDto> dto)
        {
            CheckInstrument();

            return dto.Select(Create);
        }

        public void CurrentInstrument(Exchange exchange, string securityCode)
        {
            ExchangeInfo = exchange;
            SecurityCode = securityCode;
        }

        public IEnumerable<Candle> ChangeTimeFrame(IEnumerable<Candle> sourceCandles, TimeFrame timeFrame)
        {
            CheckInstrument();

            var candlesWithUpdatedTimeFrame = ChangeTimeFrameAsDto(sourceCandles, timeFrame);

            return Create(candlesWithUpdatedTimeFrame);
        }

        public async IAsyncEnumerable<Candle> ChangeTimeFrame(IAsyncEnumerable<Candle> sourceCandles, TimeFrame timeFrame,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            var candlesWithUpdatedTimeFrame = ChangeTimeFrameAsDto(sourceCandles, timeFrame);

            await foreach (var candleDto in candlesWithUpdatedTimeFrame.WithCancellation(cancellationToken))
            {
                yield return Create(candleDto);
            }
        }

        private IAsyncEnumerable<CandleDto> ChangeTimeFrameAsDto(IAsyncEnumerable<Candle> sourceCandles, TimeFrame timeFrame)
        {
            var exchangeOffset = TimeSpan.FromMilliseconds(ExchangeInfo.TradeStart);

            var candlesWithUpdatedTimeFrame = sourceCandles
                .OrderBy()
                .GroupBy(x => x.TradeDateTime.Floor(timeFrame, exchangeOffset))
                .SelectAwaitWithCancellation(Merge);

            return candlesWithUpdatedTimeFrame;
        }

        private IEnumerable<CandleDto> ChangeTimeFrameAsDto(IEnumerable<Candle> sourceCandles, TimeFrame timeFrame)
        {
            var exchangeOffset = TimeSpan.FromMilliseconds(ExchangeInfo.TradeStart);

            var candlesWithUpdatedTimeFrame = sourceCandles
                .OrderBy(x => x)
                .GroupBy(x => x.TradeDateTime.Floor(timeFrame, exchangeOffset))
                .Select(Merge);

            return candlesWithUpdatedTimeFrame;
        }

        private async ValueTask<CandleDto> Merge(IAsyncGrouping<DateTimeOffset, Candle> candlesGroup,
            CancellationToken cancellationToken)
        {
            var childCandles = await candlesGroup.ToLinkedListAsync(cancellationToken: cancellationToken);

            var resultCandle = new CandleDto
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

        private CandleDto Merge(IGrouping<DateTimeOffset, Candle> candlesGroup)
        {
            var childCandles = candlesGroup.ToLinkedList();

            var resultCandle = new CandleDto
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
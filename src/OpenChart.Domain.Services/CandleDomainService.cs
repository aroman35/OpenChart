using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using OpenChart.Ddd.Infrastructure;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Events;
using OpenChart.Domain.Exceptions;
using OpenChart.Domain.Extensions;

namespace OpenChart.Domain.Services
{
    public class CandleDomainService : ICandleDomainService
    {
        private readonly IInstrumentInfoProvider _instrumentInfoProvider;
        private readonly IDomainEventBus _domainEventBus;
        private readonly INotificationService _notificationService;

        public CandleDomainService(
            IInstrumentInfoProvider instrumentInfoProvider,
            IDomainEventBus domainEventBus,
            INotificationService notificationService)
        {
            _instrumentInfoProvider = instrumentInfoProvider;
            _domainEventBus = domainEventBus;
            _notificationService = notificationService;
        }

        public ITransaction<Candle> Transaction { get; }

        public Task<Candle> Create(CandleDto dto, CancellationToken cancellationToken = default)
        {
            CheckInstrument();

            var candle = new Candle(
                dto,
                _instrumentInfoProvider.InstrumentInfo.Instrument.ClassCode,
                _instrumentInfoProvider.InstrumentInfo.Instrument.ClassCode);

            Task.Run(() => _domainEventBus.Send(new CandleCreatedEvent(GetType(), candle)), cancellationToken);
            return Task.FromResult(candle);
        }

        public Task<Candle> Create(IEnumerable<CandleDto> dto, CancellationToken cancellationToken = default)
        {
            CheckInstrument();
            if (!dto.Any())
                return Task.FromResult(Candle.Empty(_instrumentInfoProvider.InstrumentInfo.Instrument.ClassCode,
                    _instrumentInfoProvider.InstrumentInfo.Instrument.SecurityCode));

            var candles = dto.Select(x =>
                new Candle(x,
                    _instrumentInfoProvider.InstrumentInfo.Instrument.ClassCode,
                    _instrumentInfoProvider.InstrumentInfo.Instrument.SecurityCode));

            var candle = new Candle(candles);

            Task.Run(() => _domainEventBus.Send(new CandleCreatedEvent(GetType(), candle)), cancellationToken);
            return Task.FromResult(candle);
        }

        public void CurrentInstrument(IInstrumentInfo instrumentInfo)
        {
            _instrumentInfoProvider.InstrumentInfo = instrumentInfo;
        }

        public IEnumerable<Candle> ChangeTimeFrame(IEnumerable<Candle> sourceCandles, TimeSpan timeFrame)
        {
            UpdateTimeFrame(timeFrame);

            var candlesWithUpdatedTimeFrame = sourceCandles.OrderBy(x => x)
                .GroupBy(x => x.TradeDateTime.Floor(_instrumentInfoProvider.InstrumentInfo.TimeFrame,
                    _instrumentInfoProvider.InstrumentInfo.TradeDateStartOffset))
                .Select(x =>
                {
                    var childCandles = x.ToArray();
                    var resultCandle = new Candle(childCandles);
                    return resultCandle.SetTradeDateTime(x.Key);
                })
                .ToArray();

            return candlesWithUpdatedTimeFrame;
        }

        public IEnumerable<Candle> ChangeTimeFrame(IEnumerable<CandleDto> sourceCandles, TimeSpan timeFrame)
        {
            CheckInstrument();
            var c = sourceCandles.Select(x =>
                new Candle(x,
                    _instrumentInfoProvider.InstrumentInfo.Instrument.ClassCode,
                    _instrumentInfoProvider.InstrumentInfo.Instrument.SecurityCode));

            return ChangeTimeFrame(c, timeFrame);
        }

        public async IAsyncEnumerable<Candle> ChangeTimeFrame(IAsyncEnumerable<Candle> sourceCandles, TimeSpan timeFrame,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            UpdateTimeFrame(timeFrame);

            var candlesWithUpdatedTimeFrame = sourceCandles
                .OrderBy(x => x)
                .GroupBy(x => x.TradeDateTime.Floor(_instrumentInfoProvider.InstrumentInfo.TimeFrame,
                    _instrumentInfoProvider.InstrumentInfo.TradeDateStartOffset));

            await foreach (var candlesGroup in candlesWithUpdatedTimeFrame.WithCancellation(cancellationToken))
            {
                var childCandles = await candlesGroup.ToArrayAsync(cancellationToken: cancellationToken);
                var resultCandle = new Candle(childCandles);

                yield return resultCandle.SetTradeDateTime(candlesGroup.Key);
            }
        }

        private void UpdateTimeFrame(TimeSpan timeFrame)
        {
            CheckInstrument();

            _instrumentInfoProvider.InstrumentInfo.TimeFrame = timeFrame;
        }

        private void CheckInstrument()
        {
            if (!_instrumentInfoProvider.IsInstrumentSet)
                throw new DomainException(GetType(), "Instrument is not set");
        }
    }
}
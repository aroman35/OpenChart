using System;
using OpenChart.Ddd.Aggregates;
using OpenChart.Domain.Entities;

namespace OpenChart.Domain.Events
{
    public class CandleCreatedEvent : IDomainEvent
    {
        public CandleCreatedEvent(Type eventSource, Candle createdCandle)
        {
            EventSource = eventSource;
            CreatedCandle = createdCandle;
        }

        public Type EventSource { get; }
        public Candle CreatedCandle { get; }
    }
}
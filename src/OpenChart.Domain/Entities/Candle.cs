using System;
using System.ComponentModel;
using OpenChart.Ddd.Aggregates;
using OpenChart.Domain.DddImplementation;
using OpenChart.Domain.Exceptions;

namespace OpenChart.Domain.Entities
{
    [ImmutableObject(true)]
    public class Candle : AggregateRoot<CandleDto>, ICandle, ITradeInstrument, IEquatable<Candle>
    {
        public static Candle Empty(string classCode, string securityCode)
        {
            return new Candle(classCode, securityCode);
        }

        private Candle()
        {
            CreationTime = DateTimeOffset.UtcNow;
            LastUpdate = DateTimeOffset.UtcNow;
        }

        private Candle(string classCode, string securityCode) : this()
        {
            ClassCode = classCode;
            SecurityCode = securityCode;
        }

        public Candle(CandleDto dto, string classCode, string securityCode) : this(classCode, securityCode)
        {
            Id = dto.Id;
            Open = dto.Open;
            Close = dto.Close;
            High = dto.High;
            Low = dto.Low;
            Volume = dto.Volume;
            TradeDateTime = DateTimeOffset.FromUnixTimeMilliseconds(dto.Date).LocalDateTime;
        }

        public DateTimeOffset TradeDateTime { get; private set; }
        public decimal Open { get; private set; }
        public decimal Close { get; private set; }
        public decimal High { get; private set; }
        public decimal Low { get; private set; }
        public decimal Volume { get; private set; }
        public string ClassCode { get; }
        public string SecurityCode { get; }

        public int CompareTo(ICandle other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return TradeDateTime.CompareTo(other.TradeDateTime);
        }

        public bool Equals(Candle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return TradeDateTime.Equals(other.TradeDateTime) && ClassCode == other.ClassCode && SecurityCode == other.SecurityCode;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Candle) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TradeDateTime, ClassCode, SecurityCode);
        }

        public override int CompareTo(IAggregateRoot<CandleDto> other)
        {
            if (other is ICandle candle)
                return CompareTo(candle);
            throw new DomainException(GetType(), "Unable to compare with not ICandle object");
        }

        public override bool Equals(IAggregateRoot<CandleDto> other)
        {
            if (other is ICandle candle)
                return Equals(candle);
            return false;
        }

        public Candle SetTradeDateTime(DateTime dateTime)
        {
            TradeDateTime = dateTime;
            return this;
        }
    }
}
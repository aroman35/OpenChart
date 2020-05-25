using System;
using System.Collections.Generic;
using MediatR;
using OpenChart.Domain.Entities;

namespace OpenChart.Application.Queries.CandlesByCount
{
    public class CandlesCountRequest : IRequest<IEnumerable<Candle>>
    {
        public readonly string ClassCode;
        public readonly string SecurityCode;
        public readonly long DateLimit;
        public readonly int CandlesCount;
        public readonly TimeSpan TimeFrame;

        public CandlesCountRequest(string classCode, string securityCode, DateTime dateLimit, int candlesCount, TimeSpan timeFrame)
        {
            ClassCode = classCode;
            SecurityCode = securityCode;
            DateLimit = ((DateTimeOffset) dateLimit).ToUnixTimeMilliseconds();
            CandlesCount = candlesCount;
            TimeFrame = timeFrame;
        }
    }
}
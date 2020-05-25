using System;
using OpenChart.Domain.Entities;

namespace OpenChart.Tests.Unit.Infrastructure
{
    public static class Fakes
    {
        public static Exchange MockExchange()
        {
            var exchange = new Exchange()
            {
                ClassCode = "SPBXM",
                TradeStart = (long) TimeSpan.FromHours(7).TotalMilliseconds
            };
            return exchange;
        }
    }
}
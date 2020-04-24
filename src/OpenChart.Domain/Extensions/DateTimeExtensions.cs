using System;

namespace OpenChart.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime Floor(this DateTime dateTime, TimeSpan interval)
        {
            return dateTime.AddTicks(-(dateTime.Ticks % interval.Ticks));
        }

        public static DateTime Floor(this DateTime dateTime, TimeSpan interval, TimeSpan tradeDateStartOffset)
        {
            return dateTime.AddTicks(-(((tradeDateStartOffset.Ticks % interval.Ticks) + dateTime.Ticks) % interval.Ticks));
        }
    }
}
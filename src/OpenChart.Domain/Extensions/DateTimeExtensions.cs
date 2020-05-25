using System;
using OpenChart.Domain.Entities;

namespace OpenChart.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        private static DateTimeOffset Floor(this DateTimeOffset dateTime, TimeSpan interval)
        {
            return dateTime.AddTicks(-(dateTime.UtcTicks % interval.Ticks));
        }

        private static DateTimeOffset Floor(this DateTimeOffset dateTime, TimeSpan interval, TimeSpan tradeDateStartOffset)
        {
            return dateTime.AddTicks(-((tradeDateStartOffset.Ticks % interval.Ticks + dateTime.UtcTicks) % interval.Ticks));
        }

        public static DateTimeOffset Floor(this DateTimeOffset dateTime, TimeFrame timeFrame, TimeSpan tradeDateStartOffset)
        {
            dateTime = dateTime.ToUniversalTime();

            var date = timeFrame switch
            {
                TimeFrame.D => dateTime.Date,
                TimeFrame.W => dateTime.FirstDayOfWeek(),
                TimeFrame.MN => dateTime.FirstDayOfMonth(),
                TimeFrame.H4 => dateTime.Floor(TimeSpan.FromSeconds((int) timeFrame), tradeDateStartOffset),
                _ => dateTime.Floor(TimeSpan.FromSeconds((int) timeFrame))
            };

            return date;
        }

        private static DateTimeOffset FirstDayOfWeek(this DateTimeOffset dateTime)
        {
            var dayOfWeek = (int) dateTime.DayOfWeek;
            var diff = dayOfWeek == 0 ? 6 : dayOfWeek - 1;

            var result = dateTime.Date.AddDays(-diff);

            return result;
        }

        private static DateTimeOffset FirstDayOfMonth(this DateTimeOffset dateTime)
        {
            return new DateTimeOffset(dateTime.Year, dateTime.Month, 1, 0, 0, 0, TimeSpan.Zero).Date;
        }
    }
}
using System;
using OpenChart.Domain.Entities;

namespace OpenChart.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        private static readonly long DayTicks;

        static DateTimeExtensions()
        {
            DayTicks = TimeSpan.FromDays(1).Ticks;
        }
        private static DateTimeOffset Floor(this DateTimeOffset dateTime, TimeSpan interval)
        {
            var sourceDateTicks = dateTime.UtcTicks;
            var roundComparer = interval.Ticks;

            var tradeDateStart = sourceDateTicks - sourceDateTicks % DayTicks;
            var difference = (sourceDateTicks - tradeDateStart) % roundComparer;

            return dateTime.AddTicks(-difference);
        }

        private static DateTimeOffset Floor(this DateTimeOffset dateTime, TimeSpan interval, TimeSpan tradeDateStartOffset)
        {
            var tradeStart = tradeDateStartOffset.Ticks;
            var sourceDateTicks = dateTime.UtcTicks;
            var roundComparer = interval.Ticks;

            var tradeDateStart = sourceDateTicks - sourceDateTicks % DayTicks + tradeStart;
            var difference = (sourceDateTicks - tradeDateStart) % roundComparer;

            return dateTime.AddTicks(-difference);
        }

        public static DateTimeOffset Date(this DateTimeOffset dateTime)
        {
            var sourceDateTicks = dateTime.UtcTicks;
            var difference = sourceDateTicks % DayTicks;

            return dateTime.AddTicks(-difference);
        }

        public static DateTimeOffset TradeDate(this DateTimeOffset dateTime, TimeSpan tradeDayStart)
        {
            var tradeStart = tradeDayStart.Ticks;
            var sourceDateTicks = dateTime.UtcTicks;

            var difference = sourceDateTicks % DayTicks - tradeStart;
            difference = difference < 0 ? DayTicks + difference : difference;

            return dateTime.AddTicks(-difference);
        }

        public static DateTimeOffset FloorExact(this DateTimeOffset dateTime, TimeSpan timeFrame, TimeSpan tradeDateStartOffset)
        {
            if (timeFrame.Ticks > DayTicks)
                throw new AggregateException("Time frame must be lower than 24 hours");

            return dateTime.Floor(timeFrame, tradeDateStartOffset);
        }

        public static DateTimeOffset Floor(this DateTimeOffset dateTime, TimeFrame timeFrame, TimeSpan tradeDateStartOffset)
        {
            var date = timeFrame switch
            {
                TimeFrame.D => dateTime.TradeDate(tradeDateStartOffset),
                TimeFrame.W => dateTime.FirstDayOfWeek(tradeDateStartOffset),
                TimeFrame.MN => dateTime.FirstDayOfMonth(tradeDateStartOffset),
                TimeFrame.H4 => dateTime.Floor(TimeSpan.FromSeconds((int) timeFrame), tradeDateStartOffset),
                _ => dateTime.Floor(TimeSpan.FromSeconds((int) timeFrame))
            };

            return date;
        }

        private static DateTimeOffset FirstDayOfWeek(this DateTimeOffset dateTime, TimeSpan tradeDayStart)
        {
            var sourceTradeDate = dateTime.TradeDate(tradeDayStart);
            var dayOfWeek = (int) sourceTradeDate.DayOfWeek;
            var difference = dayOfWeek == 0 ? 6 : dayOfWeek - 1;

            var result = sourceTradeDate.AddDays(-difference);

            return result;
        }

        private static DateTimeOffset FirstDayOfMonth(this DateTimeOffset dateTime, TimeSpan tradeDayStart)
        {
            var sourceTradeDate = dateTime.TradeDate(tradeDayStart);
            return new
                DateTimeOffset(sourceTradeDate.Year, sourceTradeDate.Month, 1, 0, 0, 0, TimeSpan.Zero).AddTicks(tradeDayStart.Ticks);
        }
    }
}
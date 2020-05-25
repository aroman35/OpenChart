using System;
using System.Globalization;
using OpenChart.Domain.Entities;
using OpenChart.Domain.Extensions;
using Shouldly;
using Xunit;

namespace OpenChart.Tests.Unit.ExtensionsTests
{
    public class DateTimeExtensionsTest
    {
        private readonly TimeSpan TradeDateOffset = TimeSpan.FromHours(7);

        [Theory]
        [InlineData(TimeFrame.M1, "01/01/2020 00:00:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M1, "01/01/2020 00:01:00+00", "01/01/2020 00:01:00+00")]
        [InlineData(TimeFrame.M5, "01/01/2020 00:01:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M5, "01/01/2020 00:02:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M5, "01/01/2020 00:03:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M5, "01/01/2020 00:04:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M5, "01/01/2020 00:05:00+00", "01/01/2020 00:05:00+00")]
        [InlineData(TimeFrame.M15, "01/01/2020 00:00:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M15, "01/01/2020 00:05:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M15, "01/01/2020 00:15:00+00", "01/01/2020 00:15:00+00")]
        [InlineData(TimeFrame.M30, "01/01/2020 00:00:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M30, "01/01/2020 00:05:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M30, "01/01/2020 00:15:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.M30, "01/01/2020 00:30:00+00", "01/01/2020 00:30:00+00")]
        [InlineData(TimeFrame.H1, "01/01/2020 00:00:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.H1, "01/01/2020 00:05:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.H1, "01/01/2020 00:15:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.H1, "01/01/2020 00:30:00+00", "01/01/2020 00:00:00+00")]
        [InlineData(TimeFrame.H1, "01/01/2020 01:00:00+00", "01/01/2020 01:00:00+00")]
        [InlineData(TimeFrame.H4, "01/01/2020 07:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.H4, "01/01/2020 08:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.H4, "01/01/2020 09:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.H4, "01/01/2020 10:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.H4, "01/01/2020 11:00:00+00", "01/01/2020 11:00:00+00")]
        [InlineData(TimeFrame.H4, "01/01/2020 12:00:00+00", "01/01/2020 11:00:00+00")]
        [InlineData(TimeFrame.D, "01/01/2020 07:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.D, "01/01/2020 08:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.D, "01/02/2020 06:00:00+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.D, "01/02/2020 06:59:59+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.MN, "02/01/2020 06:59:59+00", "01/01/2020 07:00:00+00")]
        [InlineData(TimeFrame.MN, "02/01/2020 07:00:00+00", "02/01/2020 07:00:00+00")]

        public void StaticTimeFramesTest(TimeFrame frame, string sourceDateRaw, string expectedDateRaw)
        {
            var sourceDate = DateTimeOffset.Parse(sourceDateRaw, CultureInfo.InvariantCulture);
            var expectedDate = DateTimeOffset.Parse(expectedDateRaw, CultureInfo.InvariantCulture);

            var resultDate = sourceDate.Floor(frame, TradeDateOffset);

            resultDate.ShouldBe(expectedDate);
        }
    }
}
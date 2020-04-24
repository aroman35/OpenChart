using System;

namespace OpenChart.Ddd.Infrastructure
{
    public interface IDateProvider
    {
        DateTimeOffset UtcDate { get; }
    }
}
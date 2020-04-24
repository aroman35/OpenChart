using System;

namespace OpenChart.Ddd.Infrastructure
{
    public interface INotification
    {
        Type SourceService { get; }
        string Message { get; }
    }
}
using System.Threading;
using System.Threading.Tasks;

namespace OpenChart.Ddd.Infrastructure
{
    public interface INotificationService
    {
        Task Notify(INotification notification, CancellationToken cancellationToken = default);
    }
}
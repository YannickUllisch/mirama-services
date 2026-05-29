
namespace Mirama.SharedKernel.Abstractions.Common.Interfaces;

public interface INotificationHandler<in TNotification>
    where TNotification : INotification
{
    Task HandleAsync(TNotification notification, CancellationToken cancellationToken);
}

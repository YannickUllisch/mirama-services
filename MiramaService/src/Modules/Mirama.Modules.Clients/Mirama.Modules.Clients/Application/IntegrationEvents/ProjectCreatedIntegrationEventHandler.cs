using Microsoft.Extensions.Logging;
using Mirama.Modules.PM.Contracts.Events;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Application.IntegrationEvents;

internal sealed class ProjectCreatedIntegrationEventHandler(ILogger<ProjectCreatedIntegrationEventHandler> logger)
    : INotificationHandler<ProjectCreatedEvent>
{
    public Task HandleAsync(ProjectCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Clients module observed ProjectCreatedEvent {EventId} for project {ProjectId} ({Name})",
            notification.EventId, notification.ProjectId, notification.Name);

        return Task.CompletedTask;
    }
}

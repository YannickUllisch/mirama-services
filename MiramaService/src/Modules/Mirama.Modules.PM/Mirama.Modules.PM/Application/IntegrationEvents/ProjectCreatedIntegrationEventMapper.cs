using Mirama.Modules.PM.Contracts.Events;
using Mirama.Modules.PM.Domain.Events;
using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.Modules.PM.Application.IntegrationEvents;

internal sealed class ProjectCreatedIntegrationEventMapper : IIntegrationEventMapper<ProjectCreated>
{
    public IEnumerable<IIntegrationEvent> Map(ProjectCreated domainEvent)
    {
        yield return new ProjectCreatedEvent(
            EventId: Guid.NewGuid(),
            ProjectId: domainEvent.ProjectId,
            Name: domainEvent.Name);
    }
}

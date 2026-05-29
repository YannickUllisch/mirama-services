

using Mirama.SharedKernel.Abstractions.Domain.Events;

namespace Mirama.SharedKernel.Abstractions.Domain.Core;

public abstract class OrganizationAggregateRoot<TID> : OrganizationEntity<TID>, IDomainEventEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];

    protected void AddDomainEvent(IDomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.AsReadOnly();
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
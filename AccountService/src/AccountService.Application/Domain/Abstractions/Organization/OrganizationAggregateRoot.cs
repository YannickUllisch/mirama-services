

using AccountService.Application.Domain.Abstractions.Core;

namespace AccountService.Application.Domain.Abstractions.Organization;

public abstract class OrganizationAggregateRoot<TID> : OrganizationEntity<TID>
{
    private readonly List<DomainEvent> _domainEvents = [];

    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }

    public void ClearDomainEvents() => _domainEvents.Clear();
}
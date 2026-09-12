namespace Mirama.SharedKernel.Abstractions.Domain.Events;

public interface IDomainEventEntity
{
    /// <summary>Returns every event raised on this entity since the last call, clearing them in
    /// the same step — see <see cref="DomainEventList.DrainAll"/>.</summary>
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
}

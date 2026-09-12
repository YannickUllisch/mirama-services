namespace Mirama.SharedKernel.Abstractions.Domain.Events;

/// <summary>
/// The event buffer every <see cref="Core.Entity{TID}"/> holds — the "abstracted out list" that
/// makes <c>AddDomainEvent</c> available on any entity, root or child, with nothing to wire up
/// beyond inheriting <c>Entity&lt;TID&gt;</c> itself. <see cref="DrainAll"/> reads and clears in
/// one step, replacing the old separate <c>GetDomainEvents</c>/<c>ClearDomainEvents</c> pair on
/// <see cref="IDomainEventEntity"/> — nothing is left for a caller to forget to call.
/// </summary>
public sealed class DomainEventList
{
    private readonly List<IDomainEvent> _events = [];

    public void Add(IDomainEvent @event) => _events.Add(@event);

    public IReadOnlyCollection<IDomainEvent> DrainAll()
    {
        if (_events.Count == 0) return [];
        var events = _events.ToArray();
        _events.Clear();
        return events;
    }
}


namespace Mirama.SharedKernel.Abstractions.Domain.Events;

/// <summary>
/// Opt-in marker for an <see cref="IIntegrationEvent"/> whose delivery order matters
/// relative to other events of any ordered type for the same <see cref="IIntegrationEvent.AggregateId"/>
/// (e.g. "Created" must be delivered before "Renamed" for the same aggregate).
///
/// Most integration events should NOT implement this — design them as idempotent
/// snapshots (carry current state, not deltas) so out-of-order delivery converges
/// to the same result and you get full parallelism for free. Reach for this only
/// for the specific event types that genuinely need strict per-aggregate ordering.
///
/// OutboxProcessor&lt;TDbContext&gt; groups claimed messages of ordered types by
/// AggregateId and processes each group sequentially (in OccurredAtUtc order),
/// while different aggregates' groups — and all non-ordered events — run with
/// full bounded parallelism.
/// </summary>
public interface IOrderedIntegrationEvent : IIntegrationEvent
{
}

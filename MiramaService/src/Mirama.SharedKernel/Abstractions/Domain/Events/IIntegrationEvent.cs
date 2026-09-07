
namespace Mirama.SharedKernel.Abstractions.Domain.Events;

/// <summary>
/// The durable, public form of a fact — the data another handler, in the same
/// module or any other, can react to reliably, even across a crash or a
/// restart. Produced only by an IIntegrationEventMapper translating a domain
/// event (see IIntegrationEventMapper); never raised directly by an aggregate.
///
/// <see cref="Infrastructure.Persistence.AuditableUnitOfWorkDbContext"/> writes
/// one to the producing module's own Outbox in the same transaction as the
/// business change that produced it. From there, OutboxProcessor fans it out
/// into one InboxMessage row per registered handler, in that handler's owning
/// module's own Inbox, and each module's InboxProcessor delivers it to its own
/// handlers at-least-once, independently.
/// </summary>
public interface IIntegrationEvent : IDomainEvent
{
    Guid EventId { get; }

    Guid AggregateId { get; }
}

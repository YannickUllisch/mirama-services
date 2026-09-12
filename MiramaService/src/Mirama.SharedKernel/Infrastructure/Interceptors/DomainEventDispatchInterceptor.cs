using System.Diagnostics;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

namespace Mirama.SharedKernel.Infrastructure.Interceptors;

/// <summary>
/// Dispatches domain events raised by aggregates in the current change set, then translates
/// them into integration events (via <see cref="IIntegrationEventMapperResolver"/>) and writes
/// them to the outbox — all before the same <c>SaveChangesAsync</c> call persists them.
/// </summary>
public sealed class DomainEventDispatchInterceptor(
    IDispatcher dispatcher,
    IIntegrationEventMapperResolver integrationEventMapperResolver,
    IRequestContextProvider contextProvider) : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        if (eventData.Context is not null)
            await DispatchDomainEventsAsync(eventData.Context, ct);

        return await base.SavingChangesAsync(eventData, result, ct);
    }

    private async Task DispatchDomainEventsAsync(DbContext db, CancellationToken ct)
    {
        var domainEvents = db.ChangeTracker.Entries<IDomainEventEntity>()
            .Select(e => e.Entity)
            .SelectMany(entity => entity.GetDomainEvents())
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            if (domainEvent is IIntegrationEvent)
            {
                throw new InvalidOperationException(
                    $"{domainEvent.GetType().Name} implements IIntegrationEvent and was raised directly via " +
                    "AddDomainEvent. Aggregates raise plain IDomainEvent facts only; register an " +
                    $"IIntegrationEventMapper<{domainEvent.GetType().Name}> to produce the corresponding " +
                    "integration event(s) instead.");
            }

            await dispatcher.Publish(domainEvent, ct);
        }

        var integrationEvents = domainEvents
            .SelectMany(domainEvent => integrationEventMapperResolver.Map(domainEvent))
            .ToList();

        if (integrationEvents.Count == 0) return;

        var traceId = Activity.Current?.TraceId.ToString();
        var organizationId = contextProvider.OrganizationId;
        var tenantId = contextProvider.TenantId;

        db.Set<OutboxMessage>().AddRange(integrationEvents.Select(e => new OutboxMessage
        {
            Id = Guid.NewGuid(),
            AggregateId = e.AggregateId,
            OrganizationId = organizationId,
            TenantId = tenantId,
            TraceId = traceId,
            // No distinct correlation concept exists yet
            CorrelationId = traceId,
            OccurredAtUtc = e.OccurredAt,
            AvailableAtUtc = e.OccurredAt,
            Type = e.GetType().Name,
            Content = JsonSerializer.Serialize(e, e.GetType()),
        }));
    }
}

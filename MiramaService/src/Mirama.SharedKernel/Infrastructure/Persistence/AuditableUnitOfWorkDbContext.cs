using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Extensions;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

namespace Mirama.SharedKernel.Infrastructure.Persistence;

/// <summary>
/// Base DbContext shared by every module. Handles audit stamping (<see cref="IAuditable"/>),
/// tenant/organization ownership enforcement, domain event dispatch + outbox persistence, and
/// transaction lifecycle. Modules only supply their schema name and DbSets.
/// </summary>
public abstract class AuditableUnitOfWorkDbContext : DbContext, IUnitOfWork
{
    private readonly IDispatcher _dispatcher = default!;
    private readonly IRequestContextProvider _contextProvider = default!;
    private IDbContextTransaction? _currentTransaction;

    protected Guid? TenantId => _contextProvider?.TenantId;
    protected Guid? OrganizationId => _contextProvider?.OrganizationId;

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected abstract string SchemaName { get; }

    protected AuditableUnitOfWorkDbContext(
        DbContextOptions options,
        IDispatcher dispatcher,
        IRequestContextProvider requestContext) : base(options)
    {
        _dispatcher = dispatcher;
        _contextProvider = requestContext;
    }

    protected AuditableUnitOfWorkDbContext() { }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null) return;
        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is null) return;
        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(GetType())!);
        builder.HasDefaultSchema(SchemaName);
        builder.ApplyGlobalFilters(TenantId, OrganizationId);
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditable>();
        string actorId;
        try { actorId = _contextProvider.UserId.ToString(); }
        catch { actorId = "system"; }

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreated(DateTime.UtcNow, actorId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModified(DateTime.UtcNow, actorId);
                    break;
            }

            HandleEntityOwnership(entry);
        }

        var domainEvents = ChangeTracker.Entries<IDomainEventEntity>()
            .Select(e => e.Entity)
            .SelectMany(aggregate =>
            {
                var events = aggregate.GetDomainEvents();
                aggregate.ClearDomainEvents();
                return events;
            })
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _dispatcher.Publish(domainEvent, cancellationToken);
        }

        var outboxMessages = domainEvents
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredAtUtc = domainEvent.OccurredAt,
                Type = domainEvent.GetType().Name,
                Content = JsonSerializer.Serialize(domainEvent)
            })
            .ToList();

        if (outboxMessages.Count != 0)
        {
            OutboxMessages.AddRange(outboxMessages);
        }

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void HandleEntityOwnership(EntityEntry<IAuditable> entry)
    {
        if (entry.Entity is ITenantOwned tenantOwned)
        {
            if (TenantId is null)
                throw new UnauthorizedAccessException($"TenantId is required to persist {entry.Entity.GetType().Name}");

            if (entry.State == EntityState.Added)
                tenantOwned.SetTenantId(TenantId.Value);
            else if (entry.State == EntityState.Modified && tenantOwned.TenantId != TenantId)
                throw new InvalidOperationException("Changing TenantId is not allowed");
        }

        if (entry.Entity is IOrganizationOwned orgOwned && entry.State == EntityState.Added)
        {
            if (orgOwned.OrganizationId == Guid.Empty)
            {
                if (OrganizationId is null)
                    throw new InvalidOperationException($"OrganizationId is required to persist {entry.Entity.GetType().Name}");
                orgOwned.SetOrganizationId(OrganizationId.Value);
            }
        }
    }
}

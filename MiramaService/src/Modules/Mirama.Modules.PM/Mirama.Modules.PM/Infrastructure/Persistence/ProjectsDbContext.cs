using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Extensions;
using Mirama.SharedKernel.Infrastructure.Idempotency;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

namespace Mirama.Modules.Projects.Infrastructure.Persistence;

public sealed class ProjectsDbContext : DbContext, IUnitOfWork
{
    private readonly IDispatcher _dispatcher = default!;
    private readonly IRequestContextProvider _contextProvider = default!;
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _currentTransaction;

    private Guid? TenantId => _contextProvider?.TenantId;
    private Guid? OrganizationId => _contextProvider?.OrganizationId;

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    public ProjectsDbContext(
        DbContextOptions<ProjectsDbContext> options,
        IDispatcher dispatcher,
        IRequestContextProvider requestContext) : base(options)
    {
        _dispatcher = dispatcher;
        _contextProvider = requestContext;
    }

    public ProjectsDbContext() { }

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
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("projects");
        builder.ApplyGlobalFilters(TenantId, OrganizationId);
        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditable>();

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.SetCreated(DateTime.UtcNow, _contextProvider.UserId.ToString());
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModified(DateTime.UtcNow, _contextProvider.UserId.ToString());
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

        if (entry.Entity is IOrganizationOwned orgOwned && entry.State is EntityState.Added or EntityState.Modified)
        {
            if (OrganizationId is null)
                throw new InvalidOperationException($"OrganizationId is required to modify {entry.Entity.GetType().Name}");

            orgOwned.SetOrganizationId(OrganizationId.Value);
        }
    }
}

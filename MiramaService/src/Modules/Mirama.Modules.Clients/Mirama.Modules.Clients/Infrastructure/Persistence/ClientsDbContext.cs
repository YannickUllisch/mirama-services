using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Entities.ClientPortalInvitation;
using Mirama.Modules.Clients.Domain.Entities.ClientPortalUser;
using Mirama.Modules.Clients.Domain.Entities.Contact;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Extensions;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

namespace Mirama.Modules.Clients.Infrastructure.Persistence;

public sealed class ClientsDbContext : DbContext, IUnitOfWork
{
    private readonly IDispatcher _dispatcher = default!;
    private readonly IRequestContextProvider _contextProvider = default!;
    private IDbContextTransaction? _currentTransaction;

    private Guid? TenantId => _contextProvider?.TenantId;
    private Guid? OrganizationId => _contextProvider?.OrganizationId;

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<ClientPortalUser> PortalUsers => Set<ClientPortalUser>();
    public DbSet<ClientPortalInvitation> PortalInvitations => Set<ClientPortalInvitation>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    public ClientsDbContext(
        DbContextOptions<ClientsDbContext> options,
        IDispatcher dispatcher,
        IRequestContextProvider requestContext) : base(options)
    {
        _dispatcher = dispatcher;
        _contextProvider = requestContext;
    }

    public ClientsDbContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("clients");
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
            await _dispatcher.Publish(domainEvent, cancellationToken);

        var outboxMessages = domainEvents
            .Select(e => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredAtUtc = e.OccurredAt,
                Type = e.GetType().Name,
                Content = JsonSerializer.Serialize(e)
            })
            .ToList();

        if (outboxMessages.Count != 0)
            OutboxMessages.AddRange(outboxMessages);

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void HandleEntityOwnership(EntityEntry<IAuditable> entry)
    {
        if (entry.Entity is ITenantOwned tenantOwned)
        {
            if (TenantId is null)
                throw new UnauthorizedAccessException($"TenantId required for {entry.Entity.GetType().Name}");

            if (entry.State == EntityState.Added)
                tenantOwned.SetTenantId(TenantId.Value);
            else if (entry.State == EntityState.Modified && tenantOwned.TenantId != TenantId)
                throw new InvalidOperationException("Changing TenantId not allowed.");
        }

        if (entry.Entity is IOrganizationOwned orgOwned && entry.State == EntityState.Added)
        {
            if (orgOwned.OrganizationId == Guid.Empty)
            {
                if (OrganizationId is null)
                    throw new InvalidOperationException($"OrganizationId required for {entry.Entity.GetType().Name}");
                orgOwned.SetOrganizationId(OrganizationId.Value);
            }
        }
    }

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
}

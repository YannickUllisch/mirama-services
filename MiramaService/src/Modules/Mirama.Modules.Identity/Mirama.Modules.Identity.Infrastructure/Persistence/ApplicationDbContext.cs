
using System.Reflection;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Team;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;
using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Domain.Events;
using Mirama.SharedKernel.Infrastructure.Extensions;
using Mirama.SharedKernel.Infrastructure.Idempotency;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Infrastructure.Persistence;

public sealed class IdentityDbContext : DbContext, IUnitOfWork
{
    private readonly IDispatcher _dispatcher = default!;
    private readonly IRequestContextProvider _contextProvider = default!;
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _currentTransaction;
    private Guid? TenantId => _contextProvider?.TenantId;
    private Guid? OrganizationId => _contextProvider?.OrganizationId;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Policy> Policies => Set<Policy>();
    public DbSet<PolicyStatement> PolicyStatements => Set<PolicyStatement>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    public IdentityDbContext(
        DbContextOptions<IdentityDbContext> options,
        IDispatcher dispatcher,
        IRequestContextProvider requestContext) : base(options)
    {
        _dispatcher = dispatcher;
        _contextProvider = requestContext;
    }

    public IdentityDbContext() { }

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
        builder.HasDefaultSchema("account");

        builder.ApplyGlobalFilters(TenantId, OrganizationId);


        base.OnModelCreating(builder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<IAuditable>();

        // Handling Auditable Entities
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
                default:
                    break;
            }

            HandleEntityOwnership(entry);
        }

        // Persisting and Publishing Domain events in current transaction
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

    /// <summary>
    /// Validates that ownership properties (<see cref="TenantId"/> and <see cref="OrganizationId"/>) 
    /// are present and correct for auditable entities before they are persisted.
    /// Also automatically sets the tenant or organization IDs on newly added entities.
    /// </summary>
    /// <param name="entry">The IAuditable entity being validated</param>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown if a required TenantId is missing for a TenantOwned entity.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown if an TenantOwned entity attempts to change its TenantId>,
    /// or if an OrganizationOwned entity is persisted without a valid OrganizationId/>.
    /// </exception>
    /// <remarks>
    /// This method provides a second layer of security beyond authorization policies that 
    /// enforce tenant and organization claims in JWTs. It ensures that persisted entities
    /// cannot be written without valid multi-tenant/organization context.
    /// </remarks>

    private void HandleEntityOwnership(EntityEntry<IAuditable> entry)
    {
        if (entry.Entity is ITenantOwned tenantOwned)
        {
            if (TenantId is null)
            {
                throw new UnauthorizedAccessException($"TenantId is required to persist {entry.Entity.GetType().Name}");
            }

            if (entry.State == EntityState.Added)
            {
                tenantOwned.SetTenantId(TenantId.Value);
            }
            else if (entry.State == EntityState.Modified && tenantOwned.TenantId != TenantId)
            {
                throw new InvalidOperationException("Changing TenantId is not allowed");
            }
        }

        if (entry.Entity is IOrganizationOwned orgOwned && entry.State is EntityState.Added or EntityState.Modified)
        {
            if (OrganizationId is null)
            {
                throw new InvalidOperationException($"OrganizationId is required to modify {entry.Entity.GetType().Name}");
            }

            orgOwned.SetOrganizationId(OrganizationId.Value);
        }
    }
}

using System.Reflection;
using System.Text.Json;
using AccountService.Application.Common.Extensions;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Abstractions.Events;
using AccountService.Application.Domain.Aggregates.Account;
using AccountService.Application.Domain.Aggregates.Organization;
using AccountService.Application.Domain.Aggregates.Organization.Invitation;
using AccountService.Application.Domain.Aggregates.Organization.Member;
using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Domain.Aggregates.User;
using AccountService.Application.Infrastructure.Messaging.Outbox;
using AccountService.Application.Infrastructure.Persistence.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace AccountService.Application.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IMediator _mediator = default!;
    private readonly IRequestContextProvider _contextProvider = default!;
    private Guid? TenantId => _contextProvider?.TenantId;
    private Guid? OrganizationId => _contextProvider?.OrganizationId;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        IRequestContextProvider requestContext) : base(options)
    {
        _mediator = mediator;
        _contextProvider = requestContext;
    }

    public ApplicationDbContext() { }

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
            await _mediator.Publish(domainEvent, cancellationToken);
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
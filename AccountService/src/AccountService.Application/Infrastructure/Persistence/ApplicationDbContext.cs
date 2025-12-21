
using System.Reflection;
using System.Text.Json;
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Abstractions.Organization;
using AccountService.Application.Domain.Abstractions.Tenant;
using AccountService.Application.Domain.Aggregates.Organization;
using AccountService.Application.Domain.Aggregates.Organization.Invitation;
using AccountService.Application.Domain.Aggregates.Organization.Member;
using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Domain.Aggregates.User;
using AccountService.Application.Infrastructure.Common.Extensions;
using AccountService.Application.Infrastructure.Common.Interfaces;
using AccountService.Application.Infrastructure.Messaging.Outbox;
using AccountService.Application.Infrastructure.Persistence.Idempotency;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IRequestContextProvider _requestContext = default!;
    private readonly IMediator _mediator = default!;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<IdempotencyRecord> IdempotencyRecords => Set<IdempotencyRecord>();

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IMediator mediator,
        IRequestContextProvider requestContext) : base(options)
    {
        _requestContext = requestContext;
        _mediator = mediator;
    }

    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("auth");
        builder.ApplyTenantQueryFilter(_requestContext.TenantId);

        if (_requestContext.OrganizationId != null)
        {
            builder.ApplyOrganizationQueryFilter(_requestContext.OrganizationId.Value);
        }

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
                    entry.Entity.SetCreated(DateTime.UtcNow, _requestContext.UserId.ToString());
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModified(DateTime.UtcNow, _requestContext.UserId.ToString());
                    break;
                default:
                    break;
            }

            // Automatically add Organization Id to newly created and OrganizationScoped Entities
            if (entry.Entity is IOrganizationOwned orgScoped && entry.State == EntityState.Added && _requestContext.OrganizationId != null)
            {
                orgScoped.SetOrganizationId(_requestContext.OrganizationId.Value);
            }

            // Automatically add Tenant Id to newly created and TenantScoped Entities
            if (entry.Entity is ITenantOwned tenantScoped && entry.State == EntityState.Added)
            {
                tenantScoped.SetTenantId(_requestContext.TenantId);
            }
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
}
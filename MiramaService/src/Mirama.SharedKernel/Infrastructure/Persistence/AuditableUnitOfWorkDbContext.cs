
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Extensions;
using Mirama.SharedKernel.Infrastructure.Messaging.Inbox;
using Mirama.SharedKernel.Infrastructure.Messaging.Outbox;

namespace Mirama.SharedKernel.Infrastructure.Persistence;

/// <summary>
/// Base DbContext shared by every module. Owns transaction lifecycle, schema/model setup
/// (outbox/inbox tables, global tenant/org query filters), and DbSet access. Audit stamping,
/// tenant/organization ownership enforcement, domain event dispatch, and outbox writes are
/// handled by <c>SaveChangesInterceptor</c>s registered per module — see
/// <c>Infrastructure/Interceptors/AuditStampingInterceptor</c> and
/// <c>Infrastructure/Interceptors/DomainEventDispatchInterceptor</c>.
/// Modules only supply their schema name and DbSets.
/// </summary>
public abstract class AuditableUnitOfWorkDbContext : DbContext, IUnitOfWork
{
    private readonly IRequestContextProvider _contextProvider = default!;
    private IDbContextTransaction? _currentTransaction;

    protected Guid? TenantId => _contextProvider?.TenantId;
    protected Guid? OrganizationId => _contextProvider?.OrganizationId;

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<OutboxDeadLetter> OutboxDeadLetters => Set<OutboxDeadLetter>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    public DbSet<InboxDeadLetter> InboxDeadLetters => Set<InboxDeadLetter>();

    protected abstract string SchemaName { get; }

    protected AuditableUnitOfWorkDbContext(
        DbContextOptions options,
        IRequestContextProvider requestContext) : base(options)
    {
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

        builder.Entity<OutboxMessage>(e =>
        {
            e.Property(m => m.Headers).HasColumnType("jsonb");

            e.HasIndex(m => new { m.AvailableAtUtc, m.LockedUntilUtc })
             .HasDatabaseName($"IX_{SchemaName}_OutboxMessages_Claimable");
        });

        builder.Entity<OutboxDeadLetter>(e =>
        {
            e.Property(d => d.Headers).HasColumnType("jsonb");
            e.HasIndex(d => d.OrganizationId)
             .HasDatabaseName($"IX_{SchemaName}_OutboxDeadLetters_OrganizationId");
        });

        builder.Entity<InboxMessage>(e =>
        {
            e.Property(i => i.Headers).HasColumnType("jsonb");

            e.HasIndex(i => new { i.AvailableAtUtc, i.LockedUntilUtc })
             .HasDatabaseName($"IX_{SchemaName}_InboxMessages_Claimable");
        });

        builder.Entity<InboxDeadLetter>(e =>
        {
            e.Property(i => i.Headers).HasColumnType("jsonb");
            e.HasIndex(i => i.OrganizationId)
             .HasDatabaseName($"IX_{SchemaName}_InboxDeadLetters_OrganizationId");
        });

        base.OnModelCreating(builder);
    }
}


using System.Reflection;
using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Abstractions.Tenant;
using AccountService.Application.Domain.Organization;
using AccountService.Application.Domain.Organization.Invitation;
using AccountService.Application.Domain.User;
using AccountService.Application.Infrastructure.Common.Extensions;
using AccountService.Application.Infrastructure.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IUserContextService _userContext = default!;
    private readonly ITenantContextService _tenantContext = default!;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> User => Set<User>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> Members => Set<Member>();

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IUserContextService userContext,
        ITenantContextService tenantContext) : base(options)
    {
        _userContext = userContext;
        _tenantContext = tenantContext;
    }
    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("auth");
        builder.ApplyTenantQueryFilter(_tenantContext);

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
                    entry.Entity.SetCreated(DateTime.UtcNow, _userContext.UserId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModified(DateTime.UtcNow, _userContext.UserId);
                    break;
                default:
                    break;
            }

            // Automatically add Tenant Id to newly created and TenantScoped Entities
            if (entry.Entity is ITenantScoped tenantScoped && entry.State == EntityState.Added)
            {
                tenantScoped.SetOrganizationId(_tenantContext.OrganizationId);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
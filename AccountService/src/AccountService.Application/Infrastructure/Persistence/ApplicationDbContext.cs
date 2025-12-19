
using System.Reflection;
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
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly IRequestContextProvider _requestContext = default!;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> User => Set<User>();
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> Members => Set<Member>();

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        IRequestContextProvider requestContext) : base(options)
    {
        _requestContext = requestContext;
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

        return await base.SaveChangesAsync(cancellationToken);
    }
}
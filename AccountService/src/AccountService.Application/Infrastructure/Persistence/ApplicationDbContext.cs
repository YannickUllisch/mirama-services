
using System.Reflection;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Domain.Abstractions;
using AccountService.Application.Domain.Organization;
using AccountService.Application.Domain.Organization.Invitation;
using AccountService.Application.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService = default!;

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> User => Set<User>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> Members => Set<Member>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }
    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Scans for IEntityTypeConfigurations in the Assembly, allowing us to extract configs from here
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("auth");

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
                    entry.Entity.SetCreated(DateTime.UtcNow, _currentUserService.UserId);
                    break;
                case EntityState.Modified:
                    entry.Entity.SetModified(DateTime.UtcNow, _currentUserService.UserId);
                    break;
                default:
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
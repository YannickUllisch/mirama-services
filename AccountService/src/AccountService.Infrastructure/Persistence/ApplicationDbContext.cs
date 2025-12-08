
using System.Reflection;
using AccountService.Domain.Organization;
using AccountService.Domain.Organization.Invitation;
using AccountService.Domain.User;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Persistence;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<User> User => Set<User>();
    public DbSet<Invitation> Invitations => Set<Invitation>();
    public DbSet<Member> OrgMembers => Set<Member>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    public ApplicationDbContext() { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Scans for IEntityTypeConfigurations in the Assembly, allowing us to extract configs from here
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.HasDefaultSchema("auth");

        base.OnModelCreating(builder);
    }
}

using Microsoft.EntityFrameworkCore;

namespace AuthService.Server.Infrastructure.Persistence;

public sealed class OpenIdDbContext(
    DbContextOptions<OpenIdDbContext> options) : DbContext(options)
{
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("auth");
        base.OnModelCreating(builder);
    }
}
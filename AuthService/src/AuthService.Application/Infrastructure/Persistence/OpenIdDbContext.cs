
using Microsoft.EntityFrameworkCore;

namespace AuthService.Application.Infrastructure.Persistence;

public class OpenIdDbContext(DbContextOptions<OpenIdDbContext> options) : DbContext(options)
    {
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasDefaultSchema("auth");
        builder.UseOpenIddict();
        base.OnModelCreating(builder);
    }
}
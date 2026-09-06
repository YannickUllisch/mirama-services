
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace Mirama.Modules.Identity.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
{
    public IdentityDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<IdentityDbContext>();
        var connection = Environment.GetEnvironmentVariable("Infrastructure__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
        {
            throw new InvalidOperationException("Database connection string is not set.");
        }

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "identity"));

        return new IdentityDbContext(optionsBuilder.Options, null!, null!);
    }
}

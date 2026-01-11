
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;

namespace AuthService.Application.Infrastructure.Persistence;

public class OpenIdDbContextFactory : IDesignTimeDbContextFactory<OpenIdDbContext>
{
    public OpenIdDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<OpenIdDbContext>();
        var connection = Environment.GetEnvironmentVariable("Application__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(OpenIdDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistoryTemp", "auth"));

        return new OpenIdDbContext(optionsBuilder.Options);
    }
}

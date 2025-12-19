
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using DotNetEnv;
using AccountService.Application.Infrastructure.Services;

namespace AccountService.Application.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connection = Environment.GetEnvironmentVariable("Infrastructure__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "auth"));

        return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeUserProvider(), new DesignTimeOrganizationProvider());
    }
}

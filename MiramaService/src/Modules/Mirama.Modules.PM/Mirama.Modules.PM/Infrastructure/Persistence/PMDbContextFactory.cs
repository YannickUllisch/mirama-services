using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mirama.Modules.PM.Infrastructure.Persistence;

public class PMDbContextFactory : IDesignTimeDbContextFactory<PMDbContext>
{
    public PMDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<PMDbContext>();
        var connection = Environment.GetEnvironmentVariable("Infrastructure__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(PMDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "projects"));

        return new PMDbContext(optionsBuilder.Options, null!, null!);
    }
}

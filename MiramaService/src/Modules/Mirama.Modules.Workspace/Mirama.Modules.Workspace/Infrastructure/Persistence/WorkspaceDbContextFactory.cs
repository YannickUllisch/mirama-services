using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence;

public class WorkspaceDbContextFactory : IDesignTimeDbContextFactory<WorkspaceDbContext>
{
    public WorkspaceDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<WorkspaceDbContext>();
        var connection = Environment.GetEnvironmentVariable("Infrastructure__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(WorkspaceDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "workspace"));

        return new WorkspaceDbContext(optionsBuilder.Options, null!, null!);
    }
}

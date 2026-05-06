using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mirama.Modules.Projects.Infrastructure.Persistence;

public class ProjectsDbContextFactory : IDesignTimeDbContextFactory<ProjectsDbContext>
{
    public ProjectsDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<ProjectsDbContext>();
        var connection = Environment.GetEnvironmentVariable("Projects__Infrastructure__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(ProjectsDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "projects"));

        return new ProjectsDbContext(optionsBuilder.Options, null!, null!);
    }
}

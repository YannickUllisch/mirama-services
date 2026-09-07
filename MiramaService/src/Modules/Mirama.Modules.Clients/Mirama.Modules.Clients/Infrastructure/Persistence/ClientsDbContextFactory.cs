using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mirama.Modules.Clients.Infrastructure.Persistence;

public class ClientsDbContextFactory : IDesignTimeDbContextFactory<ClientsDbContext>
{
    public ClientsDbContext CreateDbContext(string[] args)
    {
        Env.TraversePath().Load();
        var optionsBuilder = new DbContextOptionsBuilder<ClientsDbContext>();
        var connection = Environment.GetEnvironmentVariable("Infrastructure__DatabaseConnection");

        if (string.IsNullOrWhiteSpace(connection))
            throw new InvalidOperationException("Database connection string is not set.");

        optionsBuilder.UseNpgsql(
            connection,
            b => b
                .MigrationsAssembly(typeof(ClientsDbContext).Assembly.FullName)
                .MigrationsHistoryTable("__EFMigrationsHistory", "clients"));

        return new ClientsDbContext(optionsBuilder.Options, null!);
    }
}

using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Infrastructure.Persistence;

internal sealed class ClientsModuleMigrator : IModuleMigrator
{
    private readonly ClientsDbContext _db;

    public ClientsModuleMigrator(ClientsDbContext db) => _db = db;

    public string ModuleName => "Clients";

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);
    }
}

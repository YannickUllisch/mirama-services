using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Infrastructure.Persistence;

internal sealed class PMModuleMigrator(PMDbContext db) : IModuleMigrator
{
    private readonly PMDbContext _db = db;

    public string ModuleName => "PM";

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);
    }
}

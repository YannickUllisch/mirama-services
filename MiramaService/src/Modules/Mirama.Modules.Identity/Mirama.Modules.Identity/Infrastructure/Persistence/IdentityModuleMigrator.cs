using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence.Seeding;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Infrastructure.Persistence;

internal sealed class IdentityModuleMigrator : IModuleMigrator
{
    private readonly IdentityDbContext _db;

    public IdentityModuleMigrator(IdentityDbContext db) => _db = db;

    public string ModuleName => "Identity";

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);
        await PolicySeed.SeedDataAsync(_db);
        await RoleSeed.SeedDataAsync(_db);
        await PlanSeed.SeedDataAsync(_db);
    }
}

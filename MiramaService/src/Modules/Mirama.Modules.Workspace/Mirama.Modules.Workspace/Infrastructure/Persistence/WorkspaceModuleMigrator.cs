using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence;

internal sealed class WorkspaceModuleMigrator(WorkspaceDbContext db) : IModuleMigrator
{
    private readonly WorkspaceDbContext _db = db;

    public string ModuleName => "Workspace";

    public async Task MigrateAsync(CancellationToken cancellationToken = default)
    {
        await _db.Database.MigrateAsync(cancellationToken);
    }
}

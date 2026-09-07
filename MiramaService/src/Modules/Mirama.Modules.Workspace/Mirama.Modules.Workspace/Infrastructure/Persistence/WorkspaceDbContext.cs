using Microsoft.EntityFrameworkCore;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Infrastructure.Persistence;

namespace Mirama.Modules.Workspace.Infrastructure.Persistence;

public sealed class WorkspaceDbContext : AuditableUnitOfWorkDbContext
{
    protected override string SchemaName => "workspace";

    public DbSet<Domain.Aggregates.ViewState.ViewState> ViewStates => Set<Domain.Aggregates.ViewState.ViewState>();

    public WorkspaceDbContext(
        DbContextOptions<WorkspaceDbContext> options,
        IRequestContextProvider requestContext) : base(options, requestContext)
    {
    }

    public WorkspaceDbContext() { }
}

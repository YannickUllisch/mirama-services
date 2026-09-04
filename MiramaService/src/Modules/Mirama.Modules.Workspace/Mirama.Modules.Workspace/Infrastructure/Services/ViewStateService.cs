using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Workspace.Contracts;
using Mirama.Modules.Workspace.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Workspace.Infrastructure.Services;

// Implements IModuleService so it is picked up automatically by the IModuleService scan in
// DependencyInjection.AddInfrastructure (same convention Identity uses for IMemberService etc.)
// and exposed to other modules purely through Contracts, per the synchronous cross-module
// pattern described in docs/mirama/modules/cross-module-communication.md.
internal sealed class ViewStateService(WorkspaceDbContext dbContext) : IViewStateService, IModuleService
{
    public async Task<ViewStateDto?> GetViewStateAsync(
        Guid userId,
        Guid organizationId,
        string surfaceKey,
        CancellationToken cancellationToken = default)
    {
        // Global org query filter already scopes this to the ambient request's organization;
        // organizationId is accepted explicitly here since callers of a cross-module contract
        // may not be running inside an HTTP request with that ambient context.
        var viewState = await dbContext.ViewStates
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(v => v.UserId == userId && v.OrganizationId == organizationId && v.SurfaceKey == surfaceKey)
            .FirstOrDefaultAsync(cancellationToken);

        if (viewState is null) return null;

        return new ViewStateDto(
            viewState.Id.Value,
            viewState.UserId,
            viewState.OrganizationId,
            viewState.SurfaceKey,
            viewState.ViewType.ToString(),
            viewState.StateJson,
            viewState.LastModified);
    }
}

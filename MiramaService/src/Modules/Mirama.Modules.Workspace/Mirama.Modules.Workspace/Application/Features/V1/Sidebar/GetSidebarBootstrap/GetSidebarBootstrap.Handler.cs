using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Contracts;
using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.Modules.Workspace.Application.Features.V1.ViewStates;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Workspace.Application.Features.V1.Sidebar.GetSidebarBootstrap;

public class GetSidebarBootstrapController : OrganizationControllerBase
{
    [HttpGet("sidebar-bootstrap")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetSidebarBootstrapQuery(), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetSidebarBootstrapQueryHandler(
    IWorkspaceQueryRepository<ViewState, ViewStateId> queryRepo,
    IClientService clientService,
    IRequestContextProvider context)
    : IRequestHandler<GetSidebarBootstrapQuery, ErrorOr<SidebarBootstrapResponse>>
{
    private const string SidebarSurfaceKey = "sidebar";

    public async Task<ErrorOr<SidebarBootstrapResponse>> HandleAsync(GetSidebarBootstrapQuery request, CancellationToken cancellationToken)
    {
        if (context.OrganizationId is not { } organizationId)
            return Error.Unauthorized("Sidebar.NoOrganization", "Organization context required.");

        var userId = context.UserId;

        var viewState = await queryRepo.Query()
            .Where(v => v.UserId == userId && v.SurfaceKey == SidebarSurfaceKey)
            .FirstOrDefaultAsync(cancellationToken);

        // Guid.Empty is a deliberate "not yet saved" sentinel - SaveViewState looks up by
        // (UserId, SurfaceKey), never by Id, so nothing depends on this round-tripping back.
        var sidebar = viewState is null
            ? new ViewStateResponse(Guid.Empty, SidebarSurfaceKey, ViewType.Sidebar, DefaultSidebarState.Json, null)
            : ViewStateMapper.ToResponse(viewState);

        var clients = await clientService.GetClientSummariesAsync(organizationId, cancellationToken);

        return new SidebarBootstrapResponse(sidebar, clients);
    }
}

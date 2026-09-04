using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.GetViewState;

public class GetViewStateController : OrganizationControllerBase
{
    [HttpGet("view-state/{surfaceKey}")]
    public async Task<IActionResult> GetBySurfaceKey([FromRoute] string surfaceKey, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetViewStateQuery(surfaceKey), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetViewStateQueryHandler(
    IWorkspaceQueryRepository<ViewState, ViewStateId> queryRepo,
    IRequestContextProvider context)
    : IRequestHandler<GetViewStateQuery, ErrorOr<ViewStateResponse?>>
{
    public async Task<ErrorOr<ViewStateResponse?>> HandleAsync(GetViewStateQuery request, CancellationToken cancellationToken)
    {
        var userId = context.UserId;

        var viewState = await queryRepo.Query()
            .Where(v => v.UserId == userId && v.SurfaceKey == request.SurfaceKey)
            .FirstOrDefaultAsync(cancellationToken);

        return viewState is null ? (ViewStateResponse?)null : ViewStateMapper.ToResponse(viewState);
    }
}

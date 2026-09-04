using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.GetViewStates;

public class GetViewStatesController : OrganizationControllerBase
{
    [HttpGet("view-state")]
    public async Task<IActionResult> GetBySurfaceKeys([FromQuery] string keys, CancellationToken ct)
    {
        var surfaceKeys = (keys ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Distinct()
            .ToList();

        var result = await Dispatcher.Send(new GetViewStatesQuery(surfaceKeys), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetViewStatesQueryHandler(
    IWorkspaceQueryRepository<ViewState, ViewStateId> queryRepo,
    IRequestContextProvider context)
    : IRequestHandler<GetViewStatesQuery, ErrorOr<List<ViewStateResponse>>>
{
    public async Task<ErrorOr<List<ViewStateResponse>>> HandleAsync(GetViewStatesQuery request, CancellationToken cancellationToken)
    {
        if (request.SurfaceKeys.Count == 0)
            return new List<ViewStateResponse>();

        var userId = context.UserId;

        var viewStates = await queryRepo.Query()
            .Where(v => v.UserId == userId && request.SurfaceKeys.Contains(v.SurfaceKey))
            .ToListAsync(cancellationToken);

        return viewStates.Select(ViewStateMapper.ToResponse).ToList();
    }
}

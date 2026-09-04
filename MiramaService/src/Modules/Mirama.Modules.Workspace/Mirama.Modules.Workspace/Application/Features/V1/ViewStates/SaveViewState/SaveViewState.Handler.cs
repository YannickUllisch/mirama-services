using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Workspace.Application.Common.Interfaces;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.SaveViewState;

public class SaveViewStateController : OrganizationControllerBase
{
    [HttpPut("view-state/{surfaceKey}")]
    public async Task<IActionResult> Save(
        [FromRoute] string surfaceKey,
        [FromBody] SaveViewStateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { SurfaceKey = surfaceKey };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class SaveViewStateCommandHandler(
    IWorkspaceCommandRepository<ViewState, ViewStateId> commandRepo,
    IRequestContextProvider context)
    : IRequestHandler<SaveViewStateCommand, ErrorOr<ViewStateResponse>>
{
    public async Task<ErrorOr<ViewStateResponse>> HandleAsync(SaveViewStateCommand request, CancellationToken cancellationToken)
    {
        var userId = context.UserId;

        var existing = await commandRepo.Query()
            .Where(v => v.UserId == userId && v.SurfaceKey == request.SurfaceKey)
            .FirstOrDefaultAsync(cancellationToken);

        if (existing is not null)
        {
            existing.ReplaceState(request.ViewType, request.StateJson);
            commandRepo.Update(existing);
            return ViewStateMapper.ToResponse(existing);
        }

        var viewState = ViewState.Create(new ViewStateDetails(
            userId,
            request.SurfaceKey,
            request.ViewType,
            request.StateJson));

        commandRepo.Add(viewState);
        return ViewStateMapper.ToResponse(viewState);
    }
}

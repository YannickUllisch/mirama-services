using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates;

public sealed record ViewStateResponse(
    Guid Id,
    string SurfaceKey,
    ViewType ViewType,
    string StateJson,
    DateTime? LastModified);

internal static class ViewStateMapper
{
    internal static ViewStateResponse ToResponse(ViewState viewState) =>
        new(
            viewState.Id.Value,
            viewState.SurfaceKey,
            viewState.ViewType,
            viewState.StateJson,
            viewState.LastModified);
}

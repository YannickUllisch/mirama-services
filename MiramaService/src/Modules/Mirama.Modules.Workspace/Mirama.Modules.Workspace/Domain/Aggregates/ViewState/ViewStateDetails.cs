namespace Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

public sealed record ViewStateDetails(
    Guid UserId,
    string SurfaceKey,
    ViewType ViewType,
    string StateJson);

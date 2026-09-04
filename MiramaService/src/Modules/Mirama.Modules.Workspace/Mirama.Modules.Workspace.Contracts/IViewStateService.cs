namespace Mirama.Modules.Workspace.Contracts;

/// <summary>
/// Public surface other modules use to read a user's saved view state without
/// depending on the Workspace module's internals. See
/// Mirama.Modules.Workspace.Infrastructure.Services.ViewStateService for the implementation.
/// </summary>
public interface IViewStateService
{
    Task<ViewStateDto?> GetViewStateAsync(
        Guid userId,
        Guid organizationId,
        string surfaceKey,
        CancellationToken cancellationToken = default);
}

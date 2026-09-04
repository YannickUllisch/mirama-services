using Mirama.SharedKernel.Abstractions.Domain.Core;
using Mirama.SharedKernel.Abstractions.Domain.Exceptions;

namespace Mirama.Modules.Workspace.Domain.Aggregates.ViewState;

/// <summary>
/// A single user's saved personalization for one "surface" of the product - the global
/// sidebar, a table's column layout, a kanban board's per-user column order/collapse state,
/// a saved filter view, etc. Deliberately generic: the surface being personalized (a project,
/// a board, a table view) is referenced only by an opaque <see cref="SurfaceKey"/>, never a
/// foreign key, so this module stays decoupled from every other module's domain model.
///
/// The shared resource being personalized (e.g. PM's KanbanBoard/KanbanColumn) always stays
/// the source of truth for what exists; this row only ever holds *this user's* view of it.
/// </summary>
public sealed class ViewState : OrganizationAggregateRoot<ViewStateId>
{
    public Guid UserId { get; private set; }
    public string SurfaceKey { get; private set; } = string.Empty;
    public ViewType ViewType { get; private set; }
    public string StateJson { get; private set; } = string.Empty;

    private ViewState() { }

    private ViewState(ViewStateDetails details)
    {
        this.UserId = details.UserId;
        this.SurfaceKey = NormalizeSurfaceKey(details.SurfaceKey);
        this.ViewType = details.ViewType;
        this.StateJson = details.StateJson;
    }

    public static ViewState Create(ViewStateDetails details) =>
        new ViewState(details) { Id = new ViewStateId(Guid.NewGuid()) };

    /// <summary>
    /// Replaces the saved state wholesale. Callers PUT a full replacement rather than
    /// patching individual fields - this keeps write volume predictable (one write per
    /// debounced client-side change, not one per drag event) and avoids merge semantics
    /// for what is, ultimately, opaque client-owned JSON.
    /// </summary>
    public void ReplaceState(ViewType viewType, string stateJson)
    {
        this.ViewType = viewType;
        this.StateJson = stateJson;
    }

    private static string NormalizeSurfaceKey(string surfaceKey)
    {
        if (string.IsNullOrWhiteSpace(surfaceKey))
            throw new DomainValidationException(nameof(SurfaceKey), "Surface key is required.");

        return surfaceKey.Trim();
    }
}

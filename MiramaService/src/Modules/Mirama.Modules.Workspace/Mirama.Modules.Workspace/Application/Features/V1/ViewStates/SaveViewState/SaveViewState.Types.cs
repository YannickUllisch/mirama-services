using ErrorOr;
using Mirama.Modules.Workspace.Domain.Aggregates.ViewState;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.SaveViewState;

// Full-state replace (PUT), not a per-field PATCH: a sidebar drag or a column reorder fires
// many intermediate states client-side. The client debounces and sends one replacement: this
// keeps write volume - and therefore cache invalidation - predictable regardless of how
// granular the UI interaction is.
public sealed record SaveViewStateCommand(
    string SurfaceKey,
    ViewType ViewType,
    string StateJson) : ICommand<ErrorOr<ViewStateResponse>>;

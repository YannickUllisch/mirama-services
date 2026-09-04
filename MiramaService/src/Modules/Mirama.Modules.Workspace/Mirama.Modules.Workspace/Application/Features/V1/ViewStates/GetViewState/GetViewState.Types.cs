using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.GetViewState;

// Response is nullable-by-design: "no view state saved yet" is the normal, expected state
// for a brand new user or a surface nobody has personalized yet - not an error condition.
// Clients treat a null body as "fall back to defaults" rather than branching on a 404.
public sealed record GetViewStateQuery(string SurfaceKey) : IQuery<ErrorOr<ViewStateResponse?>>;

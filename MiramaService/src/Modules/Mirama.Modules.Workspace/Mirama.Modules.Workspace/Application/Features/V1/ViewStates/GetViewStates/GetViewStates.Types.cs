using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.GetViewStates;

/// <summary>
/// Bootstrap/batch lookup - the SPA shell fetches every view-state it needs for first paint
/// (sidebar + whichever tables/boards are about to render) in one round trip instead of one
/// request per widget. Keys with no saved state are simply absent from the result.
/// </summary>
public sealed record GetViewStatesQuery(IReadOnlyList<string> SurfaceKeys) : IQuery<ErrorOr<List<ViewStateResponse>>>;

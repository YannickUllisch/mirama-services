using ErrorOr;
using Mirama.Modules.Clients.Contracts.Dtos;
using Mirama.Modules.Workspace.Application.Features.V1.ViewStates;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Workspace.Application.Features.V1.Sidebar.GetSidebarBootstrap;

/// <summary>
/// Single composed read for first paint: the sidebar's personalization state (saved, or the
/// computed default if the user hasn't customized anything yet) plus the org's full client
/// list, resolved via Clients.Contracts in one in-process call rather than a second network
/// hop from the browser. Keeps "Your clients" membership always live - a rename or a newly
/// created client shows up immediately, nothing about it is cached or duplicated here.
/// </summary>
public sealed record GetSidebarBootstrapQuery : IQuery<ErrorOr<SidebarBootstrapResponse>>;

public sealed record SidebarBootstrapResponse(
    ViewStateResponse Sidebar,
    List<ClientSummaryDto> Clients);

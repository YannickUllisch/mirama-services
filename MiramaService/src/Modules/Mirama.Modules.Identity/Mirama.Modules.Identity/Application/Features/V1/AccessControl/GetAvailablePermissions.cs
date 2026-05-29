using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl;

public class GetAvailablePermissionsController : TenantControllerBase
{
    [HttpGet("available-permissions")]
    public async Task<ActionResult<AvailablePermissionsResponse>> Get()
    {
        var result = await this.Dispatcher.Send(new GetAvailablePermissionsQuery());
        return result.Match(Ok, Problem);
    }
}

public sealed record GetAvailablePermissionsQuery : IQuery<ErrorOr<AvailablePermissionsResponse>>;

internal class GetAvailablePermissionsQueryHandler
    : IRequestHandler<GetAvailablePermissionsQuery, ErrorOr<AvailablePermissionsResponse>>
{
    private static readonly AvailablePermissionsResponse Cached = new()
    {
        Effects = ["Allow", "Deny"],
        Groups = Permissions.AllGroups
            .Select(g => new PermissionGroupResponse
            {
                Label = g.Label,
                Scope = g.Scope,
                ResourcePattern = g.ResourcePattern,
                AllActionsPattern = g.AllActionsPattern,
                Actions = g.Actions
                    .Select(a => new PermissionActionResponse { Action = a.Action, Label = a.Label })
                    .ToList(),
            })
            .ToList(),
    };

    public Task<ErrorOr<AvailablePermissionsResponse>> HandleAsync(
        GetAvailablePermissionsQuery request, CancellationToken ct)
        => Task.FromResult(ErrorOrFactory.From(Cached));
}

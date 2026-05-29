using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mirama.SharedKernel.Abstractions.Permissions;

namespace Mirama.SharedKernel.Models.Permissions;

internal sealed class PermissionAuthorizationFilter(IPermissionService permissionService) : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var attribute = context.ActionDescriptor.EndpointMetadata
            .OfType<RequirePermissionAttribute>()
            .FirstOrDefault();

        if (attribute is null) return;

        var projectId = context.HttpContext.Request.RouteValues["projectId"] is string raw
            && Guid.TryParse(raw, out var pid) ? pid : (Guid?)null;

        var granted = await permissionService.HasPermissionAsync(
            context.HttpContext.User,
            attribute.Permissions,
            projectId);

        if (!granted)
            context.Result = new ForbidResult();
    }
}

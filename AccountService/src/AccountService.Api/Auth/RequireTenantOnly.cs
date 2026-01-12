using Microsoft.AspNetCore.Authorization;

namespace AccountService.Api.Auth;

public class TenantOnlyRequirement : IAuthorizationRequirement { }

public class RequireTenantOnlyHandler : AuthorizationHandler<TenantOnlyRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantOnlyRequirement requirement)
    {
        var user = context.User;

        // Always allow client credentials tokens
        var clientId = user.FindFirst("client_id")?.Value ?? user.FindFirst("azp")?.Value;
        if (!string.IsNullOrEmpty(clientId))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Require tenantId for user tokens
        var tenantId = user.FindFirst("tenantId")?.Value;
        if (!string.IsNullOrEmpty(tenantId))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}
using Microsoft.AspNetCore.Authorization;

namespace Mirama.Api.Auth;

public class TenantAndOrgRequirement : IAuthorizationRequirement { }

public class RequireTenantAndOrgHandler : AuthorizationHandler<TenantAndOrgRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TenantAndOrgRequirement requirement)
    {
        var user = context.User;

        // Always allow client credentials tokens
        var clientId = user.FindFirst("client_id")?.Value ?? user.FindFirst("azp")?.Value;
        if (!string.IsNullOrEmpty(clientId))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Require both tenantId and orgId for user tokens
        var tenantId = user.FindFirst("tenantId")?.Value;
        var orgId = user.FindFirst("organizationId")?.Value;
        if (!string.IsNullOrEmpty(tenantId) && !string.IsNullOrEmpty(orgId))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}
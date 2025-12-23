using Microsoft.AspNetCore.Authorization;

namespace AccountService.Api.Authorization;

public class JWTRequirements : IAuthorizationRequirement { }

public class TenantAndOrgOnUserHandler : AuthorizationHandler<JWTRequirements>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JWTRequirements requirement)
    {
        var user = context.User;

        // If the token has a tenantId, allow
        var tenantId = user.FindFirst("tenantId")?.Value;
        if (!string.IsNullOrEmpty(tenantId))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // If the token is a client credentials token (no "tenantId" and "client_id" claim exists), allow
        var clientId = user.FindFirst("client_id")?.Value ?? user.FindFirst("azp")?.Value;
        if (!string.IsNullOrEmpty(clientId))
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        // Otherwise, fail
        context.Fail();
        return Task.CompletedTask;
    }
}
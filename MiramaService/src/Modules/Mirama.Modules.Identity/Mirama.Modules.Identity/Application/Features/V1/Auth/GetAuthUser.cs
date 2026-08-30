using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

[AllowAnonymous]
public class GetAuthUserController : ApiControllerBase
{
    [HttpGet("auth/user/{externalId:guid}")]
    public async Task<ActionResult<AuthUserResponse>> Get([FromRoute] Guid externalId)
    {
        var res = await this.Dispatcher.Send(new GetAuthUserQuery(externalId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetAuthUserQuery(Guid ExternalId) : IQuery<ErrorOr<AuthUserResponse>>;

internal class GetAuthUserQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetAuthUserQuery, ErrorOr<AuthUserResponse>>
{
    public async Task<ErrorOr<AuthUserResponse>> HandleAsync(GetAuthUserQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.LinkedExternalIds.Contains(request.ExternalId), ct);

        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User not found.");
        }

        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.AdminUserId == user.Id, ct);

        if (tenant is null)
        {
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");
        }
        var tenantRole = user.Id.Value == tenant?.AdminUserId.Value ? TenantRole.Owner : TenantRole.Assumed;

        // TODO: should use a favorite, frontend localstorage or default approach instead of most-recent-first.
        var members = await dbContext.Members
            .AsNoTracking()
            .Where(m => m.UserId == user.Id)
            .OrderByDescending(m => m.Created)
            .ToListAsync(ct);

        AuthOrgMembershipResponse? organizationInfo = null;
        foreach (var member in members)
        {
            var org = await dbContext.Organizations
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == new OrganizationId(member.OrganizationId), ct);

            if (org is not null)
            {
                organizationInfo = org.MapOrgMembershipResponse(member, tenantRole);
                break;
            }
        }

        return user.MapAuthUserResponse(tenant!.Id, tenantRole, organizationInfo);
    }
}

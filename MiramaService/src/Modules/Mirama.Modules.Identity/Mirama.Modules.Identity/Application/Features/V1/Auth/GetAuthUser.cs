using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
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

        var members = await dbContext.Members
            .AsNoTracking()
            .Where(m => m.UserId == user.Id)
            .ToListAsync(ct);

        AuthOrgMembershipResponse? organizationInfo = null;
        if (members.Count == 1)
        {
            var member = members[0];
            var org = await dbContext.Organizations
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == new OrganizationId(member.OrganizationId), ct);

            if (org is not null)
            {
                organizationInfo = org.MapOrgMembershipResponse(member);
            }
        }

        return user.MapAuthUserResponse(tenant.Id, organizationInfo);
    }
}

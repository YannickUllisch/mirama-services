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
public class GetOrgMembershipController : ApiControllerBase
{
    [HttpGet("auth/user/{externalId:guid}/organization/{organizationId:guid}")]
    public async Task<ActionResult<AuthOrgMembershipResponse>> Get(
        [FromRoute] Guid externalId,
        [FromRoute] Guid organizationId)
    {
        var res = await this.Dispatcher.Send(new GetOrgMembershipQuery(externalId, organizationId));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetOrgMembershipQuery(Guid ExternalId, Guid OrganizationId)
    : IQuery<ErrorOr<AuthOrgMembershipResponse>>;

internal class GetOrgMembershipQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetOrgMembershipQuery, ErrorOr<AuthOrgMembershipResponse>>
{
    public async Task<ErrorOr<AuthOrgMembershipResponse>> HandleAsync(GetOrgMembershipQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.LinkedExternalIds.Contains(request.ExternalId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var member = await dbContext.Members
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.UserId == user.Id && m.OrganizationId == request.OrganizationId, ct);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Membership not found.");

        var org = await dbContext.Organizations
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(request.OrganizationId), ct);

        if (org is null)
            return Error.NotFound("Organization.NotFound", "Organization not found.");

        return org.MapOrgMembershipResponse(member);
    }
}

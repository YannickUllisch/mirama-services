using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

[AllowAnonymous]
public class GetOrgMembershipController : ApiControllerBase
{
    [HttpGet("auth/user/{externalId:guid}/organization/{slug}")]
    public async Task<ActionResult<AuthOrgMembershipResponse>> Get(
        [FromRoute] Guid externalId,
        [FromRoute] string slug)
    {
        var res = await this.Dispatcher.Send(new GetOrgMembershipQuery(externalId, slug));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetOrgMembershipQuery(Guid ExternalId, string Slug)
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

        var normalizedSlug = request.Slug.Trim().ToLowerInvariant();

        var org = await dbContext.Organizations
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Slug == normalizedSlug, ct);

        if (org is null)
            return Error.NotFound("Organization.NotFound", "Organization not found.");

        var member = await dbContext.Members
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(m => m.UserId == user.Id && m.OrganizationId == org.Id.Value, ct);

        if (member is null)
            return Error.NotFound("Member.NotFound", "Membership not found.");

        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .Select(x => new { x.Id, x.AdminUserId })
            .FirstOrDefaultAsync(o => o.Id == org.TenantId, ct);

        var tenantRole = user.Id.Value == tenant?.AdminUserId.Value ? TenantRole.Owner : TenantRole.Assumed;

        return org.MapOrgMembershipResponse(member, tenantRole);
    }
}

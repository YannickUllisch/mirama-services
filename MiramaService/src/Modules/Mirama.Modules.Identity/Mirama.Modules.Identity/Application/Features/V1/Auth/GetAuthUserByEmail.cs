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
public class GetAuthUserByEmailController : ApiControllerBase
{
    [HttpGet("auth/user/by-email")]
    public async Task<ActionResult<AuthUserResponse>> Get([FromQuery] string email)
    {
        var res = await this.Dispatcher.Send(new GetAuthUserByEmailQuery(email));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetAuthUserByEmailQuery(string Email) : IQuery<ErrorOr<AuthUserResponse>>;

internal class GetAuthUserByEmailQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetAuthUserByEmailQuery, ErrorOr<AuthUserResponse>>
{
    public async Task<ErrorOr<AuthUserResponse>> HandleAsync(GetAuthUserByEmailQuery request, CancellationToken ct)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u =>
                u.Email.ToLower() == normalizedEmail &&
                u.EmailVerified != null, ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var tenant = await dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.AdminUserId == user.Id, ct);

        if (tenant is null)
            return Error.NotFound("Tenant.NotFound", "Tenant not found.");

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
                organizationInfo = org.MapOrgMembershipResponse(member);
        }

        return user.MapAuthUserResponse(tenant.Id, organizationInfo);
    }
}

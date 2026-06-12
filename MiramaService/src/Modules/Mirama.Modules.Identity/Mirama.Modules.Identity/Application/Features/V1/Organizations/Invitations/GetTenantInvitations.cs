using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations;

public class GetTenantInvitationsController : TenantControllerBase
{
    [HttpGet("invitations")]
    public async Task<ActionResult<List<InvitationResponse>>> Get()
    {
        var res = await this.Dispatcher.Send(new GetTenantInvitationsQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetTenantInvitationsQuery : IQuery<ErrorOr<List<InvitationResponse>>>;

internal class GetTenantInvitationsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTenantInvitationsQuery, ErrorOr<List<InvitationResponse>>>
{
    public async Task<ErrorOr<List<InvitationResponse>>> HandleAsync(GetTenantInvitationsQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == new UserId(contextProvider.UserId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var tenantId = contextProvider.TenantId!.Value;

        var results = await (
            from i in dbContext.Invitations.AsNoTracking().IgnoreQueryFilters()
            join o in dbContext.Organizations.AsNoTracking().IgnoreQueryFilters()
                on i.OrganizationId equals o.Id.Value
            where i.Email == user.Email
                && i.Status == InvitationStatus.Pending
                && i.ExpiresAt > DateTime.UtcNow
                && o.TenantId == tenantId
            orderby i.ExpiresAt descending
            select new { Invitation = i, OrgName = o.Name }
        ).ToListAsync(ct);

        return results.ConvertAll(r => r.Invitation.MapResponse(r.OrgName));
    }
}

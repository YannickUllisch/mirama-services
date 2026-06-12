using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations;

public class GetInvitationsController : OrganizationControllerBase
{
    [HttpGet("invitations")]
    public async Task<ActionResult<List<InvitationResponse>>> Get([FromQuery] string? status)
    {
        var res = await this.Dispatcher.Send(new GetInvitationsQuery(status));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetInvitationsQuery(string? Status) : IQuery<ErrorOr<List<InvitationResponse>>>;

internal class GetInvitationsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetInvitationsQuery, ErrorOr<List<InvitationResponse>>>
{
    public async Task<ErrorOr<List<InvitationResponse>>> HandleAsync(GetInvitationsQuery request, CancellationToken ct)
    {
        var org = await dbContext.Organizations.AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(contextProvider.OrganizationId!.Value), ct);

        var orgName = org?.Name ?? string.Empty;

        var query = dbContext.Invitations.AsNoTracking();

        if (request.Status is not null && Enum.TryParse<InvitationStatus>(request.Status, ignoreCase: true, out var status))
            query = query.Where(i => i.Status == status);

        var invitations = await query
            .OrderByDescending(i => i.ExpiresAt)
            .ToListAsync(ct);

        return invitations.ConvertAll(i => i.MapResponse(orgName));
    }
}

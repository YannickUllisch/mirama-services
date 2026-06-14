using ErrorOr;
using FluentValidation;
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
    public async Task<ActionResult<PaginatedList<InvitationResponse>>> Get([FromQuery] GetTenantInvitationsQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetTenantInvitationsQuery : IQuery<ErrorOr<PaginatedList<InvitationResponse>>>
{
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetTenantInvitationsQueryValidator : AbstractValidator<GetTenantInvitationsQuery>
{
    public GetTenantInvitationsQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetTenantInvitationsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetTenantInvitationsQuery, ErrorOr<PaginatedList<InvitationResponse>>>
{
    public async Task<ErrorOr<PaginatedList<InvitationResponse>>> HandleAsync(GetTenantInvitationsQuery request, CancellationToken ct)
    {
        var user = await dbContext.Users
            .AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Id == new UserId(contextProvider.UserId), ct);

        if (user is null)
            return Error.NotFound("User.NotFound", "User not found.");

        var tenantId = contextProvider.TenantId!.Value;

        var tenantOrgs = await dbContext.Organizations
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(o => o.TenantId == tenantId)
            .Select(o => new { Id = o.Id.Value, o.Name })
            .ToDictionaryAsync(o => o.Id, o => o.Name, ct);

        var tenantOrgIds = tenantOrgs.Keys.ToList();

        var invitationQuery = dbContext.Invitations
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(i => i.Email == user.Email
                && i.Status == InvitationStatus.Pending
                && i.ExpiresAt > DateTime.UtcNow
                && tenantOrgIds.Contains(i.OrganizationId))
            .OrderByDescending(i => i.ExpiresAt);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await invitationQuery.CountAsync(ct);
            var page = await invitationQuery.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<InvitationResponse>(page.ConvertAll(i => i.MapResponse(tenantOrgs.GetValueOrDefault(i.OrganizationId, string.Empty))), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var results = await invitationQuery.ToListAsync(ct);
        return new PaginatedList<InvitationResponse>(results.ConvertAll(i => i.MapResponse(tenantOrgs.GetValueOrDefault(i.OrganizationId, string.Empty))), results.Count, 1, results.Count > 0 ? results.Count : 1);
    }
}

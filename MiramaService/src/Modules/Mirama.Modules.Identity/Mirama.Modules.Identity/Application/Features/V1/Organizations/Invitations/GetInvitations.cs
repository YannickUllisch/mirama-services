using ErrorOr;
using FluentValidation;
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
    public async Task<ActionResult<PaginatedList<InvitationResponse>>> Get([FromQuery] GetInvitationsQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetInvitationsQuery : IQuery<ErrorOr<PaginatedList<InvitationResponse>>>
{
    public string? Status { get; init; }
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetInvitationsQueryValidator : AbstractValidator<GetInvitationsQuery>
{
    public GetInvitationsQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetInvitationsQueryHandler(
    IdentityDbContext dbContext,
    IRequestContextProvider contextProvider) : IRequestHandler<GetInvitationsQuery, ErrorOr<PaginatedList<InvitationResponse>>>
{
    public async Task<ErrorOr<PaginatedList<InvitationResponse>>> HandleAsync(GetInvitationsQuery request, CancellationToken ct)
    {
        var org = await dbContext.Organizations.AsNoTracking()
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Id == new OrganizationId(contextProvider.OrganizationId!.Value), ct);

        var orgName = org?.Name ?? string.Empty;

        var query = dbContext.Invitations.AsNoTracking();

        if (request.Status is not null && Enum.TryParse<InvitationStatus>(request.Status, ignoreCase: true, out var status))
            query = query.Where(i => i.Status == status);

        var orderedQuery = query.OrderByDescending(i => i.ExpiresAt);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await orderedQuery.CountAsync(ct);
            var page = await orderedQuery.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<InvitationResponse>(page.ConvertAll(i => i.MapResponse(orgName)), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var items = await orderedQuery.ToListAsync(ct);
        return new PaginatedList<InvitationResponse>(items.ConvertAll(i => i.MapResponse(orgName)), items.Count, 1, items.Count > 0 ? items.Count : 1);
    }
}

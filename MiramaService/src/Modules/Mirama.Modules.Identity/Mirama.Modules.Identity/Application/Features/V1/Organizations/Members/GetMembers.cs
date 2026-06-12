using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;

public class GetMembersController : OrganizationControllerBase
{
    [HttpGet("members")]
    public async Task<ActionResult<PaginatedList<MemberResponse>>> Get([FromQuery] GetMembersQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetMembersQuery : IQuery<ErrorOr<PaginatedList<MemberResponse>>>
{
    public int? PageSize { get; init; }
    public int? PageNumber { get; init; }
}

internal class GetMembersQueryValidator : AbstractValidator<GetMembersQuery>
{
    public GetMembersQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);
    }
}

internal class GetMembersQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetMembersQuery, ErrorOr<PaginatedList<MemberResponse>>>
{
    public async Task<ErrorOr<PaginatedList<MemberResponse>>> HandleAsync(GetMembersQuery request, CancellationToken ct)
    {
        var query = dbContext.Members.AsNoTracking().OrderBy(m => m.Name);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await query.CountAsync(ct);
            var page = await query.Skip((request.PageNumber.Value - 1) * request.PageSize.Value).Take(request.PageSize.Value).ToListAsync(ct);
            return new PaginatedList<MemberResponse>(page.ConvertAll(m => m.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var items = await query.ToListAsync(ct);
        return new PaginatedList<MemberResponse>(items.ConvertAll(m => m.MapResponse()), items.Count, 1, items.Count > 0 ? items.Count : 1);
    }
}

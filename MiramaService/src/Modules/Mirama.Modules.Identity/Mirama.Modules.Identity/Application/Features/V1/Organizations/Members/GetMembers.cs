using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;

public class GetMembersController : OrganizationControllerBase
{
    [HttpGet("members")]
    public async Task<ActionResult<List<MemberResponse>>> Get()
    {
        var res = await this.Dispatcher.Send(new GetMembersQuery());
        return res.Match(Ok, Problem);
    }
}

public sealed record GetMembersQuery : IQuery<ErrorOr<List<MemberResponse>>>;

internal class GetMembersQueryHandler(
    IdentityDbContext dbContext) : IRequestHandler<GetMembersQuery, ErrorOr<List<MemberResponse>>>
{
    public async Task<ErrorOr<List<MemberResponse>>> HandleAsync(GetMembersQuery request, CancellationToken ct)
    {
        var members = await dbContext.Members
            .AsNoTracking()
            .OrderBy(m => m.Name)
            .ToListAsync(ct);

        return members.ConvertAll(m => m.MapResponse());
    }
}

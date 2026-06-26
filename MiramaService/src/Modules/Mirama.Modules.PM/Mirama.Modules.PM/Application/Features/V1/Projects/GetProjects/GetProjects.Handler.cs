using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Extensions;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjects;

public class GetProjectsController : OrganizationControllerBase
{
    [HttpGet("/projects")]
    public async Task<IActionResult> Get([FromQuery] GetProjectsQuery query, CancellationToken ct)
    {
        var result = await Dispatcher.Send(query, ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectsQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo)
    : IRequestHandler<GetProjectsQuery, ErrorOr<PaginatedList<ProjectResponse>>>
{
    public async Task<ErrorOr<PaginatedList<ProjectResponse>>> HandleAsync(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var query = queryRepo.Query()
            .Include(p => p.Members)
            .Include(p => p.Teams)
            .Include(p => p.Milestones)
            .Select(p => ProjectMapper.ToResponse(p));

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
            return await query.PaginatedListAsync(request.PageNumber.Value, request.PageSize.Value);

        var items = await query.ToListAsync(cancellationToken);
        return new PaginatedList<ProjectResponse>(items, items.Count, 1, Math.Max(items.Count, 1));
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.GetProjectMilestones;

public class GetProjectMilestonesController : OrganizationControllerBase
{
    [HttpGet("projects/{projectId:guid}/milestones")]
    public async Task<IActionResult> GetMilestones(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectMilestonesQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectMilestonesQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo)
    : IRequestHandler<GetProjectMilestonesQuery, ErrorOr<PaginatedList<ProjectMilestoneResponse>>>
{
    public async Task<ErrorOr<PaginatedList<ProjectMilestoneResponse>>> HandleAsync(GetProjectMilestonesQuery request, CancellationToken cancellationToken)
    {
        var project = await queryRepo.Query()
            .Include(p => p.Milestones)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var milestones = project.Milestones.Select(ProjectMilestoneMapper.ToResponse).ToList();

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            var paged = milestones
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToList();
            return new PaginatedList<ProjectMilestoneResponse>(paged, milestones.Count, request.PageNumber.Value, request.PageSize.Value);
        }

        return new PaginatedList<ProjectMilestoneResponse>(milestones, milestones.Count, 1, Math.Max(milestones.Count, 1));
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Teams;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.GetProjectTeams;

public class GetProjectTeamsController : OrganizationControllerBase
{
    [HttpGet("/projects/{projectId:guid}/teams")]
    public async Task<IActionResult> GetTeams(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectTeamsQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectTeamsQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo)
    : IRequestHandler<GetProjectTeamsQuery, ErrorOr<PaginatedList<ProjectTeamResponse>>>
{
    public async Task<ErrorOr<PaginatedList<ProjectTeamResponse>>> HandleAsync(GetProjectTeamsQuery request, CancellationToken cancellationToken)
    {
        var project = await queryRepo.Query()
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var teams = project.Teams.Select(ProjectTeamMapper.ToResponse).ToList();

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            var paged = teams
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToList();
            return new PaginatedList<ProjectTeamResponse>(paged, teams.Count, request.PageNumber.Value, request.PageSize.Value);
        }

        return new PaginatedList<ProjectTeamResponse>(teams, teams.Count, 1, Math.Max(teams.Count, 1));
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Teams;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.AddProjectTeam;

public class AddProjectTeamController : OrganizationControllerBase
{
    [HttpPost("projects/{projectId:guid}/teams")]
    public async Task<IActionResult> AddTeam(
        [FromRoute] Guid projectId,
        [FromBody] AddProjectTeamCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/teams/{r.TeamId}", r), Problem);
    }
}

internal class AddProjectTeamCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo,
    ITeamService teamService)
    : IRequestHandler<AddProjectTeamCommand, ErrorOr<ProjectTeamResponse>>
{
    public async Task<ErrorOr<ProjectTeamResponse>> HandleAsync(AddProjectTeamCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var teamDto = await teamService.GetTeamByIdAsync(request.TeamId, cancellationToken);
        if (teamDto is null)
            return Error.NotFound("Team.NotFound", "Team not found.");

        var addResult = project.AddTeam(request.TeamId);
        if (addResult.IsError) return addResult.Errors;

        commandRepo.Update(project);

        var projectTeam = project.Teams.Find(t => t.TeamId == request.TeamId)!;
        return ProjectTeamMapper.ToResponse(projectTeam, teamDto);
    }
}

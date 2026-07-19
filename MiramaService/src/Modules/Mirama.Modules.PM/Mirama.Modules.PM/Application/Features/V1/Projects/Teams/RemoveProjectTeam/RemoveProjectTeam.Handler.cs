using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Teams.RemoveProjectTeam;

public class RemoveProjectTeamController : OrganizationControllerBase
{
    [HttpDelete("projects/{projectId:guid}/teams/{teamId:guid}")]
    public async Task<IActionResult> RemoveTeam(
        [FromRoute] Guid projectId,
        [FromRoute] Guid teamId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveProjectTeamCommand(projectId, teamId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveProjectTeamCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<RemoveProjectTeamCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveProjectTeamCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Teams)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var removeResult = project.RemoveTeam(request.TeamId);
        if (removeResult.IsError) return removeResult.Errors;

        commandRepo.Update(project);

        return Result.Deleted;
    }
}

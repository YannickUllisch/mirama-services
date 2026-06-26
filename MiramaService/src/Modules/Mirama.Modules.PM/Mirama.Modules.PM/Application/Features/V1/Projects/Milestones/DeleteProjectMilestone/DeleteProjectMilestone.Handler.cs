using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.DeleteProjectMilestone;

public class DeleteProjectMilestoneController : OrganizationControllerBase
{
    [HttpDelete("/projects/{projectId:guid}/milestones/{milestoneId:guid}")]
    public async Task<IActionResult> DeleteMilestone(
        [FromRoute] Guid projectId,
        [FromRoute] Guid milestoneId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new DeleteProjectMilestoneCommand(projectId, milestoneId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class DeleteProjectMilestoneCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<DeleteProjectMilestoneCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeleteProjectMilestoneCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Milestones)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var deleteResult = project.RemoveMilestone(new ProjectMilestoneId(request.MilestoneId));
        if (deleteResult.IsError) return deleteResult.Errors;

        commandRepo.Update(project);

        return Result.Deleted;
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.UpdateProjectMilestone;

public class UpdateProjectMilestoneController : OrganizationControllerBase
{
    [HttpPut("projects/{projectId:guid}/milestones/{milestoneId:guid}")]
    public async Task<IActionResult> UpdateMilestone(
        [FromRoute] Guid projectId,
        [FromRoute] Guid milestoneId,
        [FromBody] UpdateProjectMilestoneCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId, MilestoneId = milestoneId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateProjectMilestoneCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<UpdateProjectMilestoneCommand, ErrorOr<ProjectMilestoneResponse>>
{
    public async Task<ErrorOr<ProjectMilestoneResponse>> HandleAsync(UpdateProjectMilestoneCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Milestones)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var milestone = project.Milestones.Find(m => m.Id == new ProjectMilestoneId(request.MilestoneId));
        if (milestone is null)
            return Error.NotFound("Project.Milestone.NotFound", "Milestone not found.");

        milestone.Update(new ProjectMilestoneDetails(request.Title, request.DueDate, request.Description, request.Color));

        commandRepo.Update(project);

        return ProjectMilestoneMapper.ToResponse(milestone);
    }
}

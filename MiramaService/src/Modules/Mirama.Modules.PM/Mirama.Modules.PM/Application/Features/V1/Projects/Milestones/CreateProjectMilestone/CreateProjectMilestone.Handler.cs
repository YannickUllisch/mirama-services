using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Milestones.CreateProjectMilestone;

public class CreateProjectMilestoneController : OrganizationControllerBase
{
    [HttpPost("projects/{projectId:guid}/milestones")]
    public async Task<IActionResult> CreateMilestone(
        [FromRoute] Guid projectId,
        [FromBody] CreateProjectMilestoneCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/milestones/{r.Id}", r), Problem);
    }
}

internal class CreateProjectMilestoneCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<CreateProjectMilestoneCommand, ErrorOr<ProjectMilestoneResponse>>
{
    public async Task<ErrorOr<ProjectMilestoneResponse>> HandleAsync(CreateProjectMilestoneCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var milestone = project.AddMilestone(new ProjectMilestoneDetails(
            request.Title,
            request.DueDate,
            request.Description,
            request.Color));

        commandRepo.Update(project);

        return ProjectMilestoneMapper.ToResponse(milestone);
    }
}

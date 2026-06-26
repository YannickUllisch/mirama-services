using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.UpdateProject;

public class UpdateProjectController : OrganizationControllerBase
{
    [HttpPut("/projects/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateProjectCommand command,
        CancellationToken ct)
    {
        var cmd = command with { Id = id };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateProjectCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<UpdateProjectCommand, ErrorOr<ProjectResponse>>
{
    public async Task<ErrorOr<ProjectResponse>> HandleAsync(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Members)
            .Include(p => p.Teams)
            .Include(p => p.Milestones)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.Id), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        project.Update(new ProjectDetails(
            request.Name,
            request.StartDate,
            request.StatusId,
            request.PriorityId,
            request.Description,
            request.EndDate,
            request.Budget));

        var reconcileTagsResult = ReconcileTags(project, request.TagIds);
        if (reconcileTagsResult.IsError) return reconcileTagsResult.Errors;

        commandRepo.Update(project);

        return ProjectMapper.ToResponse(project);
    }

    private static ErrorOr<Success> ReconcileTags(Project project, List<Guid> desiredTagIds)
    {
        var toRemove = project.TagIds.Except(desiredTagIds).ToList();
        foreach (var tagId in toRemove)
            project.RemoveTag(tagId);

        foreach (var tagId in desiredTagIds.Except(project.TagIds).ToList())
        {
            var result = project.AddTag(tagId);
            if (result.IsError) return result.Errors;
        }

        return Result.Success;
    }
}

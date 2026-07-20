using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.RemoveTaskPriority;

public class RemoveTaskPriorityController : OrganizationControllerBase
{
    [HttpDelete("projects/{projectId:guid}/workflow/task-priorities/{priorityId:guid}")]
    public async Task<IActionResult> RemoveTaskPriority(
        [FromRoute] Guid projectId,
        [FromRoute] Guid priorityId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveTaskPriorityCommand(projectId, priorityId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveTaskPriorityCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<RemoveTaskPriorityCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveTaskPriorityCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskPriorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.RemoveTaskPriority(new PriorityConfigId(request.PriorityId));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return Result.Deleted;
    }
}

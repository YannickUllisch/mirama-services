using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.RemoveTaskStatus;

public class RemoveTaskStatusController : OrganizationControllerBase
{
    [HttpDelete("projects/{projectId:guid}/workflow/task-statuses/{statusId:guid}")]
    public async Task<IActionResult> RemoveTaskStatus(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveTaskStatusCommand(projectId, statusId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveTaskStatusCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<RemoveTaskStatusCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskStatuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.RemoveTaskStatus(new StatusConfigId(request.StatusId));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return Result.Deleted;
    }
}

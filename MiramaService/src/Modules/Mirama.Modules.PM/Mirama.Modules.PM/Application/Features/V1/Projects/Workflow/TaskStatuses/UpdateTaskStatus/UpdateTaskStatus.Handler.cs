using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.UpdateTaskStatus;

public class UpdateTaskStatusController : OrganizationControllerBase
{
    [HttpPut("projects/{projectId:guid}/workflow/task-statuses/{statusId:guid}")]
    public async Task<IActionResult> UpdateTaskStatus(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId,
        [FromBody] UpdateTaskStatusCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId, StatusId = statusId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateTaskStatusCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<UpdateTaskStatusCommand, ErrorOr<StatusResponse>>
{
    public async Task<ErrorOr<StatusResponse>> HandleAsync(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskStatuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var statusId = new StatusConfigId(request.StatusId);
        var result = workflowConfig.UpdateTaskStatus(statusId, new StatusDetails(request.Name, request.Category, request.Color, IsTerminal: request.IsTerminal));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        var updated = workflowConfig.TaskStatuses.First(s => s.Id == statusId);
        return StatusMapper.ToResponse(updated);
    }
}

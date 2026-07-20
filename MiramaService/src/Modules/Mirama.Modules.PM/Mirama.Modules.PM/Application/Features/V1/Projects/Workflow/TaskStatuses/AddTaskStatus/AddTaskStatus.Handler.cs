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

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.AddTaskStatus;

public class AddTaskStatusController : OrganizationControllerBase
{
    [HttpPost("projects/{projectId:guid}/workflow/task-statuses")]
    public async Task<IActionResult> AddTaskStatus(
        [FromRoute] Guid projectId,
        [FromBody] AddTaskStatusCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/workflow/task-statuses/{r.Id}", r), Problem);
    }
}

internal class AddTaskStatusCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<AddTaskStatusCommand, ErrorOr<StatusResponse>>
{
    public async Task<ErrorOr<StatusResponse>> HandleAsync(AddTaskStatusCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskStatuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.AddTaskStatus(new StatusDetails(request.Name, request.Category, request.Color, request.IsDefault, request.IsTerminal));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return StatusMapper.ToResponse(result.Value);
    }
}

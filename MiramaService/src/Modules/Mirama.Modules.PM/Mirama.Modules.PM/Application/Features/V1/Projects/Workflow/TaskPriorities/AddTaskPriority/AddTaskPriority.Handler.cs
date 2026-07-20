using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.AddTaskPriority;

public class AddTaskPriorityController : OrganizationControllerBase
{
    [HttpPost("projects/{projectId:guid}/workflow/task-priorities")]
    public async Task<IActionResult> AddTaskPriority(
        [FromRoute] Guid projectId,
        [FromBody] AddTaskPriorityCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/workflow/task-priorities/{r.Id}", r), Problem);
    }
}

internal class AddTaskPriorityCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<AddTaskPriorityCommand, ErrorOr<PriorityResponse>>
{
    public async Task<ErrorOr<PriorityResponse>> HandleAsync(AddTaskPriorityCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskPriorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.AddTaskPriority(new PriorityDetails(request.Name, request.Level, request.Color, request.Icon, request.IsDefault));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return PriorityMapper.ToResponse(result.Value);
    }
}

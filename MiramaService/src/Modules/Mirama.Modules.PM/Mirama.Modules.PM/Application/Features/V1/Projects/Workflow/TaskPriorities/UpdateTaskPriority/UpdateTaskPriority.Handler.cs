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

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.UpdateTaskPriority;

public class UpdateTaskPriorityController : OrganizationControllerBase
{
    [HttpPut("projects/{projectId:guid}/workflow/task-priorities/{priorityId:guid}")]
    public async Task<IActionResult> UpdateTaskPriority(
        [FromRoute] Guid projectId,
        [FromRoute] Guid priorityId,
        [FromBody] UpdateTaskPriorityCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId, PriorityId = priorityId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateTaskPriorityCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<UpdateTaskPriorityCommand, ErrorOr<PriorityResponse>>
{
    public async Task<ErrorOr<PriorityResponse>> HandleAsync(UpdateTaskPriorityCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskPriorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var priorityId = new PriorityConfigId(request.PriorityId);
        var result = workflowConfig.UpdateTaskPriority(priorityId, new PriorityDetails(request.Name, request.Level, request.Color, request.Icon));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        var updated = workflowConfig.TaskPriorities.First(p => p.Id == priorityId);
        return PriorityMapper.ToResponse(updated);
    }
}

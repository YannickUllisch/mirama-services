using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities.UpdateProjectPriority;

public class UpdateProjectPriorityController : OrganizationControllerBase
{
    [HttpPut("projects/{projectId:guid}/workflow/priorities/{priorityId:guid}")]
    public async Task<IActionResult> UpdatePriority(
        [FromRoute] Guid projectId,
        [FromRoute] Guid priorityId,
        [FromBody] UpdateProjectPriorityCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId, PriorityId = priorityId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateProjectPriorityCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<UpdateProjectPriorityCommand, ErrorOr<PriorityResponse>>
{
    public async Task<ErrorOr<PriorityResponse>> HandleAsync(UpdateProjectPriorityCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Priorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var priorityId = new PriorityConfigId(request.PriorityId);
        var result = workflowConfig.UpdatePriority(priorityId, new PriorityDetails(request.Name, request.Level, request.Color, request.Icon));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        var updated = workflowConfig.Priorities.First(p => p.Id == priorityId);
        return PriorityMapper.ToResponse(updated);
    }
}

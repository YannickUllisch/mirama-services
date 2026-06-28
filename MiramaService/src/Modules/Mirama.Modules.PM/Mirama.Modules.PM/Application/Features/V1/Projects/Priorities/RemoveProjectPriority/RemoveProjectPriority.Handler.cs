using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Priorities.RemoveProjectPriority;

public class RemoveProjectPriorityController : OrganizationControllerBase
{
    [HttpDelete("/projects/{projectId:guid}/priorities/{priorityId:guid}")]
    public async Task<IActionResult> RemovePriority(
        [FromRoute] Guid projectId,
        [FromRoute] Guid priorityId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveProjectPriorityCommand(projectId, priorityId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveProjectPriorityCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<RemoveProjectPriorityCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveProjectPriorityCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Priorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == request.ProjectId, cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.RemovePriority(new PriorityConfigId(request.PriorityId));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return Result.Deleted;
    }
}

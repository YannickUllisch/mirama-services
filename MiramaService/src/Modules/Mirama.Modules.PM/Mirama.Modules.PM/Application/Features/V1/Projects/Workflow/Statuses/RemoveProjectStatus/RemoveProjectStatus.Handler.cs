using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses.RemoveProjectStatus;

public class RemoveProjectStatusController : OrganizationControllerBase
{
    [HttpDelete("projects/{projectId:guid}/workflow/statuses/{statusId:guid}")]
    public async Task<IActionResult> RemoveStatus(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveProjectStatusCommand(projectId, statusId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveProjectStatusCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<RemoveProjectStatusCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.RemoveStatus(new StatusConfigId(request.StatusId));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return Result.Deleted;
    }
}

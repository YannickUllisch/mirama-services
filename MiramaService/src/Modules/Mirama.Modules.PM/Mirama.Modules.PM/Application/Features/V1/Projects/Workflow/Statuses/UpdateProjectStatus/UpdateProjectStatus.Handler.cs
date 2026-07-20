using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses.UpdateProjectStatus;

public class UpdateProjectStatusController : OrganizationControllerBase
{
    [HttpPut("projects/{projectId:guid}/workflow/statuses/{statusId:guid}")]
    public async Task<IActionResult> UpdateStatus(
        [FromRoute] Guid projectId,
        [FromRoute] Guid statusId,
        [FromBody] UpdateProjectStatusCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId, StatusId = statusId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateProjectStatusCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<UpdateProjectStatusCommand, ErrorOr<StatusResponse>>
{
    public async Task<ErrorOr<StatusResponse>> HandleAsync(UpdateProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var statusId = new StatusConfigId(request.StatusId);
        var result = workflowConfig.UpdateStatus(statusId, new StatusDetails(request.Name, request.Category, request.Color, IsTerminal: request.IsTerminal));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        var updated = workflowConfig.Statuses.First(s => s.Id == statusId);
        return StatusMapper.ToResponse(updated);
    }
}

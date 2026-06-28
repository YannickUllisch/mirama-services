using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Status;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Statuses.AddProjectStatus;

public class AddProjectStatusController : OrganizationControllerBase
{
    [HttpPost("/projects/{projectId:guid}/statuses")]
    public async Task<IActionResult> AddStatus(
        [FromRoute] Guid projectId,
        [FromBody] AddProjectStatusCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/statuses/{r.StatusId}", r), Problem);
    }
}

internal class AddProjectStatusCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<AddProjectStatusCommand, ErrorOr<StatusResponse>>
{
    public async Task<ErrorOr<StatusResponse>> HandleAsync(AddProjectStatusCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == request.ProjectId, cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.AddStatus(new StatusDetails(request.Name, request.Category, request.Color, request.IsDefault, request.IsTerminal));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return StatusMapper.ToResponse(result.Value);
    }
}

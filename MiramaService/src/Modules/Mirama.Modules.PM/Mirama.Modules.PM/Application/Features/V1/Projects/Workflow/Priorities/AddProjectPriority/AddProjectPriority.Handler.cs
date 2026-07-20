using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig.Priority;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities.AddProjectPriority;

public class AddProjectPriorityController : OrganizationControllerBase
{
    [HttpPost("projects/{projectId:guid}/workflow/priorities")]
    public async Task<IActionResult> AddPriority(
        [FromRoute] Guid projectId,
        [FromBody] AddProjectPriorityCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/workflow/priorities/{r.Id}", r), Problem);
    }
}

internal class AddProjectPriorityCommandHandler(
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<AddProjectPriorityCommand, ErrorOr<PriorityResponse>>
{
    public async Task<ErrorOr<PriorityResponse>> HandleAsync(AddProjectPriorityCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Priorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var result = workflowConfig.AddPriority(new PriorityDetails(request.Name, request.Level, request.Color, request.Icon, request.IsDefault));
        if (result.IsError) return result.Errors;

        workflowRepo.Update(workflowConfig);

        return PriorityMapper.ToResponse(result.Value);
    }
}

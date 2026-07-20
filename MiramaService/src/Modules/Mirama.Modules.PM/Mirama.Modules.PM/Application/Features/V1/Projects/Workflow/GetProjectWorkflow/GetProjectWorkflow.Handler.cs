using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.GetProjectWorkflow;

public class GetProjectWorkflowController : OrganizationControllerBase
{
    [HttpGet("projects/{projectId:guid}/workflow")]
    public async Task<IActionResult> GetWorkflow([FromRoute] Guid projectId, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectWorkflowQuery(projectId), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectWorkflowQueryHandler(
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<GetProjectWorkflowQuery, ErrorOr<WorkflowResponse>>
{
    public async Task<ErrorOr<WorkflowResponse>> HandleAsync(GetProjectWorkflowQuery request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .Include(wc => wc.Priorities)
            .Include(wc => wc.TaskStatuses)
            .Include(wc => wc.TaskPriorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        return WorkflowMapper.ToResponse(workflowConfig);
    }
}

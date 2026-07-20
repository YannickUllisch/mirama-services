using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.GetTaskStatuses;

public class GetTaskStatusesController : OrganizationControllerBase
{
    [HttpGet("projects/{projectId:guid}/workflow/task-statuses")]
    public async Task<IActionResult> GetTaskStatuses(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetTaskStatusesQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetTaskStatusesQueryHandler(
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<GetTaskStatusesQuery, ErrorOr<PaginatedList<StatusResponse>>>
{
    public async Task<ErrorOr<PaginatedList<StatusResponse>>> HandleAsync(GetTaskStatusesQuery request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskStatuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var statuses = workflowConfig.TaskStatuses
            .OrderBy(s => s.Position)
            .Select(StatusMapper.ToResponse)
            .ToList();

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            var paged = statuses
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToList();
            return new PaginatedList<StatusResponse>(paged, statuses.Count, request.PageNumber.Value, request.PageSize.Value);
        }

        return new PaginatedList<StatusResponse>(statuses, statuses.Count, 1, Math.Max(statuses.Count, 1));
    }
}

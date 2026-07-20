using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.GetTaskPriorities;

public class GetTaskPrioritiesController : OrganizationControllerBase
{
    [HttpGet("projects/{projectId:guid}/workflow/task-priorities")]
    public async Task<IActionResult> GetTaskPriorities(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetTaskPrioritiesQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetTaskPrioritiesQueryHandler(
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<GetTaskPrioritiesQuery, ErrorOr<PaginatedList<PriorityResponse>>>
{
    public async Task<ErrorOr<PaginatedList<PriorityResponse>>> HandleAsync(GetTaskPrioritiesQuery request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.TaskPriorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == new ProjectId(request.ProjectId), cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var priorities = workflowConfig.TaskPriorities
            .OrderBy(p => p.Level)
            .Select(PriorityMapper.ToResponse)
            .ToList();

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            var paged = priorities
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToList();
            return new PaginatedList<PriorityResponse>(paged, priorities.Count, request.PageNumber.Value, request.PageSize.Value);
        }

        return new PaginatedList<PriorityResponse>(priorities, priorities.Count, 1, Math.Max(priorities.Count, 1));
    }
}

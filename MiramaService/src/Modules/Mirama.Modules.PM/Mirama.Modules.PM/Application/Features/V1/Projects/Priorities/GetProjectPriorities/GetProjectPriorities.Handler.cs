using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Priorities.GetProjectPriorities;

public class GetProjectPrioritiesController : OrganizationControllerBase
{
    [HttpGet("/projects/{projectId:guid}/priorities")]
    public async Task<IActionResult> GetPriorities(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectPrioritiesQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectPrioritiesQueryHandler(
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<GetProjectPrioritiesQuery, ErrorOr<PaginatedList<PriorityResponse>>>
{
    public async Task<ErrorOr<PaginatedList<PriorityResponse>>> HandleAsync(GetProjectPrioritiesQuery request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Priorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == request.ProjectId, cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var priorities = workflowConfig.Priorities
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

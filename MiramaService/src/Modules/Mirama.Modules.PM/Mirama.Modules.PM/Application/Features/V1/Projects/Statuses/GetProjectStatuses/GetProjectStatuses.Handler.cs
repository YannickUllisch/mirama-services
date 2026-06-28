using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Statuses.GetProjectStatuses;

public class GetProjectStatusesController : OrganizationControllerBase
{
    [HttpGet("/projects/{projectId:guid}/statuses")]
    public async Task<IActionResult> GetStatuses(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectStatusesQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectStatusesQueryHandler(
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo)
    : IRequestHandler<GetProjectStatusesQuery, ErrorOr<PaginatedList<StatusResponse>>>
{
    public async Task<ErrorOr<PaginatedList<StatusResponse>>> HandleAsync(GetProjectStatusesQuery request, CancellationToken cancellationToken)
    {
        var workflowConfig = await workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .FirstOrDefaultAsync(wc => wc.ProjectId == request.ProjectId, cancellationToken);

        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Project workflow configuration not found.");

        var statuses = workflowConfig.Statuses
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

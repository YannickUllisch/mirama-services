using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Extensions;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.GetProjectTemplates;

public class GetProjectTemplatesController : OrganizationControllerBase
{
    [HttpGet("/project-templates")]
    public async Task<IActionResult> Get([FromQuery] GetProjectTemplatesQuery query, CancellationToken ct)
    {
        var result = await Dispatcher.Send(query, ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectTemplatesQueryHandler(
    IPMQueryRepository<ProjectTemplate, ProjectTemplateId> queryRepo)
    : IRequestHandler<GetProjectTemplatesQuery, ErrorOr<PaginatedList<ProjectTemplateResponse>>>
{
    public async Task<ErrorOr<PaginatedList<ProjectTemplateResponse>>> HandleAsync(GetProjectTemplatesQuery request, CancellationToken cancellationToken)
    {
        var query = queryRepo.Query()
            .Include(t => t.TaskTemplates)
            .Include(t => t.MilestoneTemplates)
            .Include(t => t.CycleTemplates)
            .Select(t => ProjectTemplateMapper.ToResponse(t));

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
            return await query.PaginatedListAsync(request.PageNumber.Value, request.PageSize.Value);

        var items = await query.ToListAsync(cancellationToken);
        return new PaginatedList<ProjectTemplateResponse>(items, items.Count, 1, Math.Max(items.Count, 1));
    }
}

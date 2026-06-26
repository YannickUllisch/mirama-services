using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.GetProjectTemplateById;

public class GetProjectTemplateByIdController : OrganizationControllerBase
{
    [HttpGet("/project-templates/{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectTemplateByIdQuery(id), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectTemplateByIdQueryHandler(
    IPMQueryRepository<ProjectTemplate, ProjectTemplateId> queryRepo)
    : IRequestHandler<GetProjectTemplateByIdQuery, ErrorOr<ProjectTemplateResponse>>
{
    public async Task<ErrorOr<ProjectTemplateResponse>> HandleAsync(GetProjectTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var template = await queryRepo.Query()
            .Include(t => t.TaskTemplates)
            .Include(t => t.MilestoneTemplates)
            .Include(t => t.CycleTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.Id), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        return ProjectTemplateMapper.ToResponse(template);
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.AddCycleTemplate;

public class AddCycleTemplateController : OrganizationControllerBase
{
    [HttpPost("project-templates/{projectTemplateId:guid}/cycles")]
    public async Task<IActionResult> Add(
        [FromRoute] Guid projectTemplateId,
        [FromBody] AddCycleTemplateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectTemplateId = projectTemplateId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class AddCycleTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<AddCycleTemplateCommand, ErrorOr<CycleTemplateResponse>>
{
    public async Task<ErrorOr<CycleTemplateResponse>> HandleAsync(AddCycleTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.CycleTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var cycle = template.AddCycleTemplate(new CycleTemplateDetails(
            request.Name,
            request.Goal,
            request.DurationDays));

        commandRepo.Update(template);

        return CycleTemplateMapper.ToResponse(cycle);
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.UpdateCycleTemplate;

public class UpdateCycleTemplateController : OrganizationControllerBase
{
    [HttpPut("project-templates/{projectTemplateId:guid}/cycles/{cycleTemplateId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectTemplateId,
        [FromRoute] Guid cycleTemplateId,
        [FromBody] UpdateCycleTemplateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectTemplateId = projectTemplateId, CycleTemplateId = cycleTemplateId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateCycleTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<UpdateCycleTemplateCommand, ErrorOr<CycleTemplateResponse>>
{
    public async Task<ErrorOr<CycleTemplateResponse>> HandleAsync(UpdateCycleTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.CycleTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var cycle = template.CycleTemplates.Find(c => c.Id == new CycleTemplateId(request.CycleTemplateId));
        if (cycle is null)
            return Error.NotFound("ProjectTemplate.Cycle.NotFound", "Cycle template not found.");

        cycle.Update(new CycleTemplateDetails(request.Name, request.Goal, request.DurationDays));
        commandRepo.Update(template);

        return CycleTemplateMapper.ToResponse(cycle);
    }
}

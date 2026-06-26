using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.CycleTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CycleTemplates.RemoveCycleTemplate;

public class RemoveCycleTemplateController : OrganizationControllerBase
{
    [HttpDelete("/project-templates/{projectTemplateId:guid}/cycles/{cycleTemplateId:guid}")]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid projectTemplateId,
        [FromRoute] Guid cycleTemplateId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveCycleTemplateCommand(projectTemplateId, cycleTemplateId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveCycleTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<RemoveCycleTemplateCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveCycleTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.CycleTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var result = template.RemoveCycleTemplate(new CycleTemplateId(request.CycleTemplateId));
        if (result.IsError) return result.Errors;

        commandRepo.Update(template);
        return Result.Deleted;
    }
}

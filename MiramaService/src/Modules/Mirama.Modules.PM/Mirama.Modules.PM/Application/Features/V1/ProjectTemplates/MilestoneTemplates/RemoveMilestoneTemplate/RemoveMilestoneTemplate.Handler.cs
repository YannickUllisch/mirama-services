using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.RemoveMilestoneTemplate;

public class RemoveMilestoneTemplateController : OrganizationControllerBase
{
    [HttpDelete("/project-templates/{projectTemplateId:guid}/milestones/{milestoneTemplateId:guid}")]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid projectTemplateId,
        [FromRoute] Guid milestoneTemplateId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveMilestoneTemplateCommand(projectTemplateId, milestoneTemplateId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveMilestoneTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<RemoveMilestoneTemplateCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveMilestoneTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.MilestoneTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var result = template.RemoveMilestoneTemplate(new MilestoneTemplateId(request.MilestoneTemplateId));
        if (result.IsError) return result.Errors;

        commandRepo.Update(template);
        return Result.Deleted;
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates.RemoveTaskTemplate;

public class RemoveTaskTemplateController : OrganizationControllerBase
{
    [HttpDelete("project-templates/{projectTemplateId:guid}/tasks/{taskTemplateId:guid}")]
    public async Task<IActionResult> Remove(
        [FromRoute] Guid projectTemplateId,
        [FromRoute] Guid taskTemplateId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveTaskTemplateCommand(projectTemplateId, taskTemplateId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveTaskTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<RemoveTaskTemplateCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveTaskTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.TaskTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var result = template.RemoveTaskTemplate(new TaskTemplateId(request.TaskTemplateId));
        if (result.IsError) return result.Errors;

        commandRepo.Update(template);
        return Result.Deleted;
    }
}

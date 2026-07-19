using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.DeleteProjectTemplate;

public class DeleteProjectTemplateController : OrganizationControllerBase
{
    [HttpDelete("project-templates/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new DeleteProjectTemplateCommand(id), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class DeleteProjectTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<DeleteProjectTemplateCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(DeleteProjectTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.Id), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        commandRepo.Remove(template);
        return Result.Deleted;
    }
}

using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.UpdateProjectTemplate;

public class UpdateProjectTemplateController : OrganizationControllerBase
{
    [HttpPut("/project-templates/{id:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid id,
        [FromBody] UpdateProjectTemplateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { Id = id };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateProjectTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<UpdateProjectTemplateCommand, ErrorOr<ProjectTemplateResponse>>
{
    public async Task<ErrorOr<ProjectTemplateResponse>> HandleAsync(UpdateProjectTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.TaskTemplates)
            .Include(t => t.MilestoneTemplates)
            .Include(t => t.CycleTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.Id), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        template.Update(new ProjectTemplateDetails(request.Name, request.Description, request.Category));
        commandRepo.Update(template);

        return ProjectTemplateMapper.ToResponse(template);
    }
}

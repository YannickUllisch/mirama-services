using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.AddMilestoneTemplate;

public class AddMilestoneTemplateController : OrganizationControllerBase
{
    [HttpPost("/project-templates/{projectTemplateId:guid}/milestones")]
    public async Task<IActionResult> Add(
        [FromRoute] Guid projectTemplateId,
        [FromBody] AddMilestoneTemplateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectTemplateId = projectTemplateId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class AddMilestoneTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<AddMilestoneTemplateCommand, ErrorOr<MilestoneTemplateResponse>>
{
    public async Task<ErrorOr<MilestoneTemplateResponse>> HandleAsync(AddMilestoneTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.MilestoneTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var milestone = template.AddMilestoneTemplate(new MilestoneTemplateDetails(
            request.Title,
            request.DayOffset,
            request.Description,
            request.Color));

        commandRepo.Update(template);

        return MilestoneTemplateMapper.ToResponse(milestone);
    }
}

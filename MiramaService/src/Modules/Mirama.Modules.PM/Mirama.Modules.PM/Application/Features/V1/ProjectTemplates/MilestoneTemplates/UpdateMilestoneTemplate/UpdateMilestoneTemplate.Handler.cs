using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.MilestoneTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.MilestoneTemplates.UpdateMilestoneTemplate;

public class UpdateMilestoneTemplateController : OrganizationControllerBase
{
    [HttpPut("project-templates/{projectTemplateId:guid}/milestones/{milestoneTemplateId:guid}")]
    public async Task<IActionResult> Update(
        [FromRoute] Guid projectTemplateId,
        [FromRoute] Guid milestoneTemplateId,
        [FromBody] UpdateMilestoneTemplateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectTemplateId = projectTemplateId, MilestoneTemplateId = milestoneTemplateId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateMilestoneTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<UpdateMilestoneTemplateCommand, ErrorOr<MilestoneTemplateResponse>>
{
    public async Task<ErrorOr<MilestoneTemplateResponse>> HandleAsync(UpdateMilestoneTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.MilestoneTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var milestone = template.MilestoneTemplates.Find(m => m.Id == new MilestoneTemplateId(request.MilestoneTemplateId));
        if (milestone is null)
            return Error.NotFound("ProjectTemplate.Milestone.NotFound", "Milestone template not found.");

        milestone.Update(new MilestoneTemplateDetails(request.Title, request.DayOffset, request.Description, request.Color));
        commandRepo.Update(template);

        return MilestoneTemplateMapper.ToResponse(milestone);
    }
}

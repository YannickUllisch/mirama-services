using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate.TaskTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.TaskTemplates.AddTaskTemplate;

public class AddTaskTemplateController : OrganizationControllerBase
{
    [HttpPost("project-templates/{projectTemplateId:guid}/tasks")]
    public async Task<IActionResult> Add(
        [FromRoute] Guid projectTemplateId,
        [FromBody] AddTaskTemplateCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectTemplateId = projectTemplateId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class AddTaskTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> commandRepo)
    : IRequestHandler<AddTaskTemplateCommand, ErrorOr<TaskTemplateResponse>>
{
    public async Task<ErrorOr<TaskTemplateResponse>> HandleAsync(AddTaskTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await commandRepo.Query()
            .Include(t => t.TaskTemplates)
            .FirstOrDefaultAsync(t => t.Id == new ProjectTemplateId(request.ProjectTemplateId), cancellationToken);

        if (template is null)
            return Error.NotFound("ProjectTemplate.NotFound", "Project template not found.");

        var parentId = request.ParentTemplateTaskId.HasValue
            ? new TaskTemplateId(request.ParentTemplateTaskId.Value)
            : null;

        var task = template.AddTaskTemplate(new TaskTemplateDetails(
            request.Title,
            request.Type,
            request.Description,
            request.EstimatedHours,
            parentId));

        commandRepo.Update(template);

        return TaskTemplateMapper.ToResponse(task);
    }
}

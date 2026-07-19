using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.ProjectTemplate;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CreateProjectTemplate;

public class CreateProjectTemplateController : OrganizationControllerBase
{
    [HttpPost("project-templates")]
    public async Task<IActionResult> Create([FromBody] CreateProjectTemplateCommand command, CancellationToken ct)
    {
        var result = await Dispatcher.Send(command, ct);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.TemplateId }, r), Problem);
    }
}

internal class CreateProjectTemplateCommandHandler(
    IPMCommandRepository<ProjectTemplate, ProjectTemplateId> repo)
    : IRequestHandler<CreateProjectTemplateCommand, ErrorOr<ProjectTemplateResponse>>
{
    public Task<ErrorOr<ProjectTemplateResponse>> HandleAsync(CreateProjectTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = ProjectTemplate.Create(new ProjectTemplateDetails(
            request.Name,
            request.Description,
            request.Category));

        repo.Add(template);

        return Task.FromResult<ErrorOr<ProjectTemplateResponse>>(ProjectTemplateMapper.ToResponse(template));
    }
}

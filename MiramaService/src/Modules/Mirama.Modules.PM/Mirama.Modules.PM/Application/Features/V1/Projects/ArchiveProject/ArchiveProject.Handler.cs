using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.ArchiveProject;

public class ArchiveProjectController : OrganizationControllerBase
{
    [HttpPost("/projects/{id:guid}/archive")]
    public async Task<IActionResult> Archive([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new ArchiveProjectCommand(id), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class ArchiveProjectCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<ArchiveProjectCommand, ErrorOr<Success>>
{
    public async Task<ErrorOr<Success>> HandleAsync(ArchiveProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.Id), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        project.Archive();
        commandRepo.Update(project);

        return Result.Success;
    }
}

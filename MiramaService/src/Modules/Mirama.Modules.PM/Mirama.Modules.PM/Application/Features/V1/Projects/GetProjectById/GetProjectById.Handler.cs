using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjectById;

public class GetProjectByIdController : OrganizationControllerBase
{
    [HttpGet("/projects/{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectByIdQuery(id), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectByIdQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo)
    : IRequestHandler<GetProjectByIdQuery, ErrorOr<ProjectResponse>>
{
    public async Task<ErrorOr<ProjectResponse>> HandleAsync(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await queryRepo.Query()
            .Include(p => p.Members)
            .Include(p => p.Teams)
            .Include(p => p.Milestones)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.Id), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        return ProjectMapper.ToResponse(project);
    }
}

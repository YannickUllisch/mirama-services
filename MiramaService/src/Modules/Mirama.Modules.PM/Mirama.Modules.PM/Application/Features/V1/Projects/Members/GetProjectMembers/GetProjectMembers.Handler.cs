using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Application.Features.V1.Projects.Members;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.GetProjectMembers;

public class GetProjectMembersController : OrganizationControllerBase
{
    [HttpGet("/projects/{projectId:guid}/members")]
    public async Task<IActionResult> GetMembers(
        [FromRoute] Guid projectId,
        [FromQuery] int? pageNumber,
        [FromQuery] int? pageSize,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectMembersQuery(projectId, pageNumber, pageSize), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectMembersQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo)
    : IRequestHandler<GetProjectMembersQuery, ErrorOr<PaginatedList<ProjectMemberResponse>>>
{
    public async Task<ErrorOr<PaginatedList<ProjectMemberResponse>>> HandleAsync(GetProjectMembersQuery request, CancellationToken cancellationToken)
    {
        var project = await queryRepo.Query()
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var members = project.Members.Select(ProjectMemberMapper.ToResponse).ToList();

        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            var paged = members
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToList();
            return new PaginatedList<ProjectMemberResponse>(paged, members.Count, request.PageNumber.Value, request.PageSize.Value);
        }

        return new PaginatedList<ProjectMemberResponse>(members, members.Count, 1, Math.Max(members.Count, 1));
    }
}

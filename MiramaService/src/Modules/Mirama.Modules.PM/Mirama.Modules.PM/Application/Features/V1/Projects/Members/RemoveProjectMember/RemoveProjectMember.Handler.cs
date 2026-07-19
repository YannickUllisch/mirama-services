using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.RemoveProjectMember;

public class RemoveProjectMemberController : OrganizationControllerBase
{
    [HttpDelete("projects/{projectId:guid}/members/{memberId:guid}")]
    public async Task<IActionResult> RemoveMember(
        [FromRoute] Guid projectId,
        [FromRoute] Guid memberId,
        CancellationToken ct)
    {
        var result = await Dispatcher.Send(new RemoveProjectMemberCommand(projectId, memberId), ct);
        return result.Match(_ => NoContent(), Problem);
    }
}

internal class RemoveProjectMemberCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo)
    : IRequestHandler<RemoveProjectMemberCommand, ErrorOr<Deleted>>
{
    public async Task<ErrorOr<Deleted>> HandleAsync(RemoveProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var removeResult = project.RemoveMember(request.MemberId);
        if (removeResult.IsError) return removeResult.Errors;

        commandRepo.Update(project);

        return Result.Deleted;
    }
}

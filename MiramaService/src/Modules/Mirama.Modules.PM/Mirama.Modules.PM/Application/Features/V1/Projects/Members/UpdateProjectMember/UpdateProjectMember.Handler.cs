using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.UpdateProjectMember;

public class UpdateProjectMemberController : OrganizationControllerBase
{
    [HttpPut("projects/{projectId:guid}/members/{memberId:guid}")]
    public async Task<IActionResult> UpdateMember(
        [FromRoute] Guid projectId,
        [FromRoute] Guid memberId,
        [FromBody] UpdateProjectMemberCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId, MemberId = memberId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class UpdateProjectMemberCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo,
    IMemberService memberService)
    : IRequestHandler<UpdateProjectMemberCommand, ErrorOr<ProjectMemberResponse>>
{
    public async Task<ErrorOr<ProjectMemberResponse>> HandleAsync(UpdateProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var updateResult = project.UpdateMemberRole(request.MemberId, request.RoleId);
        if (updateResult.IsError) return updateResult.Errors;

        commandRepo.Update(project);

        var member = project.Members.Find(m => m.MemberId == request.MemberId)!;
        var memberDtos = await memberService.GetMembersByIdsAsync([request.MemberId], cancellationToken);
        var dto = memberDtos.FirstOrDefault(m => m.Id == request.MemberId);

        if (dto is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        return ProjectMemberMapper.ToResponse(member, dto);
    }
}

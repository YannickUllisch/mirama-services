using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.AddProjectMember;

public class AddProjectMemberController : OrganizationControllerBase
{
    [HttpPost("projects/{projectId:guid}/members")]
    public async Task<IActionResult> AddMember(
        [FromRoute] Guid projectId,
        [FromBody] AddProjectMemberCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ProjectId = projectId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(r => Created($"/projects/{projectId}/members/{r.MemberId}", r), Problem);
    }
}

internal class AddProjectMemberCommandHandler(
    IPMCommandRepository<Project, ProjectId> commandRepo,
    IMemberService memberService)
    : IRequestHandler<AddProjectMemberCommand, ErrorOr<ProjectMemberResponse>>
{
    public async Task<ErrorOr<ProjectMemberResponse>> HandleAsync(AddProjectMemberCommand request, CancellationToken cancellationToken)
    {
        var project = await commandRepo.Query()
            .Include(p => p.Members)
            .FirstOrDefaultAsync(p => p.Id == new ProjectId(request.ProjectId), cancellationToken);

        if (project is null)
            return Error.NotFound("Project.NotFound", "Project not found.");

        var addResult = project.AddMember(new ProjectMemberDetails(request.MemberId, request.RoleId));
        if (addResult.IsError) return addResult.Errors;

        commandRepo.Update(project);

        var member = project.Members.Find(m => m.MemberId == request.MemberId)!;
        var memberDto = await memberService.GetMembersByIdsAsync([request.MemberId], cancellationToken);
        var dto = memberDto.FirstOrDefault(m => m.Id == request.MemberId);

        if (dto is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        return ProjectMemberMapper.ToResponse(member, dto);
    }
}

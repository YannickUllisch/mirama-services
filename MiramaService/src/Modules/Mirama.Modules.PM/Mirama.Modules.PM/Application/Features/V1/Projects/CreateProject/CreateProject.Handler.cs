using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.CreateProject;

public class CreateProjectController : OrganizationControllerBase
{
    [HttpPost("/projects")]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken ct)
    {
        var result = await Dispatcher.Send(command, ct);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.ProjectId }, r), Problem);
    }
}

internal class CreateProjectCommandHandler(
    IPMCommandRepository<Project, ProjectId> repo)
    : IRequestHandler<CreateProjectCommand, ErrorOr<ProjectResponse>>
{
    public Task<ErrorOr<ProjectResponse>> HandleAsync(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Project.Create(new ProjectDetails(
            request.Name,
            request.StartDate,
            request.StatusId,
            request.PriorityId,
            request.Description,
            request.EndDate,
            request.Budget));

        foreach (var tagId in request.TagIds)
        {
            var tagResult = project.AddTag(tagId);
            if (tagResult.IsError) return Task.FromResult<ErrorOr<ProjectResponse>>(tagResult.Errors);
        }

        foreach (var member in request.Members)
        {
            var memberResult = project.AddMember(new ProjectMemberDetails(member.MemberId, member.RoleId));
            if (memberResult.IsError) return Task.FromResult<ErrorOr<ProjectResponse>>(memberResult.Errors);
        }

        foreach (var teamId in request.TeamIds)
        {
            var teamResult = project.AddTeam(teamId);
            if (teamResult.IsError) return Task.FromResult<ErrorOr<ProjectResponse>>(teamResult.Errors);
        }

        foreach (var milestone in request.Milestones)
        {
            project.AddMilestone(new ProjectMilestoneDetails(
                milestone.Title,
                milestone.DueDate,
                milestone.Description,
                milestone.Color));
        }

        repo.Add(project);

        return Task.FromResult<ErrorOr<ProjectResponse>>(ProjectMapper.ToResponse(project));
    }
}

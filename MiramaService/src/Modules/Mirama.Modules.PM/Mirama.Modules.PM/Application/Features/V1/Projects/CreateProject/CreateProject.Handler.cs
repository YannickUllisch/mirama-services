using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Contracts.Tags;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
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
    IPMCommandRepository<Project, ProjectId> repo,
    IPMCommandRepository<WorkflowConfig, WorkflowConfigId> workflowRepo,
    IMemberService memberService,
    ITeamService teamService,
    ITagService tagService)
    : IRequestHandler<CreateProjectCommand, ErrorOr<ProjectResponse>>
{
    public async Task<ErrorOr<ProjectResponse>> HandleAsync(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var workflowConfig = WorkflowConfig.CreateWithDefaults(Guid.Empty);

        var defaultStatus = workflowConfig.Statuses.First(s => s.IsDefault);
        var defaultPriority = workflowConfig.Priorities.First(p => p.IsDefault);

        var project = Project.Create(new ProjectDetails(
            request.Name,
            request.StartDate,
            defaultStatus.Id.Value,
            defaultPriority.Id.Value,
            request.Description,
            request.EndDate,
            request.Budget));

        workflowConfig.SetProjectId(project.Id.Value);

        foreach (var tagId in request.TagIds)
        {
            var tagResult = project.AddTag(tagId);
            if (tagResult.IsError) return tagResult.Errors;
        }

        foreach (var member in request.Members)
        {
            var memberResult = project.AddMember(new ProjectMemberDetails(member.MemberId, member.RoleId));
            if (memberResult.IsError) return memberResult.Errors;
        }

        foreach (var teamId in request.TeamIds)
        {
            var teamResult = project.AddTeam(teamId);
            if (teamResult.IsError) return teamResult.Errors;
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
        workflowRepo.Add(workflowConfig);

        var membersTask = memberService.GetMembersByIdsAsync(
            project.Members.Select(m => m.MemberId).Distinct(), cancellationToken);

        var teamsTask = teamService.GetTeamsByIdsAsync(
            project.Teams.Select(t => t.TeamId).Distinct(), cancellationToken);

        var tagsTask = tagService.GetTagsByIdsAsync(project.TagIds, cancellationToken);

        await Task.WhenAll(membersTask, teamsTask, tagsTask);

        var statusLookup = workflowConfig.Statuses.ToDictionary(s => s.Id.Value);
        var priorityLookup = workflowConfig.Priorities.ToDictionary(p => p.Id.Value);
        var memberLookup = (await membersTask).ToDictionary(m => m.Id);
        var teamLookup = (await teamsTask).ToDictionary(t => t.Id);
        var tagLookup = (await tagsTask).ToDictionary(t => t.Id);

        return ProjectMapper.ToResponse(project, statusLookup, priorityLookup, tagLookup, memberLookup, teamLookup);
    }
}

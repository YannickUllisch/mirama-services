using ErrorOr;
using Microsoft.AspNetCore.Mvc;
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
    [HttpPost("projects")]
    public async Task<IActionResult> Create([FromBody] CreateProjectCommand command, CancellationToken ct)
    {
        var result = await Dispatcher.Send(command, ct);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.Id }, r), Problem);
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
        var workflowConfig = WorkflowConfig.CreateWithDefaults();

        if (request.DefaultProjectStatusName is not null)
        {
            var setStatus = workflowConfig.SetDefaultStatusByName(request.DefaultProjectStatusName);
            if (setStatus.IsError) return setStatus.Errors;
        }

        if (request.DefaultProjectPriorityName is not null)
        {
            var setPriority = workflowConfig.SetDefaultPriorityByName(request.DefaultProjectPriorityName);
            if (setPriority.IsError) return setPriority.Errors;
        }

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

        workflowConfig.SetProjectId(project.Id);

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

        var members = await memberService.GetMembersByIdsAsync(
            project.Members.Select(m => m.MemberId).Distinct(), cancellationToken);

        var teams = await teamService.GetTeamsByIdsAsync(
            project.Teams.Select(t => t.TeamId).Distinct(), cancellationToken);

        var tags = await tagService.GetTagsByIdsAsync(project.TagIds, cancellationToken);

        var statusLookup = workflowConfig.Statuses.ToDictionary(s => s.Id.Value);
        var priorityLookup = workflowConfig.Priorities.ToDictionary(p => p.Id.Value);
        var memberLookup = members.ToDictionary(m => m.Id);
        var teamLookup = teams.ToDictionary(t => t.Id);
        var tagLookup = tags.ToDictionary(t => t.Id);

        return ProjectMapper.ToResponse(project, statusLookup, priorityLookup, tagLookup, memberLookup, teamLookup);
    }
}

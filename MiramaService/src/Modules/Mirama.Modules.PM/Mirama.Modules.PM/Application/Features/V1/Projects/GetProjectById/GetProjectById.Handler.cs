using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.Modules.Identity.Contracts.Tags;
using Mirama.Modules.PM.Application.Common.Interfaces;
using Mirama.Modules.PM.Domain.Aggregates.Project;
using Mirama.Modules.PM.Domain.Aggregates.WorkflowConfig;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjectById;

public class GetProjectByIdController : OrganizationControllerBase
{
    [HttpGet("projects/{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetProjectByIdQuery(id), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectByIdQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo,
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo,
    IMemberService memberService,
    ITeamService teamService,
    ITagService tagService)
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

        var workflowTask = workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .Include(wc => wc.Priorities)
            .FirstOrDefaultAsync(wc => wc.ProjectId == project.Id, cancellationToken);

        var members = await memberService.GetMembersByIdsAsync(
            project.Members.Select(m => m.MemberId).Distinct(), cancellationToken);

        var teams = await teamService.GetTeamsByIdsAsync(
            project.Teams.Select(t => t.TeamId).Distinct(), cancellationToken);

        var tags = await tagService.GetTagsByIdsAsync(project.TagIds, cancellationToken);

        var workflowConfig = await workflowTask;
        if (workflowConfig is null)
            return Error.NotFound("WorkflowConfig.NotFound", "Workflow configuration not found.");

        var statusLookup = workflowConfig.Statuses.ToDictionary(s => s.Id.Value);
        var priorityLookup = workflowConfig.Priorities.ToDictionary(p => p.Id.Value);
        var memberLookup = members.ToDictionary(m => m.Id);
        var teamLookup = teams.ToDictionary(t => t.Id);
        var tagLookup = tags.ToDictionary(t => t.Id);

        return ProjectMapper.ToResponse(project, statusLookup, priorityLookup, tagLookup, memberLookup, teamLookup);
    }
}

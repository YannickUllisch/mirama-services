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

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjects;

public class GetProjectsController : OrganizationControllerBase
{
    [HttpGet("/projects")]
    public async Task<IActionResult> Get([FromQuery] GetProjectsQuery query, CancellationToken ct)
    {
        var result = await Dispatcher.Send(query, ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetProjectsQueryHandler(
    IPMQueryRepository<Project, ProjectId> queryRepo,
    IPMQueryRepository<WorkflowConfig, WorkflowConfigId> workflowRepo,
    IMemberService memberService,
    ITeamService teamService,
    ITagService tagService)
    : IRequestHandler<GetProjectsQuery, ErrorOr<PaginatedList<ProjectResponse>>>
{
    public async Task<ErrorOr<PaginatedList<ProjectResponse>>> HandleAsync(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var baseQuery = queryRepo.Query()
            .Include(p => p.Members)
            .Include(p => p.Teams)
            .Include(p => p.Milestones);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        List<Project> projects;
        if (request.PageNumber.HasValue && request.PageSize.HasValue)
        {
            projects = await baseQuery
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToListAsync(cancellationToken);
        }
        else
        {
            projects = await baseQuery.ToListAsync(cancellationToken);
        }

        if (projects.Count == 0)
            return new PaginatedList<ProjectResponse>([], totalCount, request.PageNumber ?? 1, Math.Max(totalCount, 1));

        var allMemberIds = projects.SelectMany(p => p.Members.Select(m => m.MemberId)).Distinct();
        var allTeamIds = projects.SelectMany(p => p.Teams.Select(t => t.TeamId)).Distinct();
        var allTagIds = projects.SelectMany(p => p.TagIds).Distinct();

        var projectIds = projects.Select(p => p.Id.Value).ToList();

        var workflowTask = workflowRepo.Query()
            .Include(wc => wc.Statuses)
            .Include(wc => wc.Priorities)
            .Where(wc => projectIds.Contains(wc.ProjectId))
            .ToListAsync(cancellationToken);

        var membersTask = memberService.GetMembersByIdsAsync(allMemberIds, cancellationToken);
        var teamsTask = teamService.GetTeamsByIdsAsync(allTeamIds, cancellationToken);
        var tagsTask = tagService.GetTagsByIdsAsync(allTagIds, cancellationToken);

        await Task.WhenAll(workflowTask, membersTask, teamsTask, tagsTask);

        var workflowConfigs = await workflowTask;
        var statusLookup = workflowConfigs.SelectMany(wc => wc.Statuses).ToDictionary(s => s.Id.Value);
        var priorityLookup = workflowConfigs.SelectMany(wc => wc.Priorities).ToDictionary(p => p.Id.Value);
        var memberLookup = (await membersTask).ToDictionary(m => m.Id);
        var teamLookup = (await teamsTask).ToDictionary(t => t.Id);
        var tagLookup = (await tagsTask).ToDictionary(t => t.Id);

        var items = projects
            .Select(p => ProjectMapper.ToResponse(p, statusLookup, priorityLookup, tagLookup, memberLookup, teamLookup))
            .ToList();

        return new PaginatedList<ProjectResponse>(items, totalCount, request.PageNumber ?? 1, Math.Max(totalCount, 1));
    }
}

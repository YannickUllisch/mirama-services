using ErrorOr;
using Mirama.Modules.PM.Domain.Aggregates.Project.Member;
using Mirama.Modules.PM.Domain.Aggregates.Project.Milestone;
using Mirama.Modules.PM.Domain.Aggregates.Project.Team;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.PM.Domain.Aggregates.Project;

public sealed class Project : OrganizationAggregateRoot<ProjectId>
{
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime? EndDate { get; private set; }
    public Guid StatusId { get; private set; }
    public Guid PriorityId { get; private set; }
    public int Budget { get; private set; }
    public bool IsArchived { get; private set; }
    public DateTime DateCreated { get; private set; }

    public List<Guid> TagIds { get; private set; } = [];
    public List<ProjectMember> Members { get; private set; } = [];
    public List<ProjectTeam> Teams { get; private set; } = [];
    public List<ProjectMilestone> Milestones { get; private set; } = [];

    private Project(ProjectDetails details)
    {
        this.Name = details.Name.Trim();
        this.Slug = GenerateSlug(details.Name);
        this.Description = details.Description?.Trim();
        this.StartDate = details.StartDate;
        this.EndDate = details.EndDate;
        this.StatusId = details.StatusId;
        this.PriorityId = details.PriorityId;
        this.Budget = details.Budget;
        this.IsArchived = false;
        this.DateCreated = DateTime.UtcNow;
    }

    private Project() { }

    public static Project Create(ProjectDetails details) =>
        new Project(details) { Id = new ProjectId(Guid.NewGuid()) };

    public void Update(ProjectDetails details)
    {
        this.Name = details.Name.Trim();
        this.Slug = GenerateSlug(details.Name);
        this.Description = details.Description?.Trim();
        this.StartDate = details.StartDate;
        this.EndDate = details.EndDate;
        this.StatusId = details.StatusId;
        this.PriorityId = details.PriorityId;
        this.Budget = details.Budget;
    }

    public void SetStatus(Guid statusId)
    {
        this.StatusId = statusId;
    }

    public void SetPriority(Guid priorityId)
    {
        this.PriorityId = priorityId;
    }

    public void Archive()
    {
        this.IsArchived = true;
    }

    public void Restore()
    {
        this.IsArchived = false;
    }

    // --- Members ---

    public ErrorOr<Created> AddMember(ProjectMemberDetails details)
    {
        var existing = this.Members.Find(m => m.MemberId == details.MemberId);
        if (existing is not null)
        {
            if (!existing.IsInherited)
                return Error.Conflict("Project.Member.AlreadyExists", "Member is already directly assigned.");
            existing.SetDirectAssignment(details.RoleId);
            return Result.Created;
        }
        this.Members.Add(ProjectMember.Create(details));
        return Result.Created;
    }

    public void AddInheritedMember(Guid memberId, Guid roleId, Guid teamId)
    {
        if (this.Members.Any(m => m.MemberId == memberId)) return;
        this.Members.Add(ProjectMember.CreateInherited(memberId, roleId, teamId));
    }

    public ErrorOr<Deleted> RemoveMember(Guid memberId)
    {
        var member = this.Members.Find(m => m.MemberId == memberId);
        if (member is null)
            return Error.NotFound("Project.Member.NotFound", "Member not found.");
        this.Members.Remove(member);
        return Result.Deleted;
    }

    public ErrorOr<Success> UpdateMemberRole(Guid memberId, Guid roleId)
    {
        var member = this.Members.Find(m => m.MemberId == memberId);
        if (member is null)
            return Error.NotFound("Project.Member.NotFound", "Member not found.");
        member.UpdateRole(roleId);
        return Result.Success;
    }

    public bool HasMember(Guid memberId) =>
        this.Members.Any(m => m.MemberId == memberId);

    // --- Teams ---

    public ErrorOr<Created> AddTeam(Guid teamId)
    {
        if (this.Teams.Any(t => t.TeamId == teamId))
            return Error.Conflict("Project.Team.AlreadyExists", "Team already assigned.");
        this.Teams.Add(ProjectTeam.Create(teamId));
        return Result.Created;
    }

    public ErrorOr<Deleted> RemoveTeam(Guid teamId)
    {
        var team = this.Teams.Find(t => t.TeamId == teamId);
        if (team is null)
            return Error.NotFound("Project.Team.NotFound", "Team not assigned.");
        this.Teams.Remove(team);
        return Result.Deleted;
    }

    // --- Milestones ---

    public ProjectMilestone AddMilestone(ProjectMilestoneDetails details)
    {
        var milestone = ProjectMilestone.Create(details);
        this.Milestones.Add(milestone);
        return milestone;
    }

    public ErrorOr<Deleted> RemoveMilestone(ProjectMilestoneId milestoneId)
    {
        var milestone = this.Milestones.Find(m => m.Id == milestoneId);
        if (milestone is null)
            return Error.NotFound("Project.Milestone.NotFound", "Milestone not found.");
        this.Milestones.Remove(milestone);
        return Result.Deleted;
    }

    // --- Tags ---

    public ErrorOr<Success> AddTag(Guid tagId)
    {
        if (this.TagIds.Contains(tagId))
            return Error.Conflict("Project.Tag.Duplicate", "Tag already applied.");
        this.TagIds.Add(tagId);
        return Result.Success;
    }

    public void RemoveTag(Guid tagId)
    {
        this.TagIds.Remove(tagId);
    }

    private static string GenerateSlug(string name) =>
        name.Trim().ToLowerInvariant().Replace(' ', '-');
}

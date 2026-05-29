using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Seeding;

public static class PolicySeed
{
    private record SeedStatement(string Action, string Resource, Effect Effect = Effect.Allow);

    private record SeedPolicy(
        string Name,
        string Description,
        AccessScope Scope,
        IReadOnlyList<SeedStatement> Statements);

    private static readonly IReadOnlyList<SeedPolicy> Seeds =
    [
        // ── Organization policies ──────────────────────────────────────────────
        new("full-access-policy",
            "Unrestricted access to all resources",
            AccessScope.Organization,
        [
            new(Permissions.Wildcard, Permissions.Wildcard),
        ]),

        new("organization-manager-policy",
            "Manage organization settings and configuration",
            AccessScope.Organization,
        [
            new(Permissions.Organization.Read,   Permissions.Organization.ResourcePattern),
            new(Permissions.Organization.Update, Permissions.Organization.ResourcePattern),
        ]),

        new("member-management-policy",
            "Full control over members, invitations and teams",
            AccessScope.Organization,
        [
            new(Permissions.Member.AllActions,     Permissions.Member.ResourcePattern),
            new(Permissions.Invitation.AllActions, Permissions.Invitation.ResourcePattern),
            new(Permissions.Team.AllActions,       Permissions.Team.ResourcePattern),
            new(Permissions.TeamMember.AllActions, Permissions.TeamMember.ResourcePattern),
        ]),

        new("project-manager-policy",
            "Full CRUD on all projects across the organization",
            AccessScope.Organization,
        [
            new(Permissions.Project.AllActions,   Permissions.Project.ResourcePattern),
            new(Permissions.Task.AllActions,      Permissions.Task.ResourcePattern),
            new(Permissions.Comment.AllActions,   Permissions.Comment.ResourcePattern),
            new(Permissions.Expense.AllActions,   Permissions.Expense.ResourcePattern),
            new(Permissions.Milestone.AllActions, Permissions.Milestone.ResourcePattern),
            new(Permissions.Tag.AllActions,       Permissions.Tag.ResourcePattern),
        ]),

        new("read-only-policy",
            "Read-only access to all resources at the organization level",
            AccessScope.Organization,
        [
            new(Permissions.ReadAll, Permissions.Wildcard),
        ]),

        // ── Project policies ───────────────────────────────────────────────────
        new("project-full-access-policy",
            "Unrestricted access within a project",
            AccessScope.Project,
        [
            new(Permissions.Wildcard, Permissions.Wildcard),
        ]),

        new("project-contributor-policy",
            "Create and manage tasks, milestones, comments, and expenses within a project",
            AccessScope.Project,
        [
            new(Permissions.Project.Read,      Permissions.Project.ResourcePattern),
            new(Permissions.Task.Read,         Permissions.Task.ResourcePattern),
            new(Permissions.Task.Create,       Permissions.Task.ResourcePattern),
            new(Permissions.Task.Update,       Permissions.Task.ResourcePattern),
            new(Permissions.Task.Assign,       Permissions.Task.ResourcePattern),
            new(Permissions.Comment.AllActions,   Permissions.Comment.ResourcePattern),
            new(Permissions.Milestone.Read,    Permissions.Milestone.ResourcePattern),
            new(Permissions.Milestone.Create,  Permissions.Milestone.ResourcePattern),
            new(Permissions.Milestone.Update,  Permissions.Milestone.ResourcePattern),
            new(Permissions.Expense.AllActions,   Permissions.Expense.ResourcePattern),
            new(Permissions.Tag.Read,          Permissions.Tag.ResourcePattern),
        ]),

        new("project-reviewer-policy",
            "Read-only access with the ability to leave comments — ideal for client reviews",
            AccessScope.Project,
        [
            new(Permissions.Project.Read,    Permissions.Project.ResourcePattern),
            new(Permissions.Task.Read,       Permissions.Task.ResourcePattern),
            new(Permissions.Comment.Read,    Permissions.Comment.ResourcePattern),
            new(Permissions.Comment.Create,  Permissions.Comment.ResourcePattern),
            new(Permissions.Milestone.Read,  Permissions.Milestone.ResourcePattern),
            new(Permissions.Expense.Read,    Permissions.Expense.ResourcePattern),
            new(Permissions.Tag.Read,        Permissions.Tag.ResourcePattern),
        ]),

        new("project-readonly-policy",
            "Read-only access within a project",
            AccessScope.Project,
        [
            new(Permissions.ReadAll, Permissions.Wildcard),
        ]),
    ];

    public static async Task SeedDataAsync(IdentityDbContext dbContext)
    {
        var existingNames = await dbContext.Policies
            .Where(p => p.TenantId == null)
            .Select(p => p.Name)
            .ToHashSetAsync();

        var toAdd = Seeds
            .Where(s => !existingNames.Contains(s.Name))
            .ToList();

        if (toAdd.Count == 0)
            return;

        foreach (var seed in toAdd)
        {
            var policy = Policy.Create(new PolicyDetails(
                seed.Name,
                seed.Scope,
                TenantId: null,
                Description: seed.Description,
                IsManaged: true));

            foreach (var stmt in seed.Statements)
                policy.AddStatement(stmt.Action, stmt.Resource, stmt.Effect);

            dbContext.Policies.Add(policy);
        }

        await dbContext.SaveChangesAsync();
    }
}

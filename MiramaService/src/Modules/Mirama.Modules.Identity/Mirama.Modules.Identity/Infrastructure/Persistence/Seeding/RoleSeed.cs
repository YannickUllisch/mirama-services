using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Seeding;

public static class RoleSeed
{
    private record SeedRole(
        string Name,
        string Description,
        AccessScope Scope,
        IReadOnlyList<string> PolicyNames);

    private static readonly IReadOnlyList<SeedRole> Seeds =
    [
        new("Owner", "Full unrestricted access - cannot be removed", AccessScope.Organization,
            ["full-access-policy"]),
        new("Admin", "Administrative access to organization settings, members, projects, and content", AccessScope.Organization,
            ["organization-manager-policy", "member-management-policy", "project-manager-policy"]),
        new("Manager", "Manage projects, tasks, content, and team members without org-level settings access", AccessScope.Organization,
            ["member-management-policy", "project-manager-policy"]),
        new("Member", "Read-only access across the organization", AccessScope.Organization,
            ["read-only-policy"]),

        new("Project Lead", "Full control within the assigned project", AccessScope.Project,
            ["project-full-access-policy"]),
        new("Contributor", "Create and manage tasks, milestones, expenses, and comments within the project", AccessScope.Project,
            ["project-contributor-policy"]),
        new("Client", "Read tasks and leave review comments - designed for external client access", AccessScope.Project,
            ["project-reviewer-policy"]),
        new("Project Viewer", "Read-only access within the assigned project", AccessScope.Project,
            ["project-readonly-policy"]),
    ];

    public static async Task SeedDataAsync(IdentityDbContext dbContext)
    {
        var existingNames = await dbContext.Roles
            .Where(r => r.TenantId == null)
            .Select(r => r.Name)
            .ToHashSetAsync();

        var toAdd = Seeds
            .Where(s => !existingNames.Contains(s.Name))
            .ToList();

        if (toAdd.Count == 0)
            return;

        var policyLookup = await dbContext.Policies
            .Where(p => p.TenantId == null)
            .ToDictionaryAsync(p => p.Name, p => p.Id);

        foreach (var seed in toAdd)
        {
            var role = Role.Create(new RoleDetails(
                seed.Name,
                seed.Scope,
                TenantId: null,
                Description: seed.Description));

            foreach (var policyName in seed.PolicyNames)
            {
                if (policyLookup.TryGetValue(policyName, out var policyId))
                    role.AttachPolicy(policyId);
            }

            dbContext.Roles.Add(role);
        }

        await dbContext.SaveChangesAsync();
    }
}

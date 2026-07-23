---
name: gortex-v1-auth-27-dirs
description: "Work in the V1/Auth +27 dirs area — 75 symbols across 55 files (68% cohesion)"
---

# V1/Auth +27 dirs

75 symbols | 55 files | 68% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/ICQRSRequests.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizationById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitationById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetTenantInvitations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMemberById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMembers.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Types.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/ICQRSRequests.cs` | IQuery |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Types.cs` | GetClientsQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs` | GetAvailablePermissionsQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs` | GetMemberPermissionsQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs` | GetPoliciesQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs` | GetPolicyByIdQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs` | GetRoleByIdQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs` | GetRolesQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs` | GetRolesWithPoliciesQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs` | GetAuthUserQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs` | GetAuthUserByEmailQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs` | GetOrgMembershipQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs` | GetBillingUsageQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs` | GetPlanByIdQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs` | GetPlansQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizationById.cs` | request, GetOrganizationByIdQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizations.cs` | GetOrganizationsQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitationById.cs` | GetInvitationByIdQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitations.cs` | request, GetInvitationsQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetTenantInvitations.cs` | GetTenantInvitationsQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMemberById.cs` | GetMemberByIdQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMembers.cs` | GetMembersQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs` | request, GetTagByIdQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs` | GetTagsQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs` | GetTeamByIdQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs` | request, GetTeamMembersQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs` | GetTeamsQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs` | request, GetTenantQuery |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs` | GetUsersQuery, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs` | GetMyInvitationsQuery, request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Types.cs` | GetProjectTemplateByIdQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Types.cs` | GetProjectTemplatesQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Types.cs` | GetProjectByIdQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Types.cs` | GetProjectsQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Types.cs` | GetProjectMembersQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Types.cs` | GetProjectMilestonesQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Types.cs` | GetProjectTeamsQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Types.cs` | GetProjectWorkflowQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Types.cs` | GetProjectPrioritiesQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Types.cs` | GetProjectStatusesQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Types.cs` | GetTaskPrioritiesQuery |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Types.cs` | GetTaskStatusesQuery |

## How to Explore

```
get_communities with id: "community-35"
smart_context with task: "understand V1/Auth +27 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._

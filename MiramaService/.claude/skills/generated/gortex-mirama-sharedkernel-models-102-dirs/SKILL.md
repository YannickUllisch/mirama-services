---
name: gortex-mirama-sharedkernel-models-102-dirs
description: "Work in the Mirama.SharedKernel/Models +102 dirs area — 343 symbols across 128 files (71% cohesion)"
---

# Mirama.SharedKernel/Models +102 dirs

343 symbols | 128 files | 71% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IDispatcher.cs`
- `src/Mirama.SharedKernel/Extensions/QueryableExtensions.cs`
- `src/Mirama.SharedKernel/Models/ApiControllerBase.cs`
- `src/Mirama.SharedKernel/Models/Dispatcher.cs`
- `src/Mirama.SharedKernel/Models/OrganizationControllerBase.cs`
- `src/Mirama.SharedKernel/Models/PaginatedList.cs`
- `src/Mirama.SharedKernel/Models/Permissions/Permissions.cs`
- `src/Mirama.SharedKernel/Models/TenantControllerBase.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/PolicyResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/RoleResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizationById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/AcceptInvitation/AcceptInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/DeclineInvitation/DeclineInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/ExtendInvitation/ExtendInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitationById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetTenantInvitations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMemberById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMembers.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/MemberResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/OrganizationResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/TagResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/TeamResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/TenantResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IDispatcher.cs` | Send, IDispatcher, cancellationToken, TResponse, request |
| `src/Mirama.SharedKernel/Extensions/QueryableExtensions.cs` | pageNumber, pageSize, QueryableExtensions, queryable, TDestination, ... |
| `src/Mirama.SharedKernel/Models/ApiControllerBase.cs` | ToNoContent, _dispatcher, ToCreated, ToOk, ApiControllerBase, ... |
| `src/Mirama.SharedKernel/Models/Dispatcher.cs` | _serviceProvider, Dispatcher |
| `src/Mirama.SharedKernel/Models/OrganizationControllerBase.cs` | OrganizationControllerBase |
| `src/Mirama.SharedKernel/Models/PaginatedList.cs` | HasPreviousPage, pageSize, pageNumber, PaginatedList, TotalPages, ... |
| `src/Mirama.SharedKernel/Models/Permissions/Permissions.cs` | All, Wildcard, Permissions, AllGroups, ReadAll |
| `src/Mirama.SharedKernel/Models/TenantControllerBase.cs` | TenantControllerBase |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs` | ct, command, CreateClientController, Create |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs` | GetClientsController |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs` | ct, command, AddContactController, Add, clientId |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs` | ct, command, Accept, AcceptInvitationController |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs` | Invite, clientId, command, ct, InviteContactController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs` | GetAvailablePermissionsController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs` | GetMemberPermissionsController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs` | Create, command, CreatePolicyController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs` | DeletePolicyController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs` | GetPoliciesController, query, scope, Get |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs` | GetPolicyByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/PolicyResponse.cs` | PolicyResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs` | id, Update, command, UpdatePolicyController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs` | AttachPolicyController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs` | Create, command, CreateRoleController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs` | DeleteRoleController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs` | DetachPolicyController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs` | GetRoleByIdController, RequirePermission |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs` | Get, scope, query, GetRolesController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs` | GetRolesWithPoliciesController, Get, query, scope |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/RoleResponse.cs` | RoleResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs` | UpdateRoleController, command, id, Update |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs` | GetAuthUserController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs` | GetAuthUserByEmailController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs` | GetOrgMembershipController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs` | LinkUserExternalIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs` | command, Post, SetupUserController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs` | GetBillingUsageController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs` | GetPlanByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs` | GetPlansController, HttpGet |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Handler.cs` | command, CreateOrganizationController, Create |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs` | DeleteOrganizationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizationById.cs` | GetOrganizationByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizations.cs` | query, Get, GetOrganizationsController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/AcceptInvitation/AcceptInvitation.cs` | AcceptInvitationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/DeclineInvitation/DeclineInvitation.cs` | DeclineInvitationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/ExtendInvitation/ExtendInvitation.cs` | ExtendInvitationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitationById.cs` | GetInvitationByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitations.cs` | GetInvitationsController, Get, query |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetTenantInvitations.cs` | Get, query, GetTenantInvitationsController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs` | RevokeInvitationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Handler.cs` | Send, SendInvitationController, command |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMemberById.cs` | GetMemberByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMembers.cs` | Get, query, GetMembersController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/MemberResponse.cs` | MemberResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs` | RemoveMemberController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs` | Update, memberId, UpdateMemberController, HttpPatch, command |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/OrganizationResponse.cs` | OrganizationResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs` | Create, CreateTagController, command |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs` | DeleteTagController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs` | GetTagByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs` | GetTagsController, query, Get |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/TagResponse.cs` | TagResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs` | command, Update, UpdateTagController, tagId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs` | Add, teamId, AddTeamMemberController, command |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs` | command, Create, CreateTeamController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs` | DeleteTeamController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs` | GetTeamByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs` | GetTeamMembersController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs` | GetTeamsController, query, Get |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs` | RemoveTeamMemberController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/TeamResponse.cs` | TeamResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs` | UpdateTeamController, command, Update, teamId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs` | command, id, UpdateOrganizationController, Update |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs` | CancelSubscriptionController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs` | GetTenantController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/TenantResponse.cs` | TenantResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs` | Update, UpdateSubscriptionController, command |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs` | Update, command, UpdateTenantSettingsController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs` | query, Get, GetUserByIdController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs` | AcceptMyInvitationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs` | DeclineMyInvitationController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs` | GetMyInvitationsController |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs` | UpdateUserController, HttpPut, id, Update, command |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Handler.cs` | CreateProjectTemplateController, Create, ct, command |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Handler.cs` | projectTemplateId, ct, Add, command, AddCycleTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Handler.cs` | RemoveCycleTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Handler.cs` | command, projectTemplateId, UpdateCycleTemplateController, cycleTemplateId, Update, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Handler.cs` | DeleteProjectTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Handler.cs` | GetProjectTemplateByIdController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Handler.cs` | GetProjectTemplatesController, query, ct, Get |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Handler.cs` | command, AddMilestoneTemplateController, projectTemplateId, ct, Add |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Handler.cs` | RemoveMilestoneTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Handler.cs` | command, projectTemplateId, milestoneTemplateId, UpdateMilestoneTemplateController, Update, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Handler.cs` | command, projectTemplateId, ct, Add, AddTaskTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Handler.cs` | RemoveTaskTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Handler.cs` | command, Update, id, ct, UpdateProjectTemplateController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Handler.cs` | HttpPost, ArchiveProjectController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Handler.cs` | CreateProjectController, ct, command, Create |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Handler.cs` | GetProjectByIdController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Handler.cs` | GetProjectsController, Get, query, ct |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Handler.cs` | AddMember, AddProjectMemberController, command, projectId, ct |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Handler.cs` | GetProjectMembersController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Handler.cs` | RemoveProjectMemberController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Handler.cs` | projectId, UpdateMember, memberId, UpdateProjectMemberController, command, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Handler.cs` | command, CreateProjectMilestoneController, projectId, ct, CreateMilestone |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Handler.cs` | DeleteProjectMilestoneController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Handler.cs` | GetProjectMilestonesController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Handler.cs` | projectId, milestoneId, UpdateMilestone, ct, UpdateProjectMilestoneController, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Handler.cs` | projectId, ct, AddProjectTeamController, command, AddTeam |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Handler.cs` | GetProjectTeamsController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs` | RemoveProjectTeamController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs` | ct, UpdateProjectController, id, command, Update |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs` | GetProjectWorkflowController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs` | projectId, AddProjectPriorityController, command, ct, AddPriority |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs` | GetProjectPrioritiesController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs` | RemoveProjectPriorityController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs` | command, projectId, priorityId, UpdateProjectPriorityController, UpdatePriority, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs` | AddProjectStatusController, AddStatus, command, ct, projectId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs` | GetProjectStatusesController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs` | RemoveProjectStatusController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs` | ct, UpdateStatus, statusId, projectId, UpdateProjectStatusController, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs` | command, AddTaskPriority, AddTaskPriorityController, ct, projectId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs` | GetTaskPrioritiesController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs` | RemoveTaskPriorityController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs` | projectId, ct, command, UpdateTaskPriority, UpdateTaskPriorityController, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs` | AddTaskStatus, command, ct, projectId, AddTaskStatusController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs` | GetTaskStatusesController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs` | RemoveTaskStatusController |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs` | projectId, command, UpdateTaskStatusController, ct, statusId, ... |

## How to Explore

```
get_communities with id: "community-55"
smart_context with task: "understand Mirama.SharedKernel/Models +102 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._

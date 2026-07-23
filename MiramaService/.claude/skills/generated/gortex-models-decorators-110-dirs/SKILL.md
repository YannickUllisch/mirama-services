---
name: gortex-models-decorators-110-dirs
description: "Work in the Models/Decorators +110 dirs area — 178 symbols across 131 files (81% cohesion)"
---

# Models/Decorators +110 dirs

178 symbols | 131 files | 81% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/INotificationHandler.cs`
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IRequestHandlers.cs`
- `src/Mirama.SharedKernel/Abstractions/Persistence/IGlobalRoleProvider.cs`
- `src/Mirama.SharedKernel/Infrastructure/Options/ApplicationOptions.cs`
- `src/Mirama.SharedKernel/Infrastructure/Options/AuthenticationOptions.cs`
- `src/Mirama.SharedKernel/Models/Decorators/AuditDecorator.cs`
- `src/Mirama.SharedKernel/Models/Decorators/LoggingDecorator.cs`
- `src/Mirama.SharedKernel/Models/Decorators/ValidationDecorator.cs`
- `src/Mirama.SharedKernel/SharedConfiguration.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Common/ClientsTransactionDecorator.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/ConfigureServices.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Common/IdentityTransactionDecorator.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/AvailablePermissionsResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs`
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
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Validation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Models/GlobalRoleProvider.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/ConfigureServices.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/PMTransactionDecorator.cs`
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
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/ConfigureServices.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/INotificationHandler.cs` | TNotification, INotificationHandler |
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IRequestHandlers.cs` | TResponse, TRequest, IRequestHandler |
| `src/Mirama.SharedKernel/Abstractions/Persistence/IGlobalRoleProvider.cs` | AllowedRoles, IGlobalRoleProvider |
| `src/Mirama.SharedKernel/Infrastructure/Options/ApplicationOptions.cs` | CorsOrigins, ApplicationOptions, Key |
| `src/Mirama.SharedKernel/Infrastructure/Options/AuthenticationOptions.cs` | Authority, Key, AuthenticationOptions, NextAuthSecret, Audience |
| `src/Mirama.SharedKernel/Models/Decorators/AuditDecorator.cs` | AuditDecorator, TRequest, TResponse |
| `src/Mirama.SharedKernel/Models/Decorators/LoggingDecorator.cs` | TResponse, TRequest, LoggingDecorator |
| `src/Mirama.SharedKernel/Models/Decorators/ValidationDecorator.cs` | TRequest, TResponse, ValidationDecorator |
| `src/Mirama.SharedKernel/SharedConfiguration.cs` | config, services, DependencyInjection, AddSharedServices |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Common/ClientsTransactionDecorator.cs` | ClientsTransactionDecorator, TRequest, TResponse |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs` | CreateClientCommandHandler |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs` | GetClientsQueryHandler |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs` | AddContactCommandHandler |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs` | AcceptInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs` | InviteContactCommandHandler |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Infrastructure/ConfigureServices.cs` | services, config, ConfigureServices, AddApplication, services, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Common/IdentityTransactionDecorator.cs` | TResponse, IdentityTransactionDecorator, TRequest |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/AvailablePermissionsResponse.cs` | AvailablePermissionsResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs` | GetAvailablePermissionsQueryHandler, Cached |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs` | GetMemberPermissionsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs` | CreatePolicyCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs` | DeletePolicyCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs` | GetPoliciesQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs` | GetPolicyByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs` | UpdatePolicyCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs` | AttachPolicyCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs` | CreateRoleCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs` | DeleteRoleCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs` | DetachPolicyCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs` | GetRoleByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs` | GetRolesQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs` | GetRolesWithPoliciesQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs` | UpdateRoleCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs` | GetAuthUserQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs` | GetAuthUserByEmailQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs` | GetOrgMembershipQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs` | LinkUserExternalIdCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs` | SetupUserCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs` | GetBillingUsageQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs` | GetPlanByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs` | GetPlansQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Handler.cs` | CreateOrganizationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs` | DeleteOrganizationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizationById.cs` | GetOrganizationByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizations.cs` | GetOrganizationsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/AcceptInvitation/AcceptInvitation.cs` | AcceptInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/DeclineInvitation/DeclineInvitation.cs` | DeclineInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/ExtendInvitation/ExtendInvitation.cs` | ExtendInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitationById.cs` | GetInvitationByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitations.cs` | GetInvitationsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetTenantInvitations.cs` | GetTenantInvitationsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs` | RevokeInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Handler.cs` | SendInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMemberById.cs` | GetMemberByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMembers.cs` | GetMembersQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs` | RemoveMemberCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs` | UpdateMemberCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs` | CreateTagCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs` | DeleteTagCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs` | GetTagByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs` | GetTagsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs` | UpdateTagCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs` | AddTeamMemberCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs` | CreateTeamCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs` | DeleteTeamCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs` | GetTeamByIdQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs` | GetTeamMembersQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs` | GetTeamsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs` | RemoveTeamMemberCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs` | UpdateTeamCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs` | UpdateOrganizationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs` | CancelSubscriptionCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs` | GetTenantQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs` | UpdateSubscriptionCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs` | UpdateTenantSettingsCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs` | GetUsersQueryHandler, _userRepository |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs` | AcceptMyInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs` | DeclineMyInvitationCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs` | GetMyInvitationsQueryHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs` | _repo, UpdateUserCommandHandler |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Validation.cs` | UpdateUserRequestValidator, roleProvider, UpdateUserRequestValidator.<init> |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Models/GlobalRoleProvider.cs` | GlobalRoleProvider, AllowedRoles |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Infrastructure/ConfigureServices.cs` | DependencyInjection, AddApplication, AddIdentityModule, config, services, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/PMTransactionDecorator.cs` | TResponse, TRequest, PMTransactionDecorator |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Handler.cs` | CreateProjectTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Handler.cs` | AddCycleTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Handler.cs` | RemoveCycleTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Handler.cs` | UpdateCycleTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Handler.cs` | DeleteProjectTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Handler.cs` | GetProjectTemplateByIdQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Handler.cs` | GetProjectTemplatesQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Handler.cs` | AddMilestoneTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Handler.cs` | RemoveMilestoneTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Handler.cs` | UpdateMilestoneTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Handler.cs` | AddTaskTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Handler.cs` | RemoveTaskTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Handler.cs` | UpdateProjectTemplateCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Handler.cs` | ArchiveProjectCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Handler.cs` | CreateProjectCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Handler.cs` | GetProjectByIdQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Handler.cs` | GetProjectsQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Handler.cs` | AddProjectMemberCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Handler.cs` | GetProjectMembersQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Handler.cs` | RemoveProjectMemberCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Handler.cs` | UpdateProjectMemberCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Handler.cs` | CreateProjectMilestoneCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Handler.cs` | DeleteProjectMilestoneCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Handler.cs` | GetProjectMilestonesQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Handler.cs` | UpdateProjectMilestoneCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Handler.cs` | AddProjectTeamCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Handler.cs` | GetProjectTeamsQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs` | RemoveProjectTeamCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs` | UpdateProjectCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs` | GetProjectWorkflowQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs` | AddProjectPriorityCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs` | GetProjectPrioritiesQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs` | RemoveProjectPriorityCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs` | UpdateProjectPriorityCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs` | AddProjectStatusCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs` | GetProjectStatusesQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs` | RemoveProjectStatusCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs` | UpdateProjectStatusCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs` | AddTaskPriorityCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs` | GetTaskPrioritiesQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs` | RemoveTaskPriorityCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs` | UpdateTaskPriorityCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs` | AddTaskStatusCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs` | GetTaskStatusesQueryHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs` | RemoveTaskStatusCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs` | UpdateTaskStatusCommandHandler |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/ConfigureServices.cs` | AddApplication, config, AddProjectsModule, services, services, ... |

## Connected Communities

- **Common/Interfaces +10 dirs** (2 cross-edges)
- **Abstractions/Persistence +2 dirs** (1 cross-edges)

## How to Explore

```
get_communities with id: "community-3"
smart_context with task: "understand Models/Decorators +110 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._

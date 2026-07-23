---
name: gortex-v1-billing-109-dirs
description: "Work in the V1/Billing +109 dirs area — 434 symbols across 134 files (96% cohesion)"
---

# V1/Billing +109 dirs

434 symbols | 134 files | 96% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IRequestHandlers.cs`
- `src/Mirama.SharedKernel/Abstractions/Persistence/IUnitOfWork.cs`
- `src/Mirama.SharedKernel/Models/Decorators/AuditDecorator.cs`
- `src/Mirama.SharedKernel/Models/Decorators/LoggingDecorator.cs`
- `src/Mirama.SharedKernel/Models/Decorators/ValidationDecorator.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Common/ClientsTransactionDecorator.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Types.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Types.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Types.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Common/IdentityTransactionDecorator.cs`
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
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/RoleWithPoliciesResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/BillingUsageResponse.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/PlanResponse.cs`
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
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/InvitationResponse.cs`
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
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Organization/Invitation/InvitationId.cs`
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
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/ProjectTeamResponse.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/PriorityResponse.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/StatusResponse.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/ProjectId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/ProjectTemplate/ProjectTemplateId.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/IRequestHandlers.cs` | cancellationToken, request, HandleAsync |
| `src/Mirama.SharedKernel/Abstractions/Persistence/IUnitOfWork.cs` | RollbackTransactionAsync, cancellationToken |
| `src/Mirama.SharedKernel/Models/Decorators/AuditDecorator.cs` | HandleAsync, request, ct |
| `src/Mirama.SharedKernel/Models/Decorators/LoggingDecorator.cs` | request, ct, HandleAsync |
| `src/Mirama.SharedKernel/Models/Decorators/ValidationDecorator.cs` | request, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Common/ClientsTransactionDecorator.cs` | request, HandleAsync, ct |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Types.cs` | ClientResponse |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/GetClients/GetClients.Handler.cs` | Get, cancellationToken, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Types.cs` | ContactResponse |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Types.cs` | PortalSessionResponse |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Types.cs` | InvitationResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Common/IdentityTransactionDecorator.cs` | HandleAsync, request, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetAvailablePermissions.cs` | request, HandleAsync, Get, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/GetMemberPermissions.cs` | request, ct, Get, memberId, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs` | id, request, HandleAsync, ct, Delete |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicies.cs` | ct, HandleAsync, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/GetPolicyById.cs` | Get, id, request, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs` | roleId, HandleAsync, policyId, request, ct, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs` | Delete, ct, HandleAsync, id, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs` | roleId, ct, HandleAsync, request, policyId, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoleById.cs` | Get, HandleAsync, id, ct, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRoles.cs` | HandleAsync, request, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/GetRolesWithPolicies.cs` | ct, request, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/GetRolesWithPolicies/RoleWithPoliciesResponse.cs` | RoleWithPoliciesResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUser.cs` | ct, HandleAsync, externalId, Get, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetAuthUserByEmail.cs` | email, Get, HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/GetOrgMembership.cs` | HandleAsync, ct, externalId, Get, organizationId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs` | userId, HandleAsync, ct, Post |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/BillingUsageResponse.cs` | BillingUsageResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetBillingUsage.cs` | Get, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlanById.cs` | ct, HandleAsync, id, Get |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/GetPlans.cs` | Get, HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Billing/PlanResponse.cs` | PlanResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs` | ct, HandleAsync, Delete, id |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizationById.cs` | Get, ct, id, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/GetOrganizations.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/AcceptInvitation/AcceptInvitation.cs` | HandleAsync, invitationId, Accept, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/DeclineInvitation/DeclineInvitation.cs` | Decline, invitationId, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/ExtendInvitation/ExtendInvitation.cs` | Extend, invitationId, HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitationById.cs` | Get, HandleAsync, invitationId, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetInvitations.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/GetTenantInvitations.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/InvitationResponse.cs` | InvitationResponse |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs` | ct, invitationId, HandleAsync, Revoke |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Handler.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMemberById.cs` | HandleAsync, Get, ct, memberId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/GetMembers.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs` | Remove, memberId, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs` | Delete, tagId, ct, HttpDelete, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTagById.cs` | Get, tagId, HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/GetTags.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs` | HandleAsync, ct, teamId, Delete |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamById.cs` | ct, Get, HandleAsync, teamId |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeamMembers.cs` | teamId, HandleAsync, Get, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/GetTeams.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs` | HandleAsync, memberId, Remove, teamId, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs` | ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs` | Cancel, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/GetTenant.cs` | HandleAsync, ct, Get |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs` | HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/GetUserById.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs` | ct, invitationId, HandleAsync, Accept |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs` | invitationId, Decline, ct, HandleAsync |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/GetMyInvitations.cs` | Get, HandleAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Domain/Aggregates/Organization/Invitation/InvitationId.cs` | InvitationId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Common/PMTransactionDecorator.cs` | ct, HandleAsync, request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Handler.cs` | projectTemplateId, ct, Remove, cancellationToken, HandleAsync, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Handler.cs` | ct, cancellationToken, Delete, HandleAsync, id |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplateById/GetProjectTemplateById.Handler.cs` | GetById, ct, cancellationToken, HandleAsync, id |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/GetProjectTemplates/GetProjectTemplates.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Handler.cs` | cancellationToken, milestoneTemplateId, Remove, projectTemplateId, HandleAsync, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Handler.cs` | taskTemplateId, cancellationToken, Remove, HandleAsync, projectTemplateId, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Handler.cs` | cancellationToken, ct, HandleAsync, id, Archive |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjectById/GetProjectById.Handler.cs` | GetById, ct, id, cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/GetProjects/GetProjects.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/GetProjectMembers/GetProjectMembers.Handler.cs` | GetMembers, pageNumber, projectId, cancellationToken, HandleAsync, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Handler.cs` | ct, HandleAsync, memberId, RemoveMember, cancellationToken, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Handler.cs` | milestoneId, projectId, HandleAsync, ct, cancellationToken, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/GetProjectMilestones/GetProjectMilestones.Handler.cs` | pageNumber, ct, GetMilestones, HandleAsync, projectId, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/GetProjectTeams/GetProjectTeams.Handler.cs` | pageNumber, GetTeams, HandleAsync, projectId, cancellationToken, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/ProjectTeamResponse.cs` | ProjectTeamResponse |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs` | projectId, HandleAsync, RemoveTeam, teamId, cancellationToken, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/GetProjectWorkflow/GetProjectWorkflow.Handler.cs` | GetWorkflow, HandleAsync, projectId, cancellationToken, ct |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/GetProjectPriorities/GetProjectPriorities.Handler.cs` | projectId, cancellationToken, GetPriorities, pageNumber, ct, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/PriorityResponse.cs` | PriorityResponse |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs` | RemovePriority, projectId, priorityId, cancellationToken, HandleAsync, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/GetProjectStatuses/GetProjectStatuses.Handler.cs` | ct, pageSize, HandleAsync, cancellationToken, GetStatuses, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs` | projectId, cancellationToken, ct, statusId, RemoveStatus, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/StatusResponse.cs` | StatusResponse |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/GetTaskPriorities/GetTaskPriorities.Handler.cs` | cancellationToken, projectId, pageNumber, ct, GetTaskPriorities, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs` | RemoveTaskPriority, cancellationToken, ct, priorityId, HandleAsync, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs` | cancellationToken, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/GetTaskStatuses/GetTaskStatuses.Handler.cs` | projectId, pageNumber, HandleAsync, GetTaskStatuses, cancellationToken, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs` | statusId, RemoveTaskStatus, projectId, cancellationToken, HandleAsync, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs` | HandleAsync, cancellationToken |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/ProjectId.cs` | ProjectId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/ProjectTemplate/ProjectTemplateId.cs` | ProjectTemplateId |

## Entry Points

- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs::LinkUserExternalIdController.Post`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs::DeleteOrganizationController.Delete`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs::RevokeInvitationController.Revoke`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs::DeleteTagController.Delete`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs::DeleteTeamController.Delete`

## Connected Communities

- **WorkflowConfig/Status +12 dirs** (101 cross-edges)
- **Mirama.SharedKernel/Models +102 dirs** (92 cross-edges)
- **Abstractions/Persistence +3 dirs** (12 cross-edges)
- **Domain/Core +10 dirs** (9 cross-edges)
- **Abstractions/Persistence +2 dirs** (9 cross-edges)
- **Mirama.Modules.Identity/Mirama.Modules.Identity · IdentityDbContext** (6 cross-edges)
- **Modules/Mirama.Modules.Identity · MemberDto** (6 cross-edges)
- **Modules/Mirama.Modules.Identity · GetTeamByIdAsync** (6 cross-edges)
- **Infrastructure/Persistence +2 dirs** (6 cross-edges)
- **Mirama.Modules.PM/Mirama.Modules.PM · ProjectMilestone** (5 cross-edges)
- **Mirama.Modules.Identity/Mirama.Modules.Identity · MapAuthUserResponse** (5 cross-edges)
- **Mirama.Modules.PM/Mirama.Modules.PM · ProjectTemplate** (5 cross-edges)
- **Aggregates/Client +3 dirs** (5 cross-edges)
- **Aggregates/Role +3 dirs** (5 cross-edges)
- **Projects/Workflow** (4 cross-edges)
- **Common/Interfaces +10 dirs** (4 cross-edges)
- **Organization/Invitation** (4 cross-edges)
- **Mirama.Modules.PM/Mirama.Modules.PM · MilestoneTemplate** (3 cross-edges)
- **Domain/Core +4 dirs** (3 cross-edges)
- **Mirama.Modules.PM/Mirama.Modules.PM · CycleTemplate** (3 cross-edges)
- **Abstractions/Permissions +5 dirs** (3 cross-edges)
- **Mirama.Modules.PM/Mirama.Modules.PM · Add** (2 cross-edges)
- **Mirama.Modules.Identity/Mirama.Modules.Identity · Policy** (2 cross-edges)
- **Mirama.SharedKernel · LogRead** (2 cross-edges)
- **Mirama.Modules.Identity/Mirama.Modules.Identity · User** (2 cross-edges)
- **Tenant/Subscription** (2 cross-edges)
- **Organization/Tag** (2 cross-edges)
- **Organization/Member** (1 cross-edges)
- **Mirama.Modules.Identity/Mirama.Modules.Identity · PolicyStatement** (1 cross-edges)
- **Mirama.Modules.Identity/Mirama.Modules.Identity · TenantSettings** (1 cross-edges)
- **Aggregates/Project** (1 cross-edges)
- **Mirama.Modules.Clients/Mirama.Modules.Clients · ClientPortalInvitation** (1 cross-edges)
- **Aggregates/Tenant** (1 cross-edges)
- **Mirama.Modules.PM/Mirama.Modules.PM · TaskTemplate** (1 cross-edges)

## How to Explore

```
get_communities with id: "community-100"
smart_context with task: "understand V1/Billing +109 dirs", format: "gcx"
find_usages with id: "src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs::LinkUserExternalIdController.Post", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._

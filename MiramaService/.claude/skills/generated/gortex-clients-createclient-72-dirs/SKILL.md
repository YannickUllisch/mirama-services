---
name: gortex-clients-createclient-72-dirs
description: "Work in the Clients/CreateClient +72 dirs area — 140 symbols across 127 files (68% cohesion)"
---

# Clients/CreateClient +72 dirs

140 symbols | 127 files | 68% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/ICQRSRequests.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Types.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Types.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Types.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs`
- `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/AcceptInvitation/AcceptInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/DeclineInvitation/DeclineInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/ExtendInvitation/ExtendInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Types.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Types.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Types.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Common/Interfaces/ICQRSRequests.cs` | ICommand |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Clients/CreateClient/CreateClient.Types.cs` | CreateClientCommand |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Contacts/AddContact/AddContact.Types.cs` | AddContactCommand |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/AcceptInvitation/AcceptInvitation.Types.cs` | AcceptInvitationCommand |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Clients/Mirama.Modules.Clients/Application/Features/V1/Portal/InviteContact/InviteContact.Types.cs` | InviteContactCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/CreatePolicy/CreatePolicy.Types.cs` | CreatePolicyCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/DeletePolicy/DeletePolicy.cs` | DeletePolicyCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Policies/UpdatePolicy/UpdatePolicy.Types.cs` | UpdatePolicyCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/AttachPolicy/AttachPolicy.cs` | AttachPolicyCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/CreateRole/CreateRole.Types.cs` | CreateRoleCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DeleteRole/DeleteRole.cs` | DeleteRoleCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/DetachPolicy/DetachPolicy.cs` | DetachPolicyCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/AccessControl/Roles/UpdateRole/UpdateRole.Types.cs` | UpdateRoleCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/LinkUserExternalId/LinkUserExternalId.Types.cs` | LinkUserExternalIdCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Auth/SetupUser/SetupUser.Types.cs` | SetupUserCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/CreateOrganization/CreateOrganization.Types.cs` | CreateOrganizationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/DeleteOrganization/DeleteOrganization.cs` | request, DeleteOrganizationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/AcceptInvitation/AcceptInvitation.cs` | AcceptInvitationCommand, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/DeclineInvitation/DeclineInvitation.cs` | request, DeclineInvitationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/ExtendInvitation/ExtendInvitation.cs` | request, ExtendInvitationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/RevokeInvitation/RevokeInvitation.cs` | RevokeInvitationCommand, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Invitations/SendInvitation/SendInvitation.Types.cs` | SendInvitationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/RemoveMember/RemoveMember.cs` | request, RemoveMemberCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Members/UpdateMember/UpdateMember.Types.cs` | UpdateMemberCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/CreateTag/CreateTag.Types.cs` | CreateTagCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/DeleteTag/DeleteTag.Handler.cs.cs` | DeleteTagCommand, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Tags/UpdateTag/UpdateTag.Types.cs` | UpdateTagCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/AddTeamMember/AddTeamMember.Types.cs` | AddTeamMemberCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/CreateTeam/CreateTeam.Types.cs` | CreateTeamCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/DeleteTeam/DeleteTeam.cs` | DeleteTeamCommand, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/RemoveTeamMember/RemoveTeamMember.cs` | RemoveTeamMemberCommand, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/Teams/UpdateTeam/UpdateTeam.Types.cs` | UpdateTeamCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Organizations/UpdateOrganization/UpdateOrganization.Types.cs` | UpdateOrganizationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/CancelSubscription/CancelSubscription.cs` | CancelSubscriptionCommand, request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateSubscription/UpdateSubscription.cs` | request, UpdateSubscriptionCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Tenants/UpdateTenantSettings/UpdateTenantSettings.Types.cs` | UpdateTenantSettingsCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/AcceptMyInvitation/AcceptMyInvitation.cs` | request, AcceptMyInvitationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/Invitations/DeclineMyInvitation/DeclineMyInvitation.cs` | request, DeclineMyInvitationCommand |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Handler.cs` | request |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity/Application/Features/V1/Users/UpdateUser/UpdateUser.Types.cs` | UpdateUserCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CreateProjectTemplate/CreateProjectTemplate.Types.cs` | CreateProjectTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/AddCycleTemplate/AddCycleTemplate.Types.cs` | AddCycleTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/RemoveCycleTemplate/RemoveCycleTemplate.Types.cs` | RemoveCycleTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/CycleTemplates/UpdateCycleTemplate/UpdateCycleTemplate.Types.cs` | UpdateCycleTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/DeleteProjectTemplate/DeleteProjectTemplate.Types.cs` | DeleteProjectTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/AddMilestoneTemplate/AddMilestoneTemplate.Types.cs` | AddMilestoneTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/RemoveMilestoneTemplate/RemoveMilestoneTemplate.Types.cs` | RemoveMilestoneTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/MilestoneTemplates/UpdateMilestoneTemplate/UpdateMilestoneTemplate.Types.cs` | UpdateMilestoneTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/AddTaskTemplate/AddTaskTemplate.Types.cs` | AddTaskTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/TaskTemplates/RemoveTaskTemplate/RemoveTaskTemplate.Types.cs` | RemoveTaskTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/ProjectTemplates/UpdateProjectTemplate/UpdateProjectTemplate.Types.cs` | UpdateProjectTemplateCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ArchiveProject/ArchiveProject.Types.cs` | ArchiveProjectCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Types.cs` | CreateProjectCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/AddProjectMember/AddProjectMember.Types.cs` | AddProjectMemberCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/RemoveProjectMember/RemoveProjectMember.Types.cs` | RemoveProjectMemberCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Members/UpdateProjectMember/UpdateProjectMember.Types.cs` | UpdateProjectMemberCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/CreateProjectMilestone/CreateProjectMilestone.Types.cs` | CreateProjectMilestoneCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/DeleteProjectMilestone/DeleteProjectMilestone.Types.cs` | DeleteProjectMilestoneCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Milestones/UpdateProjectMilestone/UpdateProjectMilestone.Types.cs` | UpdateProjectMilestoneCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/AddProjectTeam/AddProjectTeam.Types.cs` | AddProjectTeamCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Teams/RemoveProjectTeam/RemoveProjectTeam.Types.cs` | RemoveProjectTeamCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Types.cs` | UpdateProjectCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/AddProjectPriority/AddProjectPriority.Types.cs` | AddProjectPriorityCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/RemoveProjectPriority/RemoveProjectPriority.Types.cs` | RemoveProjectPriorityCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Priorities/UpdateProjectPriority/UpdateProjectPriority.Types.cs` | UpdateProjectPriorityCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/AddProjectStatus/AddProjectStatus.Types.cs` | AddProjectStatusCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/RemoveProjectStatus/RemoveProjectStatus.Types.cs` | RemoveProjectStatusCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/Statuses/UpdateProjectStatus/UpdateProjectStatus.Types.cs` | UpdateProjectStatusCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/AddTaskPriority/AddTaskPriority.Types.cs` | AddTaskPriorityCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/RemoveTaskPriority/RemoveTaskPriority.Types.cs` | RemoveTaskPriorityCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskPriorities/UpdateTaskPriority/UpdateTaskPriority.Types.cs` | UpdateTaskPriorityCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/AddTaskStatus/AddTaskStatus.Types.cs` | AddTaskStatusCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/RemoveTaskStatus/RemoveTaskStatus.Types.cs` | RemoveTaskStatusCommand |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Handler.cs` | request |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/Workflow/TaskStatuses/UpdateTaskStatus/UpdateTaskStatus.Types.cs` | UpdateTaskStatusCommand |

## How to Explore

```
get_communities with id: "community-110"
smart_context with task: "understand Clients/CreateClient +72 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._

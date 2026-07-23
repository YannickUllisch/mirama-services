---
name: gortex-workflowconfig-status-12-dirs
description: "Work in the WorkflowConfig/Status +12 dirs area — 199 symbols across 23 files (75% cohesion)"
---

# WorkflowConfig/Status +12 dirs

199 symbols | 23 files | 75% cohesion

## When to Use

Use this skill when working on files in:
- `src/Mirama.SharedKernel/Abstractions/Domain/Core/Entity.cs`
- `src/Mirama.SharedKernel/Models/Permissions/Permissions.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Organizations/IMemberService.cs`
- `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Organizations/ITeamService.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ProjectResponse.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/Member/ProjectMember.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/Project.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/ProjectDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Priority/PriorityConfig.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Priority/PriorityConfigId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Priority/PriorityDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusCategory.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusConfig.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusConfigId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusDetails.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/WorkflowConfig.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/WorkflowConfigId.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/AggregateRoots/ProjectConfiguration.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/AggregateRoots/WorkflowConfigConfiguration.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/Entities/PriorityConfigConfiguration.cs`
- `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/Entities/StatusConfigConfiguration.cs`

## Key Files

| File | Symbols |
|------|---------|
| `src/Mirama.SharedKernel/Abstractions/Domain/Core/Entity.cs` | Equals, obj |
| `src/Mirama.SharedKernel/Models/Permissions/Permissions.cs` | Create, ResourcePattern, Group, Project, Read, ... |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Organizations/IMemberService.cs` | memberIds, GetMembersByIdsAsync, ct |
| `src/Modules/Mirama.Modules.Identity/Mirama.Modules.Identity.Contracts/Organizations/ITeamService.cs` | ct, teamIds, GetTeamsByIdsAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/CreateProject/CreateProject.Handler.cs` | cancellationToken, request, HandleAsync |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/ProjectResponse.cs` | memberLookup, teamLookup, ProjectTagResponse, project, ToResponse, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Application/Features/V1/Projects/UpdateProject/UpdateProject.Handler.cs` | ReconcileTags, desiredTagIds, project |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/Member/ProjectMember.cs` | roleId, SetDirectAssignment |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/Project.cs` | teamId, HasMember, Teams, Members, AddMilestone, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/Project/ProjectDetails.cs` | ProjectDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Priority/PriorityConfig.cs` | PriorityConfig.<init>, level, SetLevel, Name, PriorityConfig.<init>, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Priority/PriorityConfigId.cs` | PriorityConfigId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Priority/PriorityDetails.cs` | PriorityDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusCategory.cs` | StatusCategory, NotStarted, Done, Cancelled, Active |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusConfig.cs` | Update, IsTerminal, StatusConfig, details, position, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusConfigId.cs` | StatusConfigId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/Status/StatusDetails.cs` | StatusDetails |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/WorkflowConfig.cs` | id, CreateWithDefaults, details, id, SetDefaultPriority, ... |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Domain/Aggregates/WorkflowConfig/WorkflowConfigId.cs` | WorkflowConfigId |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/AggregateRoots/ProjectConfiguration.cs` | builder, ProjectConfiguration, Configure |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/AggregateRoots/WorkflowConfigConfiguration.cs` | WorkflowConfigConfiguration, Configure, builder |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/Entities/PriorityConfigConfiguration.cs` | builder, PriorityConfigConfiguration, Configure |
| `src/Modules/Mirama.Modules.PM/Mirama.Modules.PM/Infrastructure/Persistence/Configurations/Entities/StatusConfigConfiguration.cs` | StatusConfigConfiguration, Configure, builder |

## Connected Communities

- **Mirama.SharedKernel/Models +102 dirs** (1 cross-edges)
- **Modules/Mirama.Modules.Identity · MemberDto** (1 cross-edges)
- **Modules/Mirama.Modules.Identity · GetTeamByIdAsync** (1 cross-edges)

## How to Explore

```
get_communities with id: "community-143"
smart_context with task: "understand WorkflowConfig/Status +12 dirs", format: "gcx"
```

_`format: "gcx"` returns the [GCX1 compact wire format](../../docs/wire-format.md) — round-trippable, ~27% fewer tokens than JSON. Drop it for JSON output; agents using `@gortex/wire` or the Go `github.com/gortexhq/gcx-go` package decode either._

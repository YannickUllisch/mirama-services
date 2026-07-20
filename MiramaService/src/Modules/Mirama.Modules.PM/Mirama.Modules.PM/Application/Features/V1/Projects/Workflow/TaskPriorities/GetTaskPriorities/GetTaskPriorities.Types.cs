using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskPriorities.GetTaskPriorities;

public sealed record GetTaskPrioritiesQuery(
    Guid ProjectId,
    int? PageNumber,
    int? PageSize) : IQuery<ErrorOr<PaginatedList<PriorityResponse>>>;

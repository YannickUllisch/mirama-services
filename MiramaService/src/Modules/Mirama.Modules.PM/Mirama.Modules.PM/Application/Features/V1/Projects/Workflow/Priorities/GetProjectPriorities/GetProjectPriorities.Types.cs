using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Priorities.GetProjectPriorities;

public sealed record GetProjectPrioritiesQuery(
    Guid ProjectId,
    int? PageNumber,
    int? PageSize) : IQuery<ErrorOr<PaginatedList<PriorityResponse>>>;

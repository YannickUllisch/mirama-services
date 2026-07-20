using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.TaskStatuses.GetTaskStatuses;

public sealed record GetTaskStatusesQuery(
    Guid ProjectId,
    int? PageNumber,
    int? PageSize) : IQuery<ErrorOr<PaginatedList<StatusResponse>>>;

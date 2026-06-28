using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Statuses.GetProjectStatuses;

public sealed record GetProjectStatusesQuery(
    Guid ProjectId,
    int? PageNumber,
    int? PageSize) : IQuery<ErrorOr<PaginatedList<StatusResponse>>>;

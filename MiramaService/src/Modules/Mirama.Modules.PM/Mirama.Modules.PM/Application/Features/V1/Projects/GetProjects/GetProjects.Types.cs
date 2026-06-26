using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjects;

public sealed record GetProjectsQuery(int? PageNumber, int? PageSize) : IQuery<ErrorOr<PaginatedList<ProjectResponse>>>;

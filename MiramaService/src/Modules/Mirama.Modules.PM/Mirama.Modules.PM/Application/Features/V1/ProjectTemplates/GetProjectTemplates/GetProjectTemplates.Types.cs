using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.GetProjectTemplates;

public sealed record GetProjectTemplatesQuery(int? PageNumber, int? PageSize)
    : IQuery<ErrorOr<PaginatedList<ProjectTemplateResponse>>>;

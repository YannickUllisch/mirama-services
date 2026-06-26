using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.GetProjectTemplateById;

public sealed record GetProjectTemplateByIdQuery(Guid Id) : IQuery<ErrorOr<ProjectTemplateResponse>>;

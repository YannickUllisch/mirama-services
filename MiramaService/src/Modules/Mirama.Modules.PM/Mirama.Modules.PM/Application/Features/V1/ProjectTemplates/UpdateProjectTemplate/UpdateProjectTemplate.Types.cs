using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.UpdateProjectTemplate;

public sealed record UpdateProjectTemplateCommand(
    Guid Id,
    string Name,
    string? Description,
    string? Category) : ICommand<ErrorOr<ProjectTemplateResponse>>;

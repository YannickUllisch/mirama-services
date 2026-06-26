using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.ProjectTemplates.CreateProjectTemplate;

public sealed record CreateProjectTemplateCommand(
    string Name,
    string? Description,
    string? Category) : ICommand<ErrorOr<ProjectTemplateResponse>>;


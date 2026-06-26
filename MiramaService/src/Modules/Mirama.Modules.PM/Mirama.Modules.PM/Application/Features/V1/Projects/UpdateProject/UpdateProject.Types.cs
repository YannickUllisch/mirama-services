using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.UpdateProject;

public sealed record UpdateProjectCommand(
    Guid Id,
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    Guid StatusId,
    Guid PriorityId,
    int Budget,
    List<Guid> TagIds) : ICommand<ErrorOr<ProjectResponse>>;

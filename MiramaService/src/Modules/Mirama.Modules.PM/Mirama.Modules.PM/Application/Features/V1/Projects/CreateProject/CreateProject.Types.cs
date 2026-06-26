using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.CreateProject;

public sealed record CreateProjectCommand(
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime? EndDate,
    Guid StatusId,
    Guid PriorityId,
    int Budget,
    List<Guid> TagIds,
    List<CreateProjectMemberInput> Members,
    List<Guid> TeamIds,
    List<CreateProjectMilestoneInput> Milestones) : ICommand<ErrorOr<ProjectResponse>>;

public sealed record CreateProjectMemberInput(Guid MemberId, Guid RoleId);

public sealed record CreateProjectMilestoneInput(
    string Title,
    DateTime DueDate,
    string? Description = null,
    string? Color = null);


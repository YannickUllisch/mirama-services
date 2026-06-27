using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.UpdateProjectMember;

public sealed record UpdateProjectMemberCommand(Guid ProjectId, Guid MemberId, Guid RoleId)
    : ICommand<ErrorOr<ProjectMemberResponse>>;

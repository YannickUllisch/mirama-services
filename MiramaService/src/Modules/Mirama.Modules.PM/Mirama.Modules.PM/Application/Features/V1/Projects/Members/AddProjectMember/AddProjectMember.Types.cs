using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Members;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.AddProjectMember;

public sealed record AddProjectMemberCommand(Guid ProjectId, Guid MemberId, Guid RoleId)
    : ICommand<ErrorOr<ProjectMemberResponse>>;

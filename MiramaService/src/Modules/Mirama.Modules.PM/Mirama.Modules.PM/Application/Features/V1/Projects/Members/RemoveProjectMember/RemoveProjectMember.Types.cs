using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.RemoveProjectMember;

public sealed record RemoveProjectMemberCommand(Guid ProjectId, Guid MemberId)
    : ICommand<ErrorOr<Deleted>>;

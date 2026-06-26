using ErrorOr;
using Mirama.Modules.PM.Application.Features.V1.Projects.Members;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Members.GetProjectMembers;

public sealed record GetProjectMembersQuery(Guid ProjectId, int? PageNumber, int? PageSize)
    : IQuery<ErrorOr<PaginatedList<ProjectMemberResponse>>>;

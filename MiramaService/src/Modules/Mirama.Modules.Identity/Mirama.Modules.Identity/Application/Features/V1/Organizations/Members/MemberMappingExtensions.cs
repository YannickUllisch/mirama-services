using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members;

internal static class MemberMapper
{
    internal static MemberResponse MapResponse(this Member member) => new()
    {
        Id = member.Id.Value,
        Name = member.Name,
        Email = member.Email,
        UserId = member.UserId?.Value,
        IamRoleId = member.IamRoleId.Value,
        OrganizationId = member.OrganizationId
    };
}

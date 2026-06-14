
using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

internal static class AuthOrgMembershipResponseMapper
{
    internal static AuthOrgMembershipResponse MapOrgMembershipResponse(this Organization org, Member member)
    {
        return new()
        {
            OrganizationId = org.Id.Value,
            UserId = member.UserId.Value,
            TenantId = org.TenantId,
            MemberId = member.Id.Value,
            IamRoleId = member.IamRoleId.Value,
        };
    }
}

public sealed record AuthOrgMembershipResponse
{
    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    [JsonPropertyName("userId")]
    public Guid UserId { get; init; }

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    [JsonPropertyName("memberId")]
    public Guid MemberId { get; init; }

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }
}


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
            Id = org.Id.Value,
            TenantId = org.TenantId,
            MemberId = member.Id.Value,
            IamRoleId = member.IamRoleId.Value,
        };
    }
}

public sealed record AuthOrgMembershipResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("tenantId")]
    public Guid TenantId { get; init; }

    [JsonPropertyName("memberId")]
    public Guid MemberId { get; init; }

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }
}

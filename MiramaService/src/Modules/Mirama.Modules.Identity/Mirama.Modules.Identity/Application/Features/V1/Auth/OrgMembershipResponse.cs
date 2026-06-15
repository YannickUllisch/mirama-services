using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth;

internal static class AuthOrgMembershipResponseMapper
{
    internal static AuthOrgMembershipResponse MapOrgMembershipResponse(this Organization org, Member member, TenantRole tenantRole)
    {
        return new()
        {
            OrganizationId = org.Id.Value,
            UserId = member.UserId.Value,
            TenantId = org.TenantId,
            TenantRole = Enum.GetName(tenantRole)!,
            MemberId = member.Id.Value,
            IamRoleIds = member.IamRoleIds.ConvertAll(r => r.Value),
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

    [JsonPropertyName("tenantRole")]
    public string TenantRole { get; init; } = string.Empty;

    [JsonPropertyName("memberId")]
    public Guid MemberId { get; init; }

    [JsonPropertyName("iamRoleIds")]
    public List<Guid> IamRoleIds { get; init; } = [];
}

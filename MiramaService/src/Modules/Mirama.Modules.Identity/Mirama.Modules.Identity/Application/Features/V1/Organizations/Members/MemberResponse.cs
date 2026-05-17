using System.Text.Json.Serialization;
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

public sealed record MemberResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("userId")]
    public Guid? UserId { get; init; }

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }
}

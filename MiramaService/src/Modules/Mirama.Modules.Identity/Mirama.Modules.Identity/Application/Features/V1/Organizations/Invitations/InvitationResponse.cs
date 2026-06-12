using System.Text.Json.Serialization;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations;

internal static class InvitationMapper
{
    internal static InvitationResponse MapResponse(this Invitation invitation, string organizationName = "") => new()
    {
        Id = invitation.Id.Value,
        Email = invitation.Email,
        Name = invitation.Name,
        InviterId = invitation.InviterId,
        IamRoleId = invitation.IamRoleId.Value,
        Status = Enum.GetName(invitation.Status)!,
        ExpiresAt = invitation.ExpiresAt,
        OrganizationId = invitation.OrganizationId,
        OrganizationName = organizationName
    };
}

public sealed record InvitationResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("inviterId")]
    public Guid InviterId { get; init; }

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }

    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; init; }

    [JsonPropertyName("organizationId")]
    public Guid OrganizationId { get; init; }

    [JsonPropertyName("organizationName")]
    public string OrganizationName { get; init; } = string.Empty;
}

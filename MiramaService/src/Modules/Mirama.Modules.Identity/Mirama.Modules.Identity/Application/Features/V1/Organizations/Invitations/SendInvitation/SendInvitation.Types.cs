using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.SendInvitation;

public sealed record SendInvitationCommand : ICommand<ErrorOr<InvitationResponse>>
{
    [JsonPropertyName("email")]
    public string Email { get; init; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }
}

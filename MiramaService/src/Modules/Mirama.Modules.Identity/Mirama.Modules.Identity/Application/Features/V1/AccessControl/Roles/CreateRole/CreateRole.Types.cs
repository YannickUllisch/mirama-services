using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.CreateRole;

public sealed record CreateRoleCommand : ICommand<ErrorOr<RoleResponse>>
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; init; }

    [JsonPropertyName("scope")]
    public string Scope { get; init; } = string.Empty;
}

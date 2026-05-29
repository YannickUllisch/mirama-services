using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.UpdateTeam;

public sealed record UpdateTeamCommand : ICommand<ErrorOr<TeamResponse>>
{
    [JsonIgnore]
    public Guid TeamId { get; init; }

    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

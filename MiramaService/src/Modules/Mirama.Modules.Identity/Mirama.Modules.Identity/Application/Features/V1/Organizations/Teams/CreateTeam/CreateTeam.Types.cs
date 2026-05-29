using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.CreateTeam;

public sealed record CreateTeamCommand : ICommand<ErrorOr<TeamResponse>>
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;
}

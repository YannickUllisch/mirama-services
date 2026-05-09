using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.AddTeamMember;

public sealed record AddTeamMemberCommand : ICommand<ErrorOr<TeamResponse>>
{
    [JsonIgnore]
    public Guid TeamId { get; init; }

    [JsonPropertyName("memberId")]
    public Guid MemberId { get; init; }
}

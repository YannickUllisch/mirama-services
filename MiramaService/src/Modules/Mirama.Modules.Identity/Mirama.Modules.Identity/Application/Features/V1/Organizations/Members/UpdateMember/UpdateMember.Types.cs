using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members.UpdateMember;

public sealed record UpdateMemberCommand : ICommand<ErrorOr<MemberResponse>>
{
    [JsonIgnore]
    public Guid MemberId { get; init; }

    [JsonPropertyName("iamRoleId")]
    public Guid IamRoleId { get; init; }
}

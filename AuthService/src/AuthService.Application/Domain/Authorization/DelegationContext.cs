
namespace AuthService.Application.Domain.Authorization;

public sealed record DelegationContext
{
    public string DelegatedSubjectId { get; init; } = string.Empty;
    public string DelegatorId { get; init; } = string.Empty;
}
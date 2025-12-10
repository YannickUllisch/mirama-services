
namespace AccountService.Application.Domain.User.ValueObjects;

public sealed record ContactInfo(string ContactEmail, string ContactPhoneNumber)
{
    public string ContactEmail { get; init; } = ContactEmail.Trim();
    public string ContactPhoneNumber { get; init; } = ContactPhoneNumber.Trim();
}

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.Contact;

public record ContactDetails(
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? JobTitle,
    bool IsPrimary);

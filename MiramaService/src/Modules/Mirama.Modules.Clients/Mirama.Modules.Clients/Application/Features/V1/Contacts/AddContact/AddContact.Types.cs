using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Application.Features.V1.Contacts.AddContact;

public sealed record AddContactCommand(
    Guid ClientId,
    string FirstName,
    string LastName,
    string Email,
    string? Phone,
    string? JobTitle,
    bool IsPrimary) : ICommand<ErrorOr<ContactResponse>>;

public sealed record ContactResponse(
    Guid ContactId,
    Guid ClientId,
    string FullName,
    string Email,
    string? Phone,
    string? JobTitle,
    bool IsPrimary);

using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client.Contact;

public class Contact : OrganizationEntity<ContactId>
{
    public ClientId ClientId { get; init; } = null!;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Phone { get; private set; }
    public string? JobTitle { get; private set; }
    public bool IsPrimary { get; private set; }

    private Contact() { }

    private Contact(ContactDetails details)
    {
        this.FirstName = details.FirstName.Trim();
        this.LastName = details.LastName.Trim();
        this.Email = details.Email.Trim().ToLowerInvariant();
        this.Phone = details.Phone;
        this.JobTitle = details.JobTitle;
        this.IsPrimary = details.IsPrimary;
    }

    public static Contact Create(ContactDetails details)
    {
        return new Contact(details) { Id = new ContactId(Guid.NewGuid()) };
    }

    public void Update(ContactDetails details)
    {
        this.FirstName = details.FirstName.Trim();
        this.LastName = details.LastName.Trim();
        this.Email = details.Email.Trim().ToLowerInvariant();
        this.Phone = details.Phone;
        this.JobTitle = details.JobTitle;
        this.IsPrimary = details.IsPrimary;
    }

    public string FullName => $"{FirstName} {LastName}";
}


using AccountService.Domain.Common;
using AccountService.Domain.User.ValueObjects;

namespace AccountService.Domain.User;

public class User : AuditableEntity
{
    public UserId Id { get; private set; } = default!;

    public string Name { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public string? Image { get; private set; }

    public GlobalRole Role { get; private set; }

    public ContactInfo Contact { get; private set; } = default!;

    private User(string name, string email, GlobalRole role, ContactInfo contactInfo, string? image)
    {
        Name = name;
        Email = email;
        Role = role;
        Contact = contactInfo;
        Image = image;
    }

    public static User Create(string name, string email, GlobalRole role, ContactInfo contactInfo, string? image)
    {
        return new User(name, email, role, contactInfo, image);
    }

}
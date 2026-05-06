using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.User;

public class User : AggregateRoot<UserId>
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Image { get; private set; }
    public DateTime? EmailVerified { get; private set; }
    public TenantRole Role { get; private set; }

    private User(string name, string email, TenantRole role, string? image)
    {
        Name = name;
        Email = email;
        Role = role;
        Image = image;
    }

    private User() { }

    public static User Create(string name, string email, TenantRole role = TenantRole.User, string? image = null)
    {
        return new User(name, email, role, image);
    }

    public void Update(string name, string email, TenantRole role, string? image)
    {
        Name = name;
        Email = email;
        Role = role;
        Image = image;
    }

    public void VerifyEmail()
    {
        EmailVerified = DateTime.UtcNow;
    }
}

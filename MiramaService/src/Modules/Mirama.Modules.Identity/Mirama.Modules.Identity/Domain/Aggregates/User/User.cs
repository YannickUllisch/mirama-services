using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.User;

public class User : AggregateRoot<UserId>
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Image { get; private set; }
    public DateTime? EmailVerified { get; private set; }
    public TenantRole Role { get; private set; }

    private User(UserDetails details)
    {
        Name = details.Name.Trim();
        Email = details.Email.Trim();
        Role = details.Role;
        Image = details.Image;
    }

    private User() { }

    public static User Create(UserDetails details)
    {
        return new User(details) { Id = new UserId(Guid.NewGuid()) };
    }

    public void Update(UserDetails details)
    {
        Name = details.Name.Trim();
        Email = details.Email.Trim();
        Role = details.Role;
        Image = details.Image;
    }

    public void VerifyEmail()
    {
        EmailVerified = DateTime.UtcNow;
    }
}

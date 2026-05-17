using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.User;

public class User : AggregateRoot<UserId>
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string? Image { get; private set; }
    public DateTime? EmailVerified { get; private set; }
    public TenantRole Role { get; private set; }

    public bool IsOnboarded { get; private set; } = false;

    private User(UserDetails details)
    {
        this.Name = details.Name.Trim();
        this.Email = details.Email.Trim();
        this.Role = details.Role;
        this.Image = details.Image;
        this.IsOnboarded = false;
    }

    private User() { }

    public static User Create(UserDetails details)
    {
        return new User(details) { Id = new UserId(Guid.NewGuid()) };
    }

    public static User CreateWithId(UserDetails details, Guid externalId)
    {
        return new User(details) { Id = new UserId(externalId) };
    }

    public void Update(UserDetails details)
    {
        this.Name = details.Name.Trim();
        this.Email = details.Email.Trim();
        this.Role = details.Role;
        this.Image = details.Image;
    }

    public void VerifyEmail()
    {
        this.EmailVerified = DateTime.UtcNow;
    }

    public void HasBeenOnboarded()
    {
        this.IsOnboarded = true;
    }
}

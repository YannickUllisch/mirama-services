
namespace AuthService.Application.Common.Interfaces;

public interface IAuthorizationRule
{
    bool IsApplicable(IAuthorizationContext context);
    void Apply(IAuthorizationContext context);
}

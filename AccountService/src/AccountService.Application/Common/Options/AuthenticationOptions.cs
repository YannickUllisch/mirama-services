
namespace AccountService.Application.Common.Options;

public class AuthenticationOptions
{
    public const string Key = "Auth";

    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
}
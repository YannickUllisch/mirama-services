
namespace Mirama.SharedKernel.Infrastructure.Options;

public class AuthenticationOptions
{
    public const string Key = "Auth";

    public string Audience { get; set; } = string.Empty;
    public string Authority { get; set; } = string.Empty;
    public string NextAuthSecret { get; set; } = string.Empty;
}
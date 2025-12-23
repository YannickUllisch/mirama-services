
namespace AuthService.Server.Common.Options;

public class GoogleOptions
{
    public const string Google = "Google";

    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
}
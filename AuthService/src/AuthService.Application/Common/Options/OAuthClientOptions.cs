
namespace AuthService.Application.Common.Options;

public class OAuthClientOptions
{
    public const string Clients = "Clients";

    public string MiramaFrontendClientId { get; set; } = string.Empty;
    public string MiramaFrontendClientSecret { get; set; } = string.Empty;
    public string MiramaFrontendRedirectUri { get; set; } = string.Empty;

    public string PostmanClientId { get; set; } = string.Empty;
    public string PostmanClientSecret { get; set; } = string.Empty;
    public string PostmanRedirectUri { get; set; } = string.Empty;
}
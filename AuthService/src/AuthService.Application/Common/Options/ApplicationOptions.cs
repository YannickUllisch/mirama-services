
namespace AuthService.Application.Common.Options;

public class ApplicationOptions
{
    public const string Application = "Application";

    public string SelfUrl { get; set; } = string.Empty;
    public string DatabaseConnection { get; set; } = string.Empty;
}
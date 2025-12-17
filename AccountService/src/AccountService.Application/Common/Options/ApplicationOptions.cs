
namespace AccountService.Application.Common.Options;

public class ApplicationOptions
{
    public const string Application = "Application";

    public string CorsOrigins { get; set; } = string.Empty;
}
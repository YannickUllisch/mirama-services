
namespace Mirama.Modules.Identity.Infrastructure.Common.Options;

public class ApplicationOptions
{
    public const string Key = "Application";

    public string CorsOrigins { get; set; } = string.Empty;
}
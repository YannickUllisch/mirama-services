
namespace Mirama.SharedKernel.Infrastructure.Options;

public class ApplicationOptions
{
    public const string Key = "Application";

    public string CorsOrigins { get; set; } = string.Empty;
}

namespace Mirama.Modules.Identity.Infrastructure.Common.Options;

public class InfrastructureOptions
{
    public const string Key = "Infrastructure";

    public string DatabaseConnection { get; set; } = string.Empty;
}
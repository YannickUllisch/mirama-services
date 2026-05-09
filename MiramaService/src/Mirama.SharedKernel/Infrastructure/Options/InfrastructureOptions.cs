
namespace Mirama.SharedKernel.Infrastructure.Options;

public class InfrastructureOptions
{
    public const string Key = "Infrastructure";

    public string DatabaseConnection { get; set; } = string.Empty;
}
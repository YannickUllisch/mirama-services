namespace Mirama.Modules.Projects.Infrastructure.Common.Options;

public class InfrastructureOptions
{
    public const string Key = "Projects:Infrastructure";

    public string DatabaseConnection { get; set; } = string.Empty;
}

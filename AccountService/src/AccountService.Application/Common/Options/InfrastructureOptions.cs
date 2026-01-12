
namespace AccountService.Application.Common.Options;

public class InfrastructureOptions
{
    public const string Infrastructure = "Infrastructure";

    public string DatabaseConnection { get; set; } = string.Empty;
}
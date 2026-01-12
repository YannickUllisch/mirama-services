
namespace ProjectService.Api.Contracts.Requests;

public record CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
}
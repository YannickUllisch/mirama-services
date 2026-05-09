using System.Text.Json.Serialization;
using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.CreateOrganization;

public sealed record CreateOrganizationCommand : ICommand<ErrorOr<OrganizationResponse>>
{
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    [JsonPropertyName("street")]
    public string Street { get; init; } = string.Empty;

    [JsonPropertyName("city")]
    public string City { get; init; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;

    [JsonPropertyName("zipCode")]
    public string ZipCode { get; init; } = string.Empty;

    [JsonPropertyName("logo")]
    public string? Logo { get; init; }
}


namespace AccountService.Domain.Organization.ValueObjects;

public sealed record Address(string Street, string City, string Country, string ZipCode)
{
    public string Street { get; init; } = Street.Trim();
    public string City { get; init; } = City.Trim();
    public string Country { get; init; } = Country.Trim();
    public string ZipCode { get; init; } = ZipCode.Trim();
    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Street)
            && !string.IsNullOrWhiteSpace(City)
            && !string.IsNullOrWhiteSpace(Country)
            && !string.IsNullOrWhiteSpace(ZipCode);
    }
}
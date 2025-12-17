
using ErrorOr;

namespace AccountService.Application.Domain.Organization.ValueObjects;

public sealed record Address
{
    public string Street { get; }
    public string City { get; }
    public string Country { get; }
    public string ZipCode { get; }

    private Address(string street, string city, string country, string zipCode)
    {
        Street = street;
        City = city;
        Country = country;
        ZipCode = zipCode;
    }

    public static ErrorOr<Address> Create(string street, string city, string country, string zipCode)
    {
        var trimmedStreet = street.Trim();
        var trimmedCity = city.Trim();
        var trimmedCountry = country.Trim();
        var trimmedZip = zipCode.Trim();

        var validationResult = Validate(trimmedStreet, trimmedCity, trimmedCountry, trimmedZip);
        if (validationResult.IsError)
        {
            return validationResult.Errors;
        }

        return new Address(trimmedStreet, trimmedCity, trimmedCountry, trimmedZip);

    }

    // <summary>
    private static ErrorOr<Success> Validate(string street, string city, string country, string zipCode)
    {
        List<Error> errors = [];

        if (string.IsNullOrWhiteSpace(street))
            errors.Add(Error.Validation(code: "Address.Street", description: "Street is required"));

        if (string.IsNullOrWhiteSpace(city))
            errors.Add(Error.Validation(code: "Address.City", description: "City is required"));

        if (string.IsNullOrWhiteSpace(country))
            errors.Add(Error.Validation(code: "Address.Country", description: "Country is required"));

        if (string.IsNullOrWhiteSpace(zipCode))
            errors.Add(Error.Validation(code: "Address.ZipCode", description: "Zip code is required"));

        if (errors.Count > 0)
            return errors;

        return Result.Success;
    }
}

using AccountService.Application.Domain.Abstractions.Core;
using AccountService.Application.Domain.Aggregates.User;
using ErrorOr;

namespace AccountService.Application.Domain.Aggregates.Account;

/// <summary>
/// An Account represents one login method connected to a User. A User can have many accounts that all link to the 
/// same user object. This ensures scalability for user login since multiple providers can be used to access the same account
/// </summary> 
public class Account : AggregateRoot<AccountId>
{
    public UserId UserId { get; init; } = default!;
    public AuthProvider Provider { get; init; }
    public Guid ProviderUserId { get; init; }
    
    private Account(UserId userId, Guid providerUserId, AuthProvider provider)
    {
        UserId = userId;
        ProviderUserId = providerUserId;
        Provider = provider;
    }

    private Account () {}

    public static ErrorOr<Account> Create(Guid userId, Guid providerUserId, string provider)
    {
        var validatedProvider = ParseAuthProvider(provider.ToLower());

        if (validatedProvider.IsError)
        {
            return validatedProvider.Errors;
        }

        return new Account(new UserId(userId), providerUserId, validatedProvider.Value);
    }

    private static ErrorOr<AuthProvider> ParseAuthProvider(string provider)
    {
        return provider switch
        {
            AuthProviderNames.Google => AuthProvider.Google,
            AuthProviderNames.Microsoft => AuthProvider.Google,
            _ => Error.NotFound("Invalid provider")
        };
    }
}
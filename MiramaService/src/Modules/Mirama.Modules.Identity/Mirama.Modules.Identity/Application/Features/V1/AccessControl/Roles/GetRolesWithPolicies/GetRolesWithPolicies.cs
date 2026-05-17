using System.Text.Json.Serialization;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Roles.GetRolesWithPolicies;

public class GetRolesWithPoliciesController : TenantControllerBase
{
    [HttpGet("roles/{scope}/with-policies")]
    public async Task<ActionResult<PaginatedList<RoleWithPoliciesResponse>>> Get(
        [FromRoute] string scope,
        [FromQuery] GetRolesWithPoliciesQuery query)
    {
        var res = await this.Dispatcher.Send(query with { Scope = scope });
        return res.Match(Ok, Problem);
    }
}

public sealed record GetRolesWithPoliciesQuery : IQuery<ErrorOr<PaginatedList<RoleWithPoliciesResponse>>>
{
    public string Scope { get; init; } = string.Empty;

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; init; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; init; } = 20;
}

internal class GetRolesWithPoliciesQueryValidator : AbstractValidator<GetRolesWithPoliciesQuery>
{
    public GetRolesWithPoliciesQueryValidator()
    {
        RuleFor(q => q.Scope)
            .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
            .WithMessage("Provided scope is not supported. Valid values: " + string.Join(", ", Enum.GetNames<AccessScope>()));

        RuleFor(q => q.PageSize).InclusiveBetween(1, 50);
        RuleFor(q => q.PageNumber).GreaterThanOrEqualTo(1);
    }
}

internal class GetRolesWithPoliciesQueryHandler(
    IIdentityQueryRepository<Role, RoleId> roleRepository,
    IIdentityQueryRepository<Policy, PolicyId> policyRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetRolesWithPoliciesQuery, ErrorOr<PaginatedList<RoleWithPoliciesResponse>>>
{
    public async Task<ErrorOr<PaginatedList<RoleWithPoliciesResponse>>> HandleAsync(GetRolesWithPoliciesQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope);

        var rolesQuery = roleRepository.Query()
            .Where(r => r.TenantId == null || r.TenantId == tenantId)
            .Where(r => r.Scope == scope)
            .OrderBy(r => r.TenantId != null)
            .ThenBy(r => r.Name);

        var total = await rolesQuery.CountAsync(ct);
        var roles = await rolesQuery
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(ct);

        var allPolicyGuids = roles.SelectMany(r => r.Policies).Select(p => p.Value).Distinct().ToList();

        var policies = await policyRepository.Query()
            .Where(p => allPolicyGuids.Contains(p.Id.Value))
            .Include(x => x.Statements)
            .ToListAsync(ct);

        var policyLookup = policies.ToDictionary(p => p.Id) as IReadOnlyDictionary<PolicyId, Policy>;

        var items = roles.ConvertAll(r => r.MapResponse(policyLookup));

        return new PaginatedList<RoleWithPoliciesResponse>(items, total, request.PageNumber, request.PageSize);
    }
}

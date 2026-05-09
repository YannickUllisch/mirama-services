using System.Text.Json.Serialization;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies;

public class GetPoliciesController : TenantControllerBase
{
    [HttpGet("policies")]
    public async Task<ActionResult<PaginatedList<PolicyResponse>>> Get([FromQuery] GetPoliciesQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetPoliciesQuery : IQuery<ErrorOr<PaginatedList<PolicyResponse>>>
{
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; init; }

    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; init; }

    [JsonPropertyName("scope")]
    public string? Scope { get; init; }
}

internal class GetPoliciesQueryValidator : AbstractValidator<GetPoliciesQuery>
{
    public GetPoliciesQueryValidator()
    {
        RuleFor(q => q.PageSize).LessThanOrEqualTo(50);

        When(q => q.Scope is not null, () =>
            RuleFor(q => q.Scope!)
                .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
                .WithMessage("Scope must be 'Organization' or 'Project'"));
    }
}

internal class GetPoliciesQueryHandler(
    IIdentityQueryRepository<Policy, PolicyId> policyRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetPoliciesQuery, ErrorOr<PaginatedList<PolicyResponse>>>
{
    public async Task<ErrorOr<PaginatedList<PolicyResponse>>> HandleAsync(GetPoliciesQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        IQueryable<Policy> query = policyRepository.Query()
            .Include(p => p.Statements)
            .Where(p => p.TenantId == null || p.TenantId == tenantId)
            .OrderBy(p => p.TenantId != null)
            .ThenBy(p => p.Name);

        if (request.Scope is not null && Enum.TryParse<AccessScope>(request.Scope, ignoreCase: true, out var scope))
            query = query.Where(p => p.Scope == scope);

        if (request.PageNumber is not null && request.PageSize is not null)
        {
            var total = await query.CountAsync(ct);
            var page = await query
                .Skip((request.PageNumber.Value - 1) * request.PageSize.Value)
                .Take(request.PageSize.Value)
                .ToListAsync(ct);

            return new PaginatedList<PolicyResponse>(page.ConvertAll(p => p.MapResponse()), total, request.PageNumber.Value, request.PageSize.Value);
        }

        var policies = await query.ToListAsync(ct);
        return new PaginatedList<PolicyResponse>(policies.ConvertAll(p => p.MapResponse()), policies.Count, 1, policies.Count > 0 ? policies.Count : 1);
    }
}

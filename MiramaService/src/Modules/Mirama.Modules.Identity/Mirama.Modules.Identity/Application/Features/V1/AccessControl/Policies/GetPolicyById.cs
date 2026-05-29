using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies;

public class GetPolicyByIdController : TenantControllerBase
{
    [HttpGet("policies/{id:guid}")]
    public async Task<ActionResult<PolicyResponse>> Get([FromRoute] Guid id)
    {
        var res = await this.Dispatcher.Send(new GetPolicyByIdQuery(id));
        return res.Match(Ok, Problem);
    }
}

public sealed record GetPolicyByIdQuery(Guid Id) : IQuery<ErrorOr<PolicyResponse>>;

internal class GetPolicyByIdQueryHandler(
    IIdentityQueryRepository<Policy, PolicyId> policyRepository,
    IRequestContextProvider contextProvider) : IRequestHandler<GetPolicyByIdQuery, ErrorOr<PolicyResponse>>
{
    public async Task<ErrorOr<PolicyResponse>> HandleAsync(GetPolicyByIdQuery request, CancellationToken ct)
    {
        var tenantId = contextProvider.TenantId;

        var policy = await policyRepository.Query()
            .Include(p => p.Statements)
            .Where(p => p.TenantId == null || p.TenantId == tenantId)
            .FirstOrDefaultAsync(p => p.Id.Value == request.Id, ct);

        if (policy is null)
            return Error.NotFound("Policy.NotFound", "Policy not found.");

        return policy.MapResponse();
    }
}

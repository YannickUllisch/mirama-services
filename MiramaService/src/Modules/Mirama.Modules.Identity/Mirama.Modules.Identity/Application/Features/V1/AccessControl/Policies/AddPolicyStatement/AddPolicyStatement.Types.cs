using System.Text.Json.Serialization;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.AddPolicyStatement;

public class AddPolicyStatementController : TenantControllerBase
{
    [HttpPost("policies/{policyId:guid}/statements")]
    public async Task<ActionResult<PolicyResponse>> Add([FromRoute] Guid policyId, [FromBody] AddPolicyStatementCommand command)
    {
        var result = await this.Dispatcher.Send(command with { PolicyId = policyId });
        return result.Match(Ok, Problem);
    }
}

public sealed record AddPolicyStatementCommand : ICommand<ErrorOr<PolicyResponse>>
{
    [JsonPropertyName("policyId")]
    public Guid PolicyId { get; init; }

    [JsonPropertyName("action")]
    public string Action { get; init; } = string.Empty;

    [JsonPropertyName("resource")]
    public string Resource { get; init; } = "*";

    [JsonPropertyName("effect")]
    public string Effect { get; init; } = "Allow";
}

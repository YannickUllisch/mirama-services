using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mirama.SharedKernel.Models;

[Route("api/v{version:apiVersion}/tenant/{tenantId:guid}")]
[Authorize(Policy = "RequireTenantOnly")]
public abstract class TenantControllerBase : ApiControllerBase
{
}

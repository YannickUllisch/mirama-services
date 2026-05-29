using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mirama.SharedKernel.Models;

[Route("api/v{version:apiVersion}/organization/{organizationId:guid}")]
// [Authorize(Policy = "RequireTenantAndOrg")]
public abstract class OrganizationControllerBase : ApiControllerBase
{
}

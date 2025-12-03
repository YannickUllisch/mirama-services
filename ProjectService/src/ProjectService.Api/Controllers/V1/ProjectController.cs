
using Microsoft.AspNetCore.Mvc;

namespace ProjectService.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/[controller]")]
public class ProjectController
{

    [HttpGet("{id}")]
    public ActionResult<string> GetProjectById([FromRoute] string id)
    {
        return id;
    }
}


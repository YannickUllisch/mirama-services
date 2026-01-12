
using Microsoft.AspNetCore.Mvc;
using ProjectService.Api.Contracts.Requests;

namespace ProjectService.Api.Controllers.V1;

[ApiController]
[Route("v{version:apiVersion}/{teamId}/[controller]")]
public class ProjectController : ControllerBase
{
    [HttpGet]
    public ActionResult<string> GetAssignedProjects([FromRoute] string teamId, [FromRoute] Guid id)
    {
        return id.ToString();
    }

    [HttpGet("{id:guid}")]
    public ActionResult<string> GetProjectById([FromRoute] Guid id)
    {
        return id.ToString();
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        // var command = new CreateProjectCommand(...);
        // var result = await _mediator.Send(command);
        return Ok("tmp");
    }

    [HttpPut("{id:guid}")]
    public ActionResult<string> Update([FromRoute] Guid id, [FromBody] string request)
    {
        return id.ToString();
    }

    [HttpDelete("{id:guid}")]
    public ActionResult<string> Delete([FromRoute] Guid id)
    {
        return id.ToString();
    }
}


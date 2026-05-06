using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Mirama.Modules.Projects.Endpoints;

public static class ProjectsEndpointGroup
{
    public static IEndpointRouteBuilder MapProjectsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGroup("/api/v1/projects")
            .RequireAuthorization();

        return app;
    }
}

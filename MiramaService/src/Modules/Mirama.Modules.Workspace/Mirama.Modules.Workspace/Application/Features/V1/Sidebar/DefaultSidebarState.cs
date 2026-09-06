using System.Text.Json;

namespace Mirama.Modules.Workspace.Application.Features.V1.Sidebar;

internal static class DefaultSidebarState
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);

    public static string Json { get; } = JsonSerializer.Serialize(
        new
        {
            // Ungrouped top-level items.
            items = new object[]
            {
                new { route = "inbox", order = 0, visible = true },
                new { route = "my-work", order = 1, visible = true },
                new { route = "agent", order = 2, visible = true },
                new { route = "dashboard", order = 3, visible = true },
            },
            groups = new object[]
            {
                new
                {
                    group = "workspace",
                    order = 0,
                    items = new object[]
                    {
                        new { route = "projects", order = 0, visible = true },
                        new { route = "boards", order = 0, visible = true },
                        new { route = "members", order = 1, visible = false },
                        new { route = "teams", order = 2, visible = false },
                    },
                },
            },
            favorites = new { items = Array.Empty<object>() },
        },
        JsonOptions);
}

using System.Text.Json;

namespace Mirama.Modules.Workspace.Application.Features.V1.Sidebar;

/// <summary>
/// The sidebar every user starts with before they've customized anything. Computed on
/// demand (see GetSidebarBootstrap) rather than persisted at signup: nothing is written to
/// the database for a user who never touches their sidebar, and if this default ever
/// changes, everyone who hasn't customized yet picks up the improved default on their next
/// load instead of being frozen into whatever shape existed when their account was created.
///
/// Route keys here are stable identifiers the frontend resolves against its own sidebar
/// manifest (icon/label/href) - this module has no knowledge of what a route actually
/// renders, only which ones exist and whether they start visible.
/// </summary>
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
                        new { route = "members", order = 1, visible = false },
                        new { route = "teams", order = 2, visible = false },
                    },
                },
            },
            // User-curated pins - always starts empty, nothing to seed.
            favorites = new { order = 1, items = Array.Empty<object>() },
            // Membership itself is never stored here - "Your clients" is always the live
            // list from Clients.Contracts (see GetSidebarBootstrap). Overrides is a sparse
            // per-client personalization map (order/color/icon/visible), keyed by client id,
            // empty until a user customizes an individual client's row.
            clients = new { order = 2, overrides = new { } },
        },
        JsonOptions);
}

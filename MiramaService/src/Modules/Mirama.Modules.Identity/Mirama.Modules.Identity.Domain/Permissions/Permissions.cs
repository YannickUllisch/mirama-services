namespace Mirama.Modules.Identity.Domain.Permissions;

public static class Permissions
{
    public static class Project
    {
        public const string Read = "project:read";
        public const string Create = "project:create";
        public const string Update = "project:update";
        public const string Delete = "project:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Task
    {
        public const string Read = "task:read";
        public const string Create = "task:create";
        public const string Update = "task:update";
        public const string Delete = "task:delete";
        public const string Assign = "task:assign";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete, Assign];
    }

    public static class Member
    {
        public const string Read = "member:read";
        public const string Create = "member:create";
        public const string Update = "member:update";
        public const string Delete = "member:delete";
        public const string Invite = "member:invite";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete, Invite];
    }

    public static class Milestone
    {
        public const string Read = "milestone:read";
        public const string Create = "milestone:create";
        public const string Update = "milestone:update";
        public const string Delete = "milestone:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Tag
    {
        public const string Read = "tag:read";
        public const string Create = "tag:create";
        public const string Update = "tag:update";
        public const string Delete = "tag:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Invitation
    {
        public const string Read = "invitation:read";
        public const string Create = "invitation:create";
        public const string Update = "invitation:update";
        public const string Delete = "invitation:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Team
    {
        public const string Read = "team:read";
        public const string Create = "team:create";
        public const string Update = "team:update";
        public const string Delete = "team:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Comment
    {
        public const string Read = "comment:read";
        public const string Create = "comment:create";
        public const string Update = "comment:update";
        public const string Delete = "comment:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Expense
    {
        public const string Read = "expense:read";
        public const string Create = "expense:create";
        public const string Update = "expense:update";
        public const string Delete = "expense:delete";

        public static readonly IReadOnlyList<string> All = [Read, Create, Update, Delete];
    }

    public static class Organization
    {
        public const string Read = "organization:read";
        public const string Update = "organization:update";
        public const string Delete = "organization:delete";

        public static readonly IReadOnlyList<string> All = [Read, Update, Delete];
    }

    public static readonly IReadOnlyList<string> All =
    [
        .. Project.All,
        .. Task.All,
        .. Member.All,
        .. Milestone.All,
        .. Tag.All,
        .. Invitation.All,
        .. Team.All,
        .. Comment.All,
        .. Expense.All,
        .. Organization.All,
    ];
}

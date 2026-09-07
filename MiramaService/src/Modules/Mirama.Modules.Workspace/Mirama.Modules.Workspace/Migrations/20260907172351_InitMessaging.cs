using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Workspace.Migrations
{
    /// <inheritdoc />
    public partial class InitMessaging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProcessedAtUtc",
                schema: "workspace",
                table: "OutboxMessages",
                newName: "LockedUntilUtc");

            migrationBuilder.AddColumn<Guid>(
                name: "AggregateId",
                schema: "workspace",
                table: "OutboxMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AvailableAtUtc",
                schema: "workspace",
                table: "OutboxMessages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                schema: "workspace",
                table: "OutboxMessages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Headers",
                schema: "workspace",
                table: "OutboxMessages",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LockedBy",
                schema: "workspace",
                table: "OutboxMessages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                schema: "workspace",
                table: "OutboxMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RetryCount",
                schema: "workspace",
                table: "OutboxMessages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                schema: "workspace",
                table: "OutboxMessages",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TraceId",
                schema: "workspace",
                table: "OutboxMessages",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InboxDeadLetters",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OutboxMessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    TraceId = table.Column<string>(type: "text", nullable: true),
                    CorrelationId = table.Column<string>(type: "text", nullable: true),
                    Headers = table.Column<string>(type: "jsonb", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    HandlerName = table.Column<string>(type: "text", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true),
                    DeadLetteredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxDeadLetters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    TraceId = table.Column<string>(type: "text", nullable: true),
                    CorrelationId = table.Column<string>(type: "text", nullable: true),
                    Headers = table.Column<string>(type: "jsonb", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    HandlerName = table.Column<string>(type: "text", nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AvailableAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LockedUntilUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockedBy = table.Column<string>(type: "text", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxDeadLetters",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AggregateId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: true),
                    TraceId = table.Column<string>(type: "text", nullable: true),
                    CorrelationId = table.Column<string>(type: "text", nullable: true),
                    Headers = table.Column<string>(type: "jsonb", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    RetryCount = table.Column<int>(type: "integer", nullable: false),
                    Error = table.Column<string>(type: "text", nullable: true),
                    DeadLetteredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxDeadLetters", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_workspace_OutboxMessages_Claimable",
                schema: "workspace",
                table: "OutboxMessages",
                columns: new[] { "AvailableAtUtc", "LockedUntilUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_workspace_InboxDeadLetters_OrganizationId",
                schema: "workspace",
                table: "InboxDeadLetters",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_workspace_InboxMessages_Claimable",
                schema: "workspace",
                table: "InboxMessages",
                columns: new[] { "AvailableAtUtc", "LockedUntilUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_workspace_OutboxDeadLetters_OrganizationId",
                schema: "workspace",
                table: "OutboxDeadLetters",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboxDeadLetters",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "InboxMessages",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "OutboxDeadLetters",
                schema: "workspace");

            migrationBuilder.DropIndex(
                name: "IX_workspace_OutboxMessages_Claimable",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "AggregateId",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "AvailableAtUtc",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "Headers",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "LockedBy",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "RetryCount",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "TenantId",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.DropColumn(
                name: "TraceId",
                schema: "workspace",
                table: "OutboxMessages");

            migrationBuilder.RenameColumn(
                name: "LockedUntilUtc",
                schema: "workspace",
                table: "OutboxMessages",
                newName: "ProcessedAtUtc");
        }
    }
}

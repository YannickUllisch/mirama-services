using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Workspace.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "workspace");

            migrationBuilder.CreateTable(
                name: "OutboxMessages",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    OccurredAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ViewStates",
                schema: "workspace",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SurfaceKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ViewType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StateJson = table.Column<string>(type: "jsonb", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewStates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewStates_OrganizationId",
                schema: "workspace",
                table: "ViewStates",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_ViewStates_OrganizationId_UserId_SurfaceKey",
                schema: "workspace",
                table: "ViewStates",
                columns: new[] { "OrganizationId", "UserId", "SurfaceKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessages",
                schema: "workspace");

            migrationBuilder.DropTable(
                name: "ViewStates",
                schema: "workspace");
        }
    }
}

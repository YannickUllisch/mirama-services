using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class onboarding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOnboarded",
                schema: "identity",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnboarded",
                schema: "identity",
                table: "Users");
        }
    }
}

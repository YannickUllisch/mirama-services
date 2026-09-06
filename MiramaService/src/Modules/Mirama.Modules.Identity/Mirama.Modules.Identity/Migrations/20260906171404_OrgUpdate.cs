using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class OrgUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "identity",
                table: "Organizations",
                type: "character varying(63)",
                maxLength: 63,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Organizations_Slug",
                schema: "identity",
                table: "Organizations",
                column: "Slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Organizations_Slug",
                schema: "identity",
                table: "Organizations");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "identity",
                table: "Organizations",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(63)",
                oldMaxLength: 63);
        }
    }
}

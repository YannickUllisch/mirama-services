using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class member_role_array : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IamRoleId",
                schema: "identity",
                table: "Members");

            migrationBuilder.AddColumn<string>(
                name: "IamRoleIds",
                schema: "identity",
                table: "Members",
                type: "jsonb",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IamRoleIds",
                schema: "identity",
                table: "Members");

            migrationBuilder.AddColumn<Guid>(
                name: "IamRoleId",
                schema: "identity",
                table: "Members",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class userExternalIdLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<List<Guid>>(
                name: "LinkedExternalIds",
                schema: "identity",
                table: "Users",
                type: "uuid[]",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                schema: "identity",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_Email",
                schema: "identity",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LinkedExternalIds",
                schema: "identity",
                table: "Users");
        }
    }
}

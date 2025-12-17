using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AccountService.Application.Migrations
{
    /// <inheritdoc />
    public partial class AuditableEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "auth",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "auth",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "auth",
                table: "User",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "User",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "auth",
                table: "Organizations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "auth",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "auth",
                table: "Organizations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Organizations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "auth",
                table: "Members",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "auth",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "auth",
                table: "Members",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "auth",
                table: "Invitations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                schema: "auth",
                table: "Invitations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                schema: "auth",
                table: "Invitations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Invitations",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                schema: "auth",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "auth",
                table: "User");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "auth",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "auth",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "auth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "auth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "auth",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "auth",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "LastModified",
                schema: "auth",
                table: "Invitations");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                schema: "auth",
                table: "Invitations");
        }
    }
}

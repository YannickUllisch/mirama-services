using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Identity.Migrations
{
    /// <inheritdoc />
    public partial class move_tenant_settings_to_organization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrandingColor",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "LogoUrl",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "ReceiveNotifications",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SettingsId",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "SettingsTimezone",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Settings_Created",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Settings_CreatedBy",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Settings_LastModified",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Settings_LastModifiedBy",
                schema: "identity",
                table: "Tenants");

            migrationBuilder.RenameColumn(
                name: "SettingsName",
                schema: "identity",
                table: "Tenants",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "SettingsIsActive",
                schema: "identity",
                table: "Tenants",
                newName: "IsActive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                schema: "identity",
                table: "Tenants",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AccentColor",
                schema: "identity",
                table: "Organizations",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryColor",
                schema: "identity",
                table: "Organizations",
                type: "character varying(7)",
                maxLength: 7,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Region",
                schema: "identity",
                table: "Organizations",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccentColor",
                schema: "identity",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "PrimaryColor",
                schema: "identity",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "Region",
                schema: "identity",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "identity",
                table: "Tenants",
                newName: "SettingsName");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "identity",
                table: "Tenants",
                newName: "SettingsIsActive");

            migrationBuilder.AlterColumn<string>(
                name: "SettingsName",
                schema: "identity",
                table: "Tenants",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "BrandingColor",
                schema: "identity",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoUrl",
                schema: "identity",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ReceiveNotifications",
                schema: "identity",
                table: "Tenants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "SettingsId",
                schema: "identity",
                table: "Tenants",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SettingsTimezone",
                schema: "identity",
                table: "Tenants",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Settings_Created",
                schema: "identity",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Settings_CreatedBy",
                schema: "identity",
                table: "Tenants",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Settings_LastModified",
                schema: "identity",
                table: "Tenants",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Settings_LastModifiedBy",
                schema: "identity",
                table: "Tenants",
                type: "text",
                nullable: true);
        }
    }
}

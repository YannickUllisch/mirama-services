using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.Clients.Migrations
{
    /// <inheritdoc />
    public partial class AddIntakeFormDomain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contract",
                schema: "clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PartyType = table.Column<int>(type: "integer", nullable: false),
                    PartyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contract", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntakeForm",
                schema: "clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Version = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Fields = table.Column<string>(type: "jsonb", nullable: true),
                    Sections = table.Column<string>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntakeForm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntakeFormSubmission",
                schema: "clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IntakeFormId = table.Column<Guid>(type: "uuid", nullable: false),
                    IntakeFormVersion = table.Column<int>(type: "integer", nullable: false),
                    Responses = table.Column<string>(type: "jsonb", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ConvertedToClientId = table.Column<Guid>(type: "uuid", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntakeFormSubmission", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContractTerm",
                schema: "clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EffectiveTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ContractId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContractTerm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContractTerm_Contract_ContractId",
                        column: x => x.ContractId,
                        principalSchema: "clients",
                        principalTable: "Contract",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contract_OrganizationId",
                schema: "clients",
                table: "Contract",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Contract_PartyType_PartyId",
                schema: "clients",
                table: "Contract",
                columns: new[] { "PartyType", "PartyId" });

            migrationBuilder.CreateIndex(
                name: "IX_ContractTerm_ContractId",
                schema: "clients",
                table: "ContractTerm",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_IntakeForm_OrganizationId",
                schema: "clients",
                table: "IntakeForm",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_IntakeFormSubmission_IntakeFormId",
                schema: "clients",
                table: "IntakeFormSubmission",
                column: "IntakeFormId");

            migrationBuilder.CreateIndex(
                name: "IX_IntakeFormSubmission_OrganizationId",
                schema: "clients",
                table: "IntakeFormSubmission",
                column: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContractTerm",
                schema: "clients");

            migrationBuilder.DropTable(
                name: "IntakeForm",
                schema: "clients");

            migrationBuilder.DropTable(
                name: "IntakeFormSubmission",
                schema: "clients");

            migrationBuilder.DropTable(
                name: "Contract",
                schema: "clients");
        }
    }
}

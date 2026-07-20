using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mirama.Modules.PM.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskWorkflowAndProjectDateCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowConfigIdForTaskStatus",
                schema: "projects",
                table: "StatusConfig",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkflowConfigIdForTaskPriority",
                schema: "projects",
                table: "PriorityConfig",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StatusConfig_WorkflowConfigIdForTaskStatus",
                schema: "projects",
                table: "StatusConfig",
                column: "WorkflowConfigIdForTaskStatus");

            migrationBuilder.CreateIndex(
                name: "IX_PriorityConfig_WorkflowConfigIdForTaskPriority",
                schema: "projects",
                table: "PriorityConfig",
                column: "WorkflowConfigIdForTaskPriority");

            migrationBuilder.AddForeignKey(
                name: "FK_PriorityConfig_WorkflowConfig_WorkflowConfigIdForTaskPriori~",
                schema: "projects",
                table: "PriorityConfig",
                column: "WorkflowConfigIdForTaskPriority",
                principalSchema: "projects",
                principalTable: "WorkflowConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StatusConfig_WorkflowConfig_WorkflowConfigIdForTaskStatus",
                schema: "projects",
                table: "StatusConfig",
                column: "WorkflowConfigIdForTaskStatus",
                principalSchema: "projects",
                principalTable: "WorkflowConfig",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriorityConfig_WorkflowConfig_WorkflowConfigIdForTaskPriori~",
                schema: "projects",
                table: "PriorityConfig");

            migrationBuilder.DropForeignKey(
                name: "FK_StatusConfig_WorkflowConfig_WorkflowConfigIdForTaskStatus",
                schema: "projects",
                table: "StatusConfig");

            migrationBuilder.DropIndex(
                name: "IX_StatusConfig_WorkflowConfigIdForTaskStatus",
                schema: "projects",
                table: "StatusConfig");

            migrationBuilder.DropIndex(
                name: "IX_PriorityConfig_WorkflowConfigIdForTaskPriority",
                schema: "projects",
                table: "PriorityConfig");

            migrationBuilder.DropColumn(
                name: "WorkflowConfigIdForTaskStatus",
                schema: "projects",
                table: "StatusConfig");

            migrationBuilder.DropColumn(
                name: "WorkflowConfigIdForTaskPriority",
                schema: "projects",
                table: "PriorityConfig");
        }
    }
}

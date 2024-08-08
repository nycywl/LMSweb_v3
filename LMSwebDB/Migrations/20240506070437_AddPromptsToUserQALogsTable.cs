using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddPromptsToUserQALogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompletionToken",
                table: "UserQALogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PromptToken",
                table: "UserQALogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SystemPrompt",
                table: "UserQALogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TotalToken",
                table: "UserQALogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserPrompt",
                table: "UserQALogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletionToken",
                table: "UserQALogs");

            migrationBuilder.DropColumn(
                name: "PromptToken",
                table: "UserQALogs");

            migrationBuilder.DropColumn(
                name: "SystemPrompt",
                table: "UserQALogs");

            migrationBuilder.DropColumn(
                name: "TotalToken",
                table: "UserQALogs");

            migrationBuilder.DropColumn(
                name: "UserPrompt",
                table: "UserQALogs");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddAndAlterColumnsForCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OpenAIPrompt",
                table: "Courses",
                newName: "SystemPrompt");

            migrationBuilder.AddColumn<string>(
                name: "UserPrompt",
                table: "Courses",
                type: "ntext",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserPrompt",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SystemPrompt",
                table: "Courses",
                newName: "OpenAIPrompt");
        }
    }
}

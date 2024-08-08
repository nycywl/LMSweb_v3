using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnOpenAIPromptToCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OpenAIPrompt",
                table: "Courses",
                type: "ntext",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpenAIPrompt",
                table: "Courses");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddAssistantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Assistants",
                columns: table => new
                {
                    AssistantID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    AssistantName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assistants", x => x.AssistantID);
                    table.ForeignKey(
                        name: "FK_Assistants_Users",
                        column: x => x.AssistantID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Assistants");
        }
    }
}

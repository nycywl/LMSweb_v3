using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddAssistantAndCourseRelationTableManage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Manages",
                columns: table => new
                {
                    AssistantID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CourseID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manages", x => new { x.AssistantID, x.CourseID });
                    table.ForeignKey(
                        name: "FK_Assistants_Manage",
                        column: x => x.AssistantID,
                        principalTable: "Assistants",
                        principalColumn: "AssistantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Courses_Manage",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manages_CourseID",
                table: "Manages",
                column: "CourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manages");
        }
    }
}

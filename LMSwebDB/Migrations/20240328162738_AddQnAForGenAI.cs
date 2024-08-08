using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddQnAForGenAI : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QnAs",
                columns: table => new
                {
                    QnAID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Embeddings = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    CourseID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MaterialID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QnAs", x => x.QnAID);
                    table.ForeignKey(
                        name: "FK_QnA_Material",
                        column: x => x.MaterialID,
                        principalTable: "Materials",
                        principalColumn: "MaterialID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QnAs_MaterialID",
                table: "QnAs",
                column: "MaterialID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QnAs");
        }
    }
}

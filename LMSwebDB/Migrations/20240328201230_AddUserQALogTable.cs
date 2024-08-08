using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AddUserQALogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserQALogs",
                columns: table => new
                {
                    LogID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CourseID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    MaterialID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UserQuestion = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    AnswerFromGPT = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Score = table.Column<float>(type: "real", nullable: false, defaultValue: 0f),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.LogID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserQALogs");
        }
    }
}

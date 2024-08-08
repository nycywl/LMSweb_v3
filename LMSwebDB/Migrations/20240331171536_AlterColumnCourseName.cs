using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AlterColumnCourseName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CName",
                table: "Courses",
                newName: "CourseName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CourseName",
                table: "Courses",
                newName: "CName");
        }
    }
}

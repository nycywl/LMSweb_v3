using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistants_Users",
                table: "Assistants");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Student",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teacher",
                table: "Teachers",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
               name: "FK_Courses_Teachers",
               table: "Courses",
               column: "TeacherID",
               principalTable: "Teachers",
               principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistants_Users",
                table: "Assistants",
                column: "AssistantID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Course",
                table: "Students",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Users",
                table: "Teachers",
                column: "TeacherID",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assistants_Users",
                table: "Assistants");

            migrationBuilder.DropForeignKey(
                name: "FK_Student_Course",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Teachers",
                table: "Courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Teacher",
                table: "Teachers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Teachers",
                table: "Teachers",
                column: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Teachers",
                table: "Courses",
                column: "TeacherID",
                principalTable: "Teachers",
                principalColumn: "TeacherID");

            migrationBuilder.AddForeignKey(
                name: "FK_Assistants_Users",
                table: "Assistants",
                column: "AssistantID",
                principalTable: "Users",
                principalColumn: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Student_Student",
                table: "Students",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Teachers_Users",
                table: "Teachers",
                column: "TeacherID",
                principalTable: "Users",
                principalColumn: "UserID");
        }
    }
}

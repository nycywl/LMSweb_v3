using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LMSwebDB.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    UPassword = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    TeacherID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TeacherName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.TeacherID);
                    table.ForeignKey(
                        name: "FK_Teachers_Users",
                        column: x => x.TeacherID,
                        principalTable: "Users",
                        principalColumn: "UserID");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    TeacherID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    CName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseID);
                    table.ForeignKey(
                        name: "FK_Courses_Teachers",
                        column: x => x.TeacherID,
                        principalTable: "Teachers",
                        principalColumn: "TeacherID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    StudentID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    CourseID = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    StudentName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: ""),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentID);
                    table.ForeignKey(
                        name: "FK_Student_Student",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "CourseID");
                    table.ForeignKey(
                        name: "FK_Students_Users",
                        column: x => x.StudentID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserID", "Name", "RoleName", "UPassword" },
                values: new object[,]
                {
                    { "S001", "林小楷", "Student", "c5e0e70cb9001fc326a9d5b3c39c1d3b48919bf3adacc633729bfff7c27f1d26" },
                    { "S002", "李阿禎", "Student", "c842f8af9e82946217a5f35c046c0470ce855b145a093448295d758810c68303" },
                    { "S003", "許小琪", "Student", "f598aee2eda0ebd461a60eb24ecc6378674be0d06a591ef8f25b201e4f619e48" },
                    { "S004", "Kevin", "Student", "db3ea858db39f2f6eafd7ad39f7798428d9e6244a430919f84c7dc8b905081ad" },
                    { "S005", "Vivian", "Student", "09fd191dc08a0375f4f10fd8ce970d8193a0b475bb3d75db4b8221e8f0d74979" },
                    { "S006", "Amy", "Student", "19a85017e5a5057f9cb3104e7afde89aea6c4d74f544ba5eaeaab256bcf937af" },
                    { "T001", "陳立維", "Teacher", "15152d459354c17470fbeba5c03aa9b0790b237b04f190aba04b2a3d1afe64bf" },
                    { "T002", "曾老師", "Teacher", "9ba7d0652682e1fe75b90bd1ea8a1a69e679a0039c80fc9c85e10e2ff7ddc793" },
                    { "T003", "李偉老師", "Teacher", "963606fbc3791a6c3053264f977ce910821a69680e5e41de99e6b3f04d7d0471" },
                    { "T004", "焰超老師", "Teacher", "877b4011250e9bc6afe05dcddfa93ec093139527a094a8fb0f8f6f80bf0cce2e" },
                    { "T005", "蔡老師", "Teacher", "369be97f36a54ac72f4e7e3a69ca6860b6bf17d148b0686d0ff9e18a1bd32249" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "StudentID", "CourseID", "Gender", "StudentName" },
                values: new object[,]
                {
                    { "S001", null, null, "林小楷" },
                    { "S002", null, null, "李阿禎" },
                    { "S003", null, null, "許小琪" },
                    { "S004", null, null, "Kevin" },
                    { "S005", null, null, "Vivian" },
                    { "S006", null, null, "Amy" }
                });

            migrationBuilder.InsertData(
                table: "Teachers",
                columns: new[] { "TeacherID", "TeacherName" },
                values: new object[,]
                {
                    { "T001", "陳立維" },
                    { "T002", "曾老師" },
                    { "T003", "李偉老師" },
                    { "T004", "焰超老師" },
                    { "T005", "蔡老師" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_TeacherID",
                table: "Courses",
                column: "TeacherID");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CourseID",
                table: "Students",
                column: "CourseID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

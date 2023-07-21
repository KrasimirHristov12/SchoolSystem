using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnnecessaryProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers",
                table: "Quizzes");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Quizzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Quizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

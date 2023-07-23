using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedStudentsQuizzes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answers",
                table: "StudentsQuizzes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Answers",
                table: "StudentsQuizzes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AdjustedStudentQuizzesAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "IsFourthChecked",
                table: "StudentQuizzesQuestionAnswers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IsFourthChecked",
                table: "StudentQuizzesQuestionAnswers",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameAsnwers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsThirdChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsThirdAnswerChecked");

            migrationBuilder.RenameColumn(
                name: "IsSecondChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsSecondAnswerChecked");

            migrationBuilder.RenameColumn(
                name: "IsFourthChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsFourthAnswerChecked");

            migrationBuilder.RenameColumn(
                name: "IsFirstChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsFirstAnswerChecked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsThirdAnswerChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsThirdChecked");

            migrationBuilder.RenameColumn(
                name: "IsSecondAnswerChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsSecondChecked");

            migrationBuilder.RenameColumn(
                name: "IsFourthAnswerChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsFourthChecked");

            migrationBuilder.RenameColumn(
                name: "IsFirstAnswerChecked",
                table: "StudentQuizzesQuestionAnswers",
                newName: "IsFirstChecked");
        }
    }
}

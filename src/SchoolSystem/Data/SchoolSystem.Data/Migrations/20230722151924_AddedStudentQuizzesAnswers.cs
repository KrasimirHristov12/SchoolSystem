using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedStudentQuizzesAnswers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentQuizzesQuestionAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    IsFirstChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsSecondChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsThirdChecked = table.Column<bool>(type: "bit", nullable: false),
                    IsFourthChecked = table.Column<int>(type: "int", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentQuizzesQuestionAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentQuizzesQuestionAnswers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentQuizzesQuestionAnswers_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizzesQuestionAnswers_IsDeleted",
                table: "StudentQuizzesQuestionAnswers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizzesQuestionAnswers_QuestionId",
                table: "StudentQuizzesQuestionAnswers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentQuizzesQuestionAnswers_StudentId",
                table: "StudentQuizzesQuestionAnswers",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentQuizzesQuestionAnswers");
        }
    }
}

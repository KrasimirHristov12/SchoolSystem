using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGradingScale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GradingScales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuizId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MinimumPointForPoor = table.Column<int>(type: "int", nullable: false),
                    MaximumPointsForPoor = table.Column<int>(type: "int", nullable: false),
                    MinimumPointsForFair = table.Column<int>(type: "int", nullable: false),
                    MaximumPointsForFair = table.Column<int>(type: "int", nullable: false),
                    MinimumPointsForGood = table.Column<int>(type: "int", nullable: false),
                    MaximumPointsForGood = table.Column<int>(type: "int", nullable: false),
                    MinimumPointsForVeryGood = table.Column<int>(type: "int", nullable: false),
                    MaximumPointsForVeryGood = table.Column<int>(type: "int", nullable: false),
                    MinimumPointsForExcellent = table.Column<int>(type: "int", nullable: false),
                    MaximumPointsForExcellent = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradingScales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradingScales_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradingScales_IsDeleted",
                table: "GradingScales",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_GradingScales_QuizId",
                table: "GradingScales",
                column: "QuizId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradingScales");
        }
    }
}

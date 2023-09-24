using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedDeletedColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "StudentsQuizzes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "StudentsQuizzes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentsQuizzes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "StudentsQuizzes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentsQuizzes_IsDeleted",
                table: "StudentsQuizzes",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_StudentsQuizzes_IsDeleted",
                table: "StudentsQuizzes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "StudentsQuizzes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "StudentsQuizzes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentsQuizzes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "StudentsQuizzes");
        }
    }
}

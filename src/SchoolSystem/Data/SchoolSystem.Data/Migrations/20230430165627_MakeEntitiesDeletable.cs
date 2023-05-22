#nullable disable
namespace SchoolSystem.Data.Migrations
{
    using System;

    using Microsoft.EntityFrameworkCore.Migrations;

    /// <inheritdoc />
    public partial class MakeEntitiesDeletable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Teachers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Teachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Teachers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Teachers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Students",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Classes",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOn",
                table: "Classes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Classes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_IsDeleted",
                table: "Teachers",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Students_IsDeleted",
                table: "Students",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_IsDeleted",
                table: "Classes",
                column: "IsDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Teachers_IsDeleted",
                table: "Teachers");

            migrationBuilder.DropIndex(
                name: "IX_Students_IsDeleted",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Classes_IsDeleted",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "DeletedOn",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Classes");
        }
    }
}

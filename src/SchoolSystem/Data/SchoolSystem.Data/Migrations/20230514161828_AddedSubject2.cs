#nullable disable
namespace SchoolSystem.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    /// <inheritdoc />
    public partial class AddedSubject2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Subject_SubjectsId",
                table: "SubjectTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subject",
                table: "Subject");

            migrationBuilder.RenameTable(
                name: "Subject",
                newName: "Subjects");

            migrationBuilder.RenameIndex(
                name: "IX_Subject_IsDeleted",
                table: "Subjects",
                newName: "IX_Subjects_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Subjects_SubjectsId",
                table: "SubjectTeacher",
                column: "SubjectsId",
                principalTable: "Subjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectTeacher_Subjects_SubjectsId",
                table: "SubjectTeacher");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Subjects",
                table: "Subjects");

            migrationBuilder.RenameTable(
                name: "Subjects",
                newName: "Subject");

            migrationBuilder.RenameIndex(
                name: "IX_Subjects_IsDeleted",
                table: "Subject",
                newName: "IX_Subject_IsDeleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subject",
                table: "Subject",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectTeacher_Subject_SubjectsId",
                table: "SubjectTeacher",
                column: "SubjectsId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

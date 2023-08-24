using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteredNotifications2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsNew",
                table: "Notifications",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsNew",
                table: "Notifications");
        }
    }
}

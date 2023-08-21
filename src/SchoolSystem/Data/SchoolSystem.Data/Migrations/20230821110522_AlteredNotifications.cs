using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AlteredNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Notifications_NotificationId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_SenderId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_NotificationId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "NotificationsReceivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationsReceivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationsReceivers_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NotificationsReceivers_Notifications_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notifications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsReceivers_NotificationId",
                table: "NotificationsReceivers",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationsReceivers_ReceiverId",
                table: "NotificationsReceivers",
                column: "ReceiverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationsReceivers");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "NotificationId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_SenderId",
                table: "Notifications",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_NotificationId",
                table: "AspNetUsers",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Notifications_NotificationId",
                table: "AspNetUsers",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_SenderId",
                table: "Notifications",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

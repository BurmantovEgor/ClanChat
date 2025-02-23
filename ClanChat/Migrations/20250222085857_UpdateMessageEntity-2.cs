using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClanChat.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageEntity2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Message",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_UserId",
                table: "Message",
                newName: "IX_Message_SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_User_SenderId",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Message",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId",
                table: "Message",
                newName: "IX_Message_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_User_UserId",
                table: "Message",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

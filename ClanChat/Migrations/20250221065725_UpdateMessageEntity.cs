using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClanChat.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClanId",
                table: "Message",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Message_ClanId",
                table: "Message",
                column: "ClanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Clan_ClanId",
                table: "Message",
                column: "ClanId",
                principalTable: "Clan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Clan_ClanId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ClanId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "Message");
        }
    }
}

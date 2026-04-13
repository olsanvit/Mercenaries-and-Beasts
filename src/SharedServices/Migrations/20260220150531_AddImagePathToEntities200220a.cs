using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToEntities200220a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropIndex(
                name: "IX_PlayerMercenarySlots_MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropColumn(
                name: "MercenaryId",
                table: "PlayerMercenarySlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MercenaryId",
                table: "PlayerMercenarySlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMercenarySlots_MercenaryId",
                table: "PlayerMercenarySlots",
                column: "MercenaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId",
                table: "PlayerMercenarySlots",
                column: "MercenaryId",
                principalTable: "PlayerMercenaries",
                principalColumn: "Id");
        }
    }
}

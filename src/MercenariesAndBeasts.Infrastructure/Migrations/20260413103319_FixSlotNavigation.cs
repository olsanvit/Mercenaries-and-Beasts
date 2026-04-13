using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSlotNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerBeastSlots_PlayerMonsters_BeastId",
                table: "PlayerBeastSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropIndex(
                name: "IX_PlayerMercenarySlots_MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropIndex(
                name: "IX_PlayerBeastSlots_BeastId",
                table: "PlayerBeastSlots");

            migrationBuilder.DropColumn(
                name: "MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropColumn(
                name: "BeastId",
                table: "PlayerBeastSlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MercenaryId",
                table: "PlayerMercenarySlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BeastId",
                table: "PlayerBeastSlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMercenarySlots_MercenaryId",
                table: "PlayerMercenarySlots",
                column: "MercenaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBeastSlots_BeastId",
                table: "PlayerBeastSlots",
                column: "BeastId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerBeastSlots_PlayerMonsters_BeastId",
                table: "PlayerBeastSlots",
                column: "BeastId",
                principalTable: "PlayerMonsters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId",
                table: "PlayerMercenarySlots",
                column: "MercenaryId",
                principalTable: "PlayerMercenaries",
                principalColumn: "Id");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixEquipmentSlotNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeastEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "BeastEquipmentSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_MercenaryEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "MercenaryEquipmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_MercenaryEquipmentSlots_PlayerItemId1",
                table: "MercenaryEquipmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_BeastEquipmentSlots_PlayerItemId1",
                table: "BeastEquipmentSlots");

            migrationBuilder.DropColumn(
                name: "PlayerItemId1",
                table: "MercenaryEquipmentSlots");

            migrationBuilder.DropColumn(
                name: "PlayerItemId1",
                table: "BeastEquipmentSlots");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PlayerItemId1",
                table: "MercenaryEquipmentSlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerItemId1",
                table: "BeastEquipmentSlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MercenaryEquipmentSlots_PlayerItemId1",
                table: "MercenaryEquipmentSlots",
                column: "PlayerItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_BeastEquipmentSlots_PlayerItemId1",
                table: "BeastEquipmentSlots",
                column: "PlayerItemId1");

            migrationBuilder.AddForeignKey(
                name: "FK_BeastEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "BeastEquipmentSlots",
                column: "PlayerItemId1",
                principalTable: "PlayerItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MercenaryEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "MercenaryEquipmentSlots",
                column: "PlayerItemId1",
                principalTable: "PlayerItems",
                principalColumn: "Id");
        }
    }
}

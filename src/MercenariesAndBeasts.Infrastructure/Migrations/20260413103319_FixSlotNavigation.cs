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
            migrationBuilder.Sql(@"ALTER TABLE ""PlayerBeastSlots"" DROP CONSTRAINT IF EXISTS ""FK_PlayerBeastSlots_PlayerMonsters_BeastId"";");
            migrationBuilder.Sql(@"ALTER TABLE ""PlayerMercenarySlots"" DROP CONSTRAINT IF EXISTS ""FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_PlayerMercenarySlots_MercenaryId"";");
            migrationBuilder.Sql(@"DROP INDEX IF EXISTS ""IX_PlayerBeastSlots_BeastId"";");
            migrationBuilder.Sql(@"ALTER TABLE ""PlayerMercenarySlots"" DROP COLUMN IF EXISTS ""MercenaryId"";");
            migrationBuilder.Sql(@"ALTER TABLE ""PlayerBeastSlots"" DROP COLUMN IF EXISTS ""BeastId"";");
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

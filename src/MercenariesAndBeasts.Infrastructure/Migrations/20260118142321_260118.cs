using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _260118 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BonusStats_CritMultiplier",
                table: "PlayerMonsters",
                newName: "BonusStats_CriticalMultiplier");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CritChance",
                table: "PlayerMonsters",
                newName: "BonusStats_CriticalChance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CritMultiplier",
                table: "PlayerMercenaries",
                newName: "BonusStats_CriticalMultiplier");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CritChance",
                table: "PlayerMercenaries",
                newName: "BonusStats_CriticalChance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CritMultiplier",
                table: "PlayerItems",
                newName: "BonusStats_CriticalMultiplier");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CritChance",
                table: "PlayerItems",
                newName: "BonusStats_CriticalChance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CritMultiplier",
                table: "MonsterTemplates",
                newName: "BaseStats_CriticalMultiplier");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CritChance",
                table: "MonsterTemplates",
                newName: "BaseStats_CriticalChance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CritMultiplier",
                table: "MercenaryTemplates",
                newName: "BaseStats_CriticalMultiplier");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CritChance",
                table: "MercenaryTemplates",
                newName: "BaseStats_CriticalChance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CritMultiplier",
                table: "ItemTemplates",
                newName: "BaseStats_CriticalMultiplier");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CritChance",
                table: "ItemTemplates",
                newName: "BaseStats_CriticalChance");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BonusStats_CriticalMultiplier",
                table: "PlayerMonsters",
                newName: "BonusStats_CritMultiplier");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CriticalChance",
                table: "PlayerMonsters",
                newName: "BonusStats_CritChance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CriticalMultiplier",
                table: "PlayerMercenaries",
                newName: "BonusStats_CritMultiplier");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CriticalChance",
                table: "PlayerMercenaries",
                newName: "BonusStats_CritChance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CriticalMultiplier",
                table: "PlayerItems",
                newName: "BonusStats_CritMultiplier");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CriticalChance",
                table: "PlayerItems",
                newName: "BonusStats_CritChance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CriticalMultiplier",
                table: "MonsterTemplates",
                newName: "BaseStats_CritMultiplier");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CriticalChance",
                table: "MonsterTemplates",
                newName: "BaseStats_CritChance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CriticalMultiplier",
                table: "MercenaryTemplates",
                newName: "BaseStats_CritMultiplier");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CriticalChance",
                table: "MercenaryTemplates",
                newName: "BaseStats_CritChance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CriticalMultiplier",
                table: "ItemTemplates",
                newName: "BaseStats_CritMultiplier");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CriticalChance",
                table: "ItemTemplates",
                newName: "BaseStats_CritChance");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add26011116 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BaseStats_ActionCostReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ActionCostReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ActionCostReduction",
                table: "ItemTemplates");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CooldownReduction",
                table: "PlayerMonsters",
                newName: "BonusStats_StatusPotency");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CooldownReduction",
                table: "PlayerMercenaries",
                newName: "BonusStats_StatusPotency");

            migrationBuilder.RenameColumn(
                name: "BonusStats_CooldownReduction",
                table: "PlayerItems",
                newName: "BonusStats_StatusPotency");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CooldownReduction",
                table: "MonsterTemplates",
                newName: "BaseStats_StatusPotency");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CooldownReduction",
                table: "MercenaryTemplates",
                newName: "BaseStats_StatusPotency");

            migrationBuilder.RenameColumn(
                name: "BaseStats_CooldownReduction",
                table: "ItemTemplates",
                newName: "BaseStats_StatusPotency");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BonusStats_StatusPotency",
                table: "PlayerMonsters",
                newName: "BonusStats_CooldownReduction");

            migrationBuilder.RenameColumn(
                name: "BonusStats_StatusPotency",
                table: "PlayerMercenaries",
                newName: "BonusStats_CooldownReduction");

            migrationBuilder.RenameColumn(
                name: "BonusStats_StatusPotency",
                table: "PlayerItems",
                newName: "BonusStats_CooldownReduction");

            migrationBuilder.RenameColumn(
                name: "BaseStats_StatusPotency",
                table: "MonsterTemplates",
                newName: "BaseStats_CooldownReduction");

            migrationBuilder.RenameColumn(
                name: "BaseStats_StatusPotency",
                table: "MercenaryTemplates",
                newName: "BaseStats_CooldownReduction");

            migrationBuilder.RenameColumn(
                name: "BaseStats_StatusPotency",
                table: "ItemTemplates",
                newName: "BaseStats_CooldownReduction");

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ActionCostReduction",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ActionCostReduction",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ActionCostReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

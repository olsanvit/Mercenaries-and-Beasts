using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _260117 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "BonusStats_Armor",
                table: "PlayerMonsters",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CounterChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_EnergyCostReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HealingBonus",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HealingReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BonusStats_MaxEnergy",
                table: "PlayerMonsters",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ShieldBonus",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Thorns",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BonusStats_Armor",
                table: "PlayerMercenaries",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CounterChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_EnergyCostReduction",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HealingBonus",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HealingReduction",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BonusStats_MaxEnergy",
                table: "PlayerMercenaries",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ShieldBonus",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Thorns",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BonusStats_Armor",
                table: "PlayerItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CounterChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_EnergyCostReduction",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HealingBonus",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HealingReduction",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BonusStats_MaxEnergy",
                table: "PlayerItems",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ShieldBonus",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Thorns",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BaseStats_Armor",
                table: "MonsterTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CounterChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_EnergyCostReduction",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HealingBonus",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HealingReduction",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BaseStats_MaxEnergy",
                table: "MonsterTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ShieldBonus",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Thorns",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BaseStats_Armor",
                table: "MercenaryTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CounterChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_EnergyCostReduction",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HealingBonus",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HealingReduction",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BaseStats_MaxEnergy",
                table: "MercenaryTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ShieldBonus",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Thorns",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BaseStats_Armor",
                table: "ItemTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CounterChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_EnergyCostReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HealingBonus",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HealingReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<float>(
                name: "BaseStats_MaxEnergy",
                table: "ItemTemplates",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ShieldBonus",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Thorns",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BonusStats_Armor",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_CounterChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_EnergyCostReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_HealingBonus",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_HealingReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_MaxEnergy",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_ShieldBonus",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_Thorns",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_Armor",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_CounterChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_EnergyCostReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_HealingBonus",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_HealingReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_MaxEnergy",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_ShieldBonus",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_Thorns",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_Armor",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_CounterChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_EnergyCostReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_HealingBonus",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_HealingReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_MaxEnergy",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_ShieldBonus",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_Thorns",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BaseStats_Armor",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CounterChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_EnergyCostReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HealingBonus",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HealingReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_MaxEnergy",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ShieldBonus",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Thorns",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Armor",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CounterChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_EnergyCostReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HealingBonus",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HealingReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_MaxEnergy",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ShieldBonus",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Thorns",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Armor",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CounterChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_EnergyCostReduction",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HealingBonus",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HealingReduction",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_MaxEnergy",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ShieldBonus",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Thorns",
                table: "ItemTemplates");
        }
    }
}

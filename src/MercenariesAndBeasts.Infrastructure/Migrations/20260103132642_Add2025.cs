using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add2025 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDungeonAchievements_DungeonAchievements_AchievementId",
                table: "PlayerDungeonAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDungeonAchievements_Players_PlayerId",
                table: "PlayerDungeonAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionAchievements_ExpeditionAchievements_Achieve~",
                table: "PlayerExpeditionAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionAchievements_Players_PlayerId",
                table: "PlayerExpeditionAchievements");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionProgresses_PlayerId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDungeonProgresses_PlayerId",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropIndex(
                name: "IX_ExpeditionAchievements_LocationId",
                table: "ExpeditionAchievements");

            migrationBuilder.DropIndex(
                name: "IX_DungeonAchievements_DungeonId",
                table: "DungeonAchievements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerExpeditionAchievements",
                table: "PlayerExpeditionAchievements");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionAchievements_PlayerId",
                table: "PlayerExpeditionAchievements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerDungeonAchievements",
                table: "PlayerDungeonAchievements");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDungeonAchievements_PlayerId",
                table: "PlayerDungeonAchievements");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerItemDiscoveries");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerItemDiscoveries");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerItemDiscoveries");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerItemDiscoveries");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "PlayerBeastSlots");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "PlayerBeastSlots");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "PlayerBeastSlots");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "PlayerBeastSlots");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "ItemEffect");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "ItemEffect");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "ItemEffect");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "ItemEffect");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "DescriptionCs",
                table: "Dungeons");

            migrationBuilder.DropColumn(
                name: "DescriptionDe",
                table: "Dungeons");

            migrationBuilder.DropColumn(
                name: "NameCs",
                table: "Dungeons");

            migrationBuilder.DropColumn(
                name: "NameDe",
                table: "Dungeons");

            migrationBuilder.RenameTable(
                name: "PlayerExpeditionAchievements",
                newName: "PlayerExpeditionAchievement");

            migrationBuilder.RenameTable(
                name: "PlayerDungeonAchievements",
                newName: "PlayerDungeonAchievement");

            migrationBuilder.RenameColumn(
                name: "BonusStats_ElementalResistance",
                table: "PlayerMonsters",
                newName: "BonusStats_TurnMeterGain");

            migrationBuilder.RenameColumn(
                name: "BonusStats_ElementalDamageBonus",
                table: "PlayerMonsters",
                newName: "BonusStats_TrueDamageBonus");

            migrationBuilder.RenameColumn(
                name: "ContractItemId",
                table: "PlayerMercenarySlots",
                newName: "MercenaryId");

            migrationBuilder.RenameColumn(
                name: "BonusStats_ElementalResistance",
                table: "PlayerMercenaries",
                newName: "BonusStats_TurnMeterGain");

            migrationBuilder.RenameColumn(
                name: "BonusStats_ElementalDamageBonus",
                table: "PlayerMercenaries",
                newName: "BonusStats_TrueDamageBonus");

            migrationBuilder.RenameColumn(
                name: "BonusStats_ElementalResistance",
                table: "PlayerItems",
                newName: "BonusStats_TurnMeterGain");

            migrationBuilder.RenameColumn(
                name: "BonusStats_ElementalDamageBonus",
                table: "PlayerItems",
                newName: "BonusStats_TrueDamageBonus");

            migrationBuilder.RenameColumn(
                name: "EggItemId",
                table: "PlayerBeastSlots",
                newName: "BeastId");

            migrationBuilder.RenameColumn(
                name: "BaseStats_ElementalResistance",
                table: "MonsterTemplates",
                newName: "BaseStats_TurnMeterGain");

            migrationBuilder.RenameColumn(
                name: "BaseStats_ElementalDamageBonus",
                table: "MonsterTemplates",
                newName: "BaseStats_TrueDamageBonus");

            migrationBuilder.RenameColumn(
                name: "BaseStats_ElementalResistance",
                table: "MercenaryTemplates",
                newName: "BaseStats_TurnMeterGain");

            migrationBuilder.RenameColumn(
                name: "BaseStats_ElementalDamageBonus",
                table: "MercenaryTemplates",
                newName: "BaseStats_TrueDamageBonus");

            migrationBuilder.RenameColumn(
                name: "ItemType",
                table: "ItemTemplates",
                newName: "Element");

            migrationBuilder.RenameColumn(
                name: "BaseStats_ElementalResistance",
                table: "ItemTemplates",
                newName: "BaseStats_TurnMeterGain");

            migrationBuilder.RenameColumn(
                name: "BaseStats_ElementalDamageBonus",
                table: "ItemTemplates",
                newName: "BaseStats_TrueDamageBonus");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerExpeditionAchievements_AchievementId",
                table: "PlayerExpeditionAchievement",
                newName: "IX_PlayerExpeditionAchievement_AchievementId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDungeonAchievements_AchievementId",
                table: "PlayerDungeonAchievement",
                newName: "IX_PlayerDungeonAchievement_AchievementId");

            migrationBuilder.AddColumn<float>(
                name: "AccuracyPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ActionCostReductionPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ArmorPenetrationPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "BlockChancePerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CooldownReductionPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CritChancePerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CritMultiplierPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DamageBonusPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DamageReductionPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "DotDamagePerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "EnergyRegenPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "EvasionPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HpRegenPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LifeStealPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "StatusChancePerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "StatusDurationPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "StatusResistancePerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TrueDamageBonusPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TurnMeterGainPerLevel",
                table: "StatScaling",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "CountryChangeCount",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CountryChangedUtc",
                table: "Players",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Players",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStats_Element",
                table: "PlayerMonsters",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Accuracy",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ArmorPenetration",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BleedChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BlockChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BurnChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CleanseChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CooldownReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DamageBonus",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DamageReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DotDamageBonus",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DotDamageReduction",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_EnergyRegen",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Evasion",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_FreezeChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HpRegen",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_LifeSteal",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_PoisonChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ShockChance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_StatusDurationBonus",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_StatusResistance",
                table: "PlayerMonsters",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStats_Element",
                table: "PlayerMercenaries",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Accuracy",
                table: "PlayerMercenaries",
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
                name: "BonusStats_ArmorPenetration",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BleedChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BlockChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BurnChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CleanseChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CooldownReduction",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DamageBonus",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DamageReduction",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DotDamageBonus",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DotDamageReduction",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_EnergyRegen",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Evasion",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_FreezeChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HpRegen",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_LifeSteal",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_PoisonChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ShockChance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_StatusDurationBonus",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_StatusResistance",
                table: "PlayerMercenaries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStats_Element",
                table: "PlayerItems",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Accuracy",
                table: "PlayerItems",
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
                name: "BonusStats_ArmorPenetration",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BleedChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BlockChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_BurnChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CleanseChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_CooldownReduction",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DamageBonus",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DamageReduction",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DotDamageBonus",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_DotDamageReduction",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_EnergyRegen",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_Evasion",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_FreezeChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_HpRegen",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_LifeSteal",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_PoisonChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_ShockChance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_StatusDurationBonus",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BonusStats_StatusResistance",
                table: "PlayerItems",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "EquipSlot",
                table: "PlayerItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "AchievementsId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EncounterId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EncounterOrder1TemplateId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EncounterOrder2TemplateId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EncounterOrder3TemplateId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EncounterOrder4TemplateId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EncounterOrder5TemplateId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EncounterRolledUtc",
                table: "PlayerExpeditionProgresses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalLosses",
                table: "PlayerExpeditionProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalWins",
                table: "PlayerExpeditionProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinStreak",
                table: "PlayerExpeditionProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrentStage",
                table: "PlayerDungeonProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "WinStreak",
                table: "PlayerDungeonProgresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "BaseStats_Element",
                table: "MonsterTemplates",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Accuracy",
                table: "MonsterTemplates",
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
                name: "BaseStats_ArmorPenetration",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BleedChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BlockChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BurnChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CleanseChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CooldownReduction",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DamageBonus",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DamageReduction",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DotDamageBonus",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DotDamageReduction",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_EnergyRegen",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Evasion",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_FreezeChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HpRegen",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_LifeSteal",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_PoisonChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ShockChance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_StatusDurationBonus",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_StatusResistance",
                table: "MonsterTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "MonsterTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "BaseStats_Element",
                table: "MercenaryTemplates",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Accuracy",
                table: "MercenaryTemplates",
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
                name: "BaseStats_ArmorPenetration",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BleedChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BlockChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BurnChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CleanseChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CooldownReduction",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DamageBonus",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DamageReduction",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DotDamageBonus",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DotDamageReduction",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_EnergyRegen",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Evasion",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_FreezeChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HpRegen",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_LifeSteal",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_PoisonChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ShockChance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_StatusDurationBonus",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_StatusResistance",
                table: "MercenaryTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "MercenaryTemplates",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerItemId1",
                table: "MercenaryEquipmentSlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Element",
                table: "Locations",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "BaseStats_Element",
                table: "ItemTemplates",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Accuracy",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ActionCostReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ArmorPenetration",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BleedChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BlockChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_BurnChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CleanseChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_CooldownReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DamageBonus",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DamageReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DotDamageBonus",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_DotDamageReduction",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_EnergyRegen",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_Evasion",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_FreezeChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_HpRegen",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_LifeSteal",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_PoisonChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_ShockChance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_StatusDurationBonus",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BaseStats_StatusResistance",
                table: "ItemTemplates",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<Guid>(
                name: "MercenaryTemplateId",
                table: "ItemTemplates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MonsterTemplateId",
                table: "ItemTemplates",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEn",
                table: "ExpeditionAchievements",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "ExpeditionAchievements",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ExpeditionAchievements",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Element",
                table: "Dungeons",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<string>(
                name: "NameEn",
                table: "DungeonAchievements",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "DungeonAchievements",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DungeonAchievements",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerItemId1",
                table: "BeastEquipmentSlots",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProgressId",
                table: "PlayerExpeditionAchievement",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Current",
                table: "PlayerDungeonAchievement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "PlayerDungeonAchievement",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ProgressId",
                table: "PlayerDungeonAchievement",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Required",
                table: "PlayerDungeonAchievement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerExpeditionAchievement",
                table: "PlayerExpeditionAchievement",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerDungeonAchievement",
                table: "PlayerDungeonAchievement",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    Iso3 = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    Continent = table.Column<string>(type: "text", nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "ItemUpgradeResources",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    DefaultAmount = table.Column<int>(type: "integer", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DescriptionEn = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemUpgradeResources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemUpgradeResources_ItemTemplates_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LocalizedTexts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Culture = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizedTexts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerExpeditionAchievementSummary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    BossDefeated = table.Column<bool>(type: "boolean", nullable: false),
                    EnemiesDefeatedCount = table.Column<int>(type: "integer", nullable: false),
                    EnemiesTotalCount = table.Column<int>(type: "integer", nullable: false),
                    OrdersDiscoveredCount = table.Column<int>(type: "integer", nullable: false),
                    OrdersTotalCount = table.Column<int>(type: "integer", nullable: false),
                    BestWinStreak = table.Column<int>(type: "integer", nullable: false),
                    RequiredWinStreak = table.Column<int>(type: "integer", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerExpeditionAchievementSummary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionAchievementSummary_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionAchievementSummary_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerExpeditionEncounter",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Target1MercenaryTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Target2MercenaryTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Target3MercenaryTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    BossIndex = table.Column<int>(type: "integer", nullable: false),
                    RollVersion = table.Column<int>(type: "integer", nullable: false),
                    LastRolledUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerExpeditionEncounter", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionEncounter_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionEncounter_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_CountryCode",
                table: "Players",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMercenarySlots_MercenaryId",
                table: "PlayerMercenarySlots",
                column: "MercenaryId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_AchievementsId",
                table: "PlayerExpeditionProgresses",
                column: "AchievementsId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_EncounterId",
                table: "PlayerExpeditionProgresses",
                column: "EncounterId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_PlayerId_LocationId",
                table: "PlayerExpeditionProgresses",
                columns: new[] { "PlayerId", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonProgresses_PlayerId_DungeonId",
                table: "PlayerDungeonProgresses",
                columns: new[] { "PlayerId", "DungeonId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBeastSlots_BeastId",
                table: "PlayerBeastSlots",
                column: "BeastId");

            migrationBuilder.CreateIndex(
                name: "IX_MercenaryEquipmentSlots_PlayerItemId1",
                table: "MercenaryEquipmentSlots",
                column: "PlayerItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_MercenaryTemplateId",
                table: "ItemTemplates",
                column: "MercenaryTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_MonsterTemplateId",
                table: "ItemTemplates",
                column: "MonsterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionAchievements_LocationId_Code",
                table: "ExpeditionAchievements",
                columns: new[] { "LocationId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionAchievements_LocationId_Index",
                table: "ExpeditionAchievements",
                columns: new[] { "LocationId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DungeonAchievements_DungeonId_Code",
                table: "DungeonAchievements",
                columns: new[] { "DungeonId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DungeonAchievements_DungeonId_Index",
                table: "DungeonAchievements",
                columns: new[] { "DungeonId", "Index" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeastEquipmentSlots_PlayerItemId1",
                table: "BeastEquipmentSlots",
                column: "PlayerItemId1");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievement_PlayerId_ProgressId_Achievement~",
                table: "PlayerExpeditionAchievement",
                columns: new[] { "PlayerId", "ProgressId", "AchievementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievement_ProgressId_AchievementId",
                table: "PlayerExpeditionAchievement",
                columns: new[] { "ProgressId", "AchievementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonAchievement_PlayerId_ProgressId_AchievementId",
                table: "PlayerDungeonAchievement",
                columns: new[] { "PlayerId", "ProgressId", "AchievementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonAchievement_ProgressId_AchievementId",
                table: "PlayerDungeonAchievement",
                columns: new[] { "ProgressId", "AchievementId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Continent",
                table: "Countries",
                column: "Continent");

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameEn",
                table: "Countries",
                column: "NameEn");

            migrationBuilder.CreateIndex(
                name: "IX_ItemUpgradeResources_ItemTemplateId_Type",
                table: "ItemUpgradeResources",
                columns: new[] { "ItemTemplateId", "Type" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocalizedTexts_Culture",
                table: "LocalizedTexts",
                column: "Culture");

            migrationBuilder.CreateIndex(
                name: "IX_LocalizedTexts_EntityType_EntityId",
                table: "LocalizedTexts",
                columns: new[] { "EntityType", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizedTexts_EntityType_EntityId_Culture",
                table: "LocalizedTexts",
                columns: new[] { "EntityType", "EntityId", "Culture" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievementSummary_LocationId",
                table: "PlayerExpeditionAchievementSummary",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievementSummary_PlayerId_LocationId",
                table: "PlayerExpeditionAchievementSummary",
                columns: new[] { "PlayerId", "LocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionEncounter_LocationId",
                table: "PlayerExpeditionEncounter",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionEncounter_PlayerId",
                table: "PlayerExpeditionEncounter",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_BeastEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "BeastEquipmentSlots",
                column: "PlayerItemId1",
                principalTable: "PlayerItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTemplates_MercenaryTemplates_MercenaryTemplateId",
                table: "ItemTemplates",
                column: "MercenaryTemplateId",
                principalTable: "MercenaryTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ItemTemplates_MonsterTemplates_MonsterTemplateId",
                table: "ItemTemplates",
                column: "MonsterTemplateId",
                principalTable: "MonsterTemplates",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MercenaryEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "MercenaryEquipmentSlots",
                column: "PlayerItemId1",
                principalTable: "PlayerItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerBeastSlots_PlayerMonsters_BeastId",
                table: "PlayerBeastSlots",
                column: "BeastId",
                principalTable: "PlayerMonsters",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDungeonAchievement_DungeonAchievements_AchievementId",
                table: "PlayerDungeonAchievement",
                column: "AchievementId",
                principalTable: "DungeonAchievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDungeonAchievement_PlayerDungeonProgresses_ProgressId",
                table: "PlayerDungeonAchievement",
                column: "ProgressId",
                principalTable: "PlayerDungeonProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDungeonAchievement_Players_PlayerId",
                table: "PlayerDungeonAchievement",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionAchievement_ExpeditionAchievements_Achievem~",
                table: "PlayerExpeditionAchievement",
                column: "AchievementId",
                principalTable: "ExpeditionAchievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionAchievement_PlayerExpeditionProgresses_Prog~",
                table: "PlayerExpeditionAchievement",
                column: "ProgressId",
                principalTable: "PlayerExpeditionProgresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionAchievement_Players_PlayerId",
                table: "PlayerExpeditionAchievement",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionProgresses_PlayerExpeditionAchievementSumma~",
                table: "PlayerExpeditionProgresses",
                column: "AchievementsId",
                principalTable: "PlayerExpeditionAchievementSummary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionProgresses_PlayerExpeditionEncounter_Encoun~",
                table: "PlayerExpeditionProgresses",
                column: "EncounterId",
                principalTable: "PlayerExpeditionEncounter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId",
                table: "PlayerMercenarySlots",
                column: "MercenaryId",
                principalTable: "PlayerMercenaries",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BeastEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "BeastEquipmentSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemTemplates_MercenaryTemplates_MercenaryTemplateId",
                table: "ItemTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_ItemTemplates_MonsterTemplates_MonsterTemplateId",
                table: "ItemTemplates");

            migrationBuilder.DropForeignKey(
                name: "FK_MercenaryEquipmentSlots_PlayerItems_PlayerItemId1",
                table: "MercenaryEquipmentSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerBeastSlots_PlayerMonsters_BeastId",
                table: "PlayerBeastSlots");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDungeonAchievement_DungeonAchievements_AchievementId",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDungeonAchievement_PlayerDungeonProgresses_ProgressId",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDungeonAchievement_Players_PlayerId",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionAchievement_ExpeditionAchievements_Achievem~",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionAchievement_PlayerExpeditionProgresses_Prog~",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionAchievement_Players_PlayerId",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionProgresses_PlayerExpeditionAchievementSumma~",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionProgresses_PlayerExpeditionEncounter_Encoun~",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "ItemUpgradeResources");

            migrationBuilder.DropTable(
                name: "LocalizedTexts");

            migrationBuilder.DropTable(
                name: "PlayerExpeditionAchievementSummary");

            migrationBuilder.DropTable(
                name: "PlayerExpeditionEncounter");

            migrationBuilder.DropIndex(
                name: "IX_Players_CountryCode",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_PlayerMercenarySlots_MercenaryId",
                table: "PlayerMercenarySlots");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionProgresses_AchievementsId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionProgresses_EncounterId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionProgresses_PlayerId_LocationId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDungeonProgresses_PlayerId_DungeonId",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropIndex(
                name: "IX_PlayerBeastSlots_BeastId",
                table: "PlayerBeastSlots");

            migrationBuilder.DropIndex(
                name: "IX_MercenaryEquipmentSlots_PlayerItemId1",
                table: "MercenaryEquipmentSlots");

            migrationBuilder.DropIndex(
                name: "IX_ItemTemplates_MercenaryTemplateId",
                table: "ItemTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ItemTemplates_MonsterTemplateId",
                table: "ItemTemplates");

            migrationBuilder.DropIndex(
                name: "IX_ExpeditionAchievements_LocationId_Code",
                table: "ExpeditionAchievements");

            migrationBuilder.DropIndex(
                name: "IX_ExpeditionAchievements_LocationId_Index",
                table: "ExpeditionAchievements");

            migrationBuilder.DropIndex(
                name: "IX_DungeonAchievements_DungeonId_Code",
                table: "DungeonAchievements");

            migrationBuilder.DropIndex(
                name: "IX_DungeonAchievements_DungeonId_Index",
                table: "DungeonAchievements");

            migrationBuilder.DropIndex(
                name: "IX_BeastEquipmentSlots_PlayerItemId1",
                table: "BeastEquipmentSlots");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerExpeditionAchievement",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionAchievement_PlayerId_ProgressId_Achievement~",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionAchievement_ProgressId_AchievementId",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerDungeonAchievement",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDungeonAchievement_PlayerId_ProgressId_AchievementId",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropIndex(
                name: "IX_PlayerDungeonAchievement_ProgressId_AchievementId",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropColumn(
                name: "AccuracyPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "ActionCostReductionPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "ArmorPenetrationPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "BlockChancePerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "CooldownReductionPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "CritChancePerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "CritMultiplierPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "DamageBonusPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "DamageReductionPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "DotDamagePerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "EnergyRegenPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "EvasionPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "HpRegenPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "LifeStealPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "StatusChancePerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "StatusDurationPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "StatusResistancePerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "TrueDamageBonusPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "TurnMeterGainPerLevel",
                table: "StatScaling");

            migrationBuilder.DropColumn(
                name: "CountryChangeCount",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CountryChangedUtc",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "BonusStats_Accuracy",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_ArmorPenetration",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_BleedChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_BlockChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_BurnChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_CleanseChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_CooldownReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_DamageBonus",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_DamageReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_DotDamageBonus",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_DotDamageReduction",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_EnergyRegen",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_Evasion",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_FreezeChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_HpRegen",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_LifeSteal",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_PoisonChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_ShockChance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_StatusDurationBonus",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_StatusResistance",
                table: "PlayerMonsters");

            migrationBuilder.DropColumn(
                name: "BonusStats_Accuracy",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_ArmorPenetration",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_BleedChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_BlockChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_BurnChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_CleanseChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_CooldownReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_DamageBonus",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_DamageReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_DotDamageBonus",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_DotDamageReduction",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_EnergyRegen",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_Evasion",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_FreezeChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_HpRegen",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_LifeSteal",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_PoisonChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_ShockChance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_StatusDurationBonus",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_StatusResistance",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "BonusStats_Accuracy",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_ActionCostReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_ArmorPenetration",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_BleedChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_BlockChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_BurnChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_CleanseChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_CooldownReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_DamageBonus",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_DamageReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_DotDamageBonus",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_DotDamageReduction",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_EnergyRegen",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_Evasion",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_FreezeChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_HpRegen",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_LifeSteal",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_PoisonChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_ShockChance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_StatusDurationBonus",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "BonusStats_StatusResistance",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "EquipSlot",
                table: "PlayerItems");

            migrationBuilder.DropColumn(
                name: "AchievementsId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterOrder1TemplateId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterOrder2TemplateId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterOrder3TemplateId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterOrder4TemplateId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterOrder5TemplateId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "EncounterRolledUtc",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "TotalLosses",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "TotalWins",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "WinStreak",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "CurrentStage",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "WinStreak",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "BaseStats_Accuracy",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ActionCostReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ArmorPenetration",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BleedChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BlockChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BurnChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CleanseChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CooldownReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DamageBonus",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DamageReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DotDamageBonus",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DotDamageReduction",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_EnergyRegen",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Evasion",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_FreezeChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HpRegen",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_LifeSteal",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_PoisonChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ShockChance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_StatusDurationBonus",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_StatusResistance",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Accuracy",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ActionCostReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ArmorPenetration",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BleedChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BlockChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BurnChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CleanseChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CooldownReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DamageBonus",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DamageReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DotDamageBonus",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DotDamageReduction",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_EnergyRegen",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Evasion",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_FreezeChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HpRegen",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_LifeSteal",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_PoisonChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ShockChance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_StatusDurationBonus",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_StatusResistance",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "PlayerItemId1",
                table: "MercenaryEquipmentSlots");

            migrationBuilder.DropColumn(
                name: "BaseStats_Accuracy",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ActionCostReduction",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ArmorPenetration",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BleedChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BlockChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_BurnChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CleanseChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_CooldownReduction",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DamageBonus",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DamageReduction",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DotDamageBonus",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_DotDamageReduction",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_EnergyRegen",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_Evasion",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_FreezeChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_HpRegen",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_LifeSteal",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_PoisonChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_ShockChance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_StatusDurationBonus",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "BaseStats_StatusResistance",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "MercenaryTemplateId",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "MonsterTemplateId",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "PlayerItemId1",
                table: "BeastEquipmentSlots");

            migrationBuilder.DropColumn(
                name: "ProgressId",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "Current",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropColumn(
                name: "ProgressId",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropColumn(
                name: "Required",
                table: "PlayerDungeonAchievement");

            migrationBuilder.RenameTable(
                name: "PlayerExpeditionAchievement",
                newName: "PlayerExpeditionAchievements");

            migrationBuilder.RenameTable(
                name: "PlayerDungeonAchievement",
                newName: "PlayerDungeonAchievements");

            migrationBuilder.RenameColumn(
                name: "BonusStats_TurnMeterGain",
                table: "PlayerMonsters",
                newName: "BonusStats_ElementalResistance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_TrueDamageBonus",
                table: "PlayerMonsters",
                newName: "BonusStats_ElementalDamageBonus");

            migrationBuilder.RenameColumn(
                name: "MercenaryId",
                table: "PlayerMercenarySlots",
                newName: "ContractItemId");

            migrationBuilder.RenameColumn(
                name: "BonusStats_TurnMeterGain",
                table: "PlayerMercenaries",
                newName: "BonusStats_ElementalResistance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_TrueDamageBonus",
                table: "PlayerMercenaries",
                newName: "BonusStats_ElementalDamageBonus");

            migrationBuilder.RenameColumn(
                name: "BonusStats_TurnMeterGain",
                table: "PlayerItems",
                newName: "BonusStats_ElementalResistance");

            migrationBuilder.RenameColumn(
                name: "BonusStats_TrueDamageBonus",
                table: "PlayerItems",
                newName: "BonusStats_ElementalDamageBonus");

            migrationBuilder.RenameColumn(
                name: "BeastId",
                table: "PlayerBeastSlots",
                newName: "EggItemId");

            migrationBuilder.RenameColumn(
                name: "BaseStats_TurnMeterGain",
                table: "MonsterTemplates",
                newName: "BaseStats_ElementalResistance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_TrueDamageBonus",
                table: "MonsterTemplates",
                newName: "BaseStats_ElementalDamageBonus");

            migrationBuilder.RenameColumn(
                name: "BaseStats_TurnMeterGain",
                table: "MercenaryTemplates",
                newName: "BaseStats_ElementalResistance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_TrueDamageBonus",
                table: "MercenaryTemplates",
                newName: "BaseStats_ElementalDamageBonus");

            migrationBuilder.RenameColumn(
                name: "Element",
                table: "ItemTemplates",
                newName: "ItemType");

            migrationBuilder.RenameColumn(
                name: "BaseStats_TurnMeterGain",
                table: "ItemTemplates",
                newName: "BaseStats_ElementalResistance");

            migrationBuilder.RenameColumn(
                name: "BaseStats_TrueDamageBonus",
                table: "ItemTemplates",
                newName: "BaseStats_ElementalDamageBonus");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerExpeditionAchievement_AchievementId",
                table: "PlayerExpeditionAchievements",
                newName: "IX_PlayerExpeditionAchievements_AchievementId");

            migrationBuilder.RenameIndex(
                name: "IX_PlayerDungeonAchievement_AchievementId",
                table: "PlayerDungeonAchievements",
                newName: "IX_PlayerDungeonAchievements_AchievementId");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "Players",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "Players",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStats_Element",
                table: "PlayerMonsters",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerMonsters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerMonsters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerMonsters",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerMonsters",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerMercenarySlots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerMercenarySlots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerMercenarySlots",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerMercenarySlots",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStats_Element",
                table: "PlayerMercenaries",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerMercenaries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerMercenaries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerMercenaries",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerMercenaries",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BonusStats_Element",
                table: "PlayerItems",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerItems",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerItemDiscoveries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerItemDiscoveries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerItemDiscoveries",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerItemDiscoveries",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerExpeditionProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerExpeditionProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerExpeditionProgresses",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerExpeditionProgresses",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerDungeonProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerDungeonProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerDungeonProgresses",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerDungeonProgresses",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "PlayerBeastSlots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "PlayerBeastSlots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "PlayerBeastSlots",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "PlayerBeastSlots",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BaseStats_Element",
                table: "MonsterTemplates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "MonsterTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "MonsterTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "MonsterTemplates",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "MonsterTemplates",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BaseStats_Element",
                table: "MercenaryTemplates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "MercenaryTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "MercenaryTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "MercenaryTemplates",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "MercenaryTemplates",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Element",
                table: "Locations",
                type: "integer",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "Locations",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "Locations",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BaseStats_Element",
                table: "ItemTemplates",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "ItemTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "ItemTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "ItemTemplates",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "ItemTemplates",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "ItemEffect",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "ItemEffect",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "ItemEffect",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "ItemEffect",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "ExpeditionStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "ExpeditionStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "ExpeditionStages",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "ExpeditionStages",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEn",
                table: "ExpeditionAchievements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "ExpeditionAchievements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "ExpeditionAchievements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "DungeonStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "DungeonStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "DungeonStages",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "DungeonStages",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Element",
                table: "Dungeons",
                type: "integer",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(32)",
                oldMaxLength: 32);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionCs",
                table: "Dungeons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionDe",
                table: "Dungeons",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameCs",
                table: "Dungeons",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameDe",
                table: "Dungeons",
                type: "character varying(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEn",
                table: "DungeonAchievements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionEn",
                table: "DungeonAchievements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(2048)",
                oldMaxLength: 2048);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "DungeonAchievements",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerExpeditionAchievements",
                table: "PlayerExpeditionAchievements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerDungeonAchievements",
                table: "PlayerDungeonAchievements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_PlayerId",
                table: "PlayerExpeditionProgresses",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonProgresses_PlayerId",
                table: "PlayerDungeonProgresses",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionAchievements_LocationId",
                table: "ExpeditionAchievements",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonAchievements_DungeonId",
                table: "DungeonAchievements",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievements_PlayerId",
                table: "PlayerExpeditionAchievements",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonAchievements_PlayerId",
                table: "PlayerDungeonAchievements",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDungeonAchievements_DungeonAchievements_AchievementId",
                table: "PlayerDungeonAchievements",
                column: "AchievementId",
                principalTable: "DungeonAchievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDungeonAchievements_Players_PlayerId",
                table: "PlayerDungeonAchievements",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionAchievements_ExpeditionAchievements_Achieve~",
                table: "PlayerExpeditionAchievements",
                column: "AchievementId",
                principalTable: "ExpeditionAchievements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionAchievements_Players_PlayerId",
                table: "PlayerExpeditionAchievements",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

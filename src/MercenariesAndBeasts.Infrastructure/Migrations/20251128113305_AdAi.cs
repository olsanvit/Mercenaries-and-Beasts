using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AdAi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseLevel",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Locations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BaseLevel",
                table: "Dungeons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "Dungeons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DungeonAchievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DungeonId = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DungeonAchievements_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionAchievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionAchievements_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: true),
                    BaseStats_MaxHp = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Attack = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Defense = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseQuality = table.Column<int>(type: "integer", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MercenaryTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_MaxHp = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Attack = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Defense = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MercenaryTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonsterTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_MaxHp = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Attack = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Defense = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Experience = table.Column<long>(type: "bigint", nullable: false),
                    Energy = table.Column<int>(type: "integer", nullable: false),
                    MaxEnergy = table.Column<int>(type: "integer", nullable: false),
                    Gold = table.Column<long>(type: "bigint", nullable: false),
                    ActiveMercenaryTeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActiveMonsterTeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemEffect",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EffectType = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<double>(type: "double precision", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    TargetStat = table.Column<string>(type: "text", nullable: true),
                    ItemTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemEffect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemEffect_ItemTemplates_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ExpeditionStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageNumber = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    EnemyMercenaryId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnemyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FixedLevel = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpeditionStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpeditionStages_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpeditionStages_MercenaryTemplates_EnemyId",
                        column: x => x.EnemyId,
                        principalTable: "MercenaryTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DungeonStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DungeonId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageIndex = table.Column<int>(type: "integer", nullable: false),
                    StageType = table.Column<int>(type: "integer", nullable: false),
                    MonsterTemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    RecommendedLevel = table.Column<int>(type: "integer", nullable: false),
                    DifficultyRating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DungeonStages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DungeonStages_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DungeonStages_MonsterTemplates_MonsterTemplateId",
                        column: x => x.MonsterTemplateId,
                        principalTable: "MonsterTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDungeonAchievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AchievementId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDungeonAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerDungeonAchievements_DungeonAchievements_AchievementId",
                        column: x => x.AchievementId,
                        principalTable: "DungeonAchievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerDungeonAchievements_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDungeonProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    DungeonId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxStageReached = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDungeonProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerDungeonProgresses_Dungeons_DungeonId",
                        column: x => x.DungeonId,
                        principalTable: "Dungeons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerDungeonProgresses_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerExpeditionAchievements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    AchievementId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletedAtUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerExpeditionAchievements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionAchievements_ExpeditionAchievements_Achieve~",
                        column: x => x.AchievementId,
                        principalTable: "ExpeditionAchievements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionAchievements_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerExpeditionProgresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxStageReached = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastUpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerExpeditionProgresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionProgresses_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerExpeditionProgresses_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Quality = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_MaxHp = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Attack = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Defense = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_Element = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    PrefixCode = table.Column<string>(type: "text", nullable: true),
                    SuffixCode = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItems_ItemTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerItems_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMercenaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_MaxHp = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Attack = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Defense = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_Element = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMercenaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMercenaries_MercenaryTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "MercenaryTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMercenaries_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerMonsters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_MaxHp = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Attack = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Defense = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_Speed = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_Element = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMonsters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMonsters_MonsterTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "MonsterTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerMonsters_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DungeonAchievements_DungeonId",
                table: "DungeonAchievements",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonStages_DungeonId",
                table: "DungeonStages",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonStages_MonsterTemplateId",
                table: "DungeonStages",
                column: "MonsterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionAchievements_LocationId",
                table: "ExpeditionAchievements",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionStages_EnemyId",
                table: "ExpeditionStages",
                column: "EnemyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpeditionStages_LocationId",
                table: "ExpeditionStages",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemEffect_ItemTemplateId",
                table: "ItemEffect",
                column: "ItemTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonAchievements_AchievementId",
                table: "PlayerDungeonAchievements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonAchievements_PlayerId",
                table: "PlayerDungeonAchievements",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonProgresses_DungeonId",
                table: "PlayerDungeonProgresses",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDungeonProgresses_PlayerId",
                table: "PlayerDungeonProgresses",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievements_AchievementId",
                table: "PlayerExpeditionAchievements",
                column: "AchievementId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievements_PlayerId",
                table: "PlayerExpeditionAchievements",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_LocationId",
                table: "PlayerExpeditionProgresses",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_PlayerId",
                table: "PlayerExpeditionProgresses",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_PlayerId",
                table: "PlayerItems",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItems_TemplateId",
                table: "PlayerItems",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMercenaries_PlayerId",
                table: "PlayerMercenaries",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMercenaries_TemplateId",
                table: "PlayerMercenaries",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMonsters_PlayerId",
                table: "PlayerMonsters",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMonsters_TemplateId",
                table: "PlayerMonsters",
                column: "TemplateId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DungeonStages");

            migrationBuilder.DropTable(
                name: "ExpeditionStages");

            migrationBuilder.DropTable(
                name: "ItemEffect");

            migrationBuilder.DropTable(
                name: "PlayerDungeonAchievements");

            migrationBuilder.DropTable(
                name: "PlayerDungeonProgresses");

            migrationBuilder.DropTable(
                name: "PlayerExpeditionAchievements");

            migrationBuilder.DropTable(
                name: "PlayerExpeditionProgresses");

            migrationBuilder.DropTable(
                name: "PlayerItems");

            migrationBuilder.DropTable(
                name: "PlayerMercenaries");

            migrationBuilder.DropTable(
                name: "PlayerMonsters");

            migrationBuilder.DropTable(
                name: "DungeonAchievements");

            migrationBuilder.DropTable(
                name: "ExpeditionAchievements");

            migrationBuilder.DropTable(
                name: "ItemTemplates");

            migrationBuilder.DropTable(
                name: "MercenaryTemplates");

            migrationBuilder.DropTable(
                name: "MonsterTemplates");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropColumn(
                name: "BaseLevel",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "BaseLevel",
                table: "Dungeons");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "Dungeons");
        }
    }
}

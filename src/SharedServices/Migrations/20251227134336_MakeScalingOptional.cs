using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeScalingOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    IsAdmin = table.Column<bool>(type: "boolean", nullable: false),
                    PreferredCulture = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    IsBanned = table.Column<bool>(type: "boolean", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dungeons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Element = table.Column<int>(type: "integer", maxLength: 32, nullable: false),
                    UnlockOrder = table.Column<int>(type: "integer", nullable: false),
                    MinLevel = table.Column<int>(type: "integer", nullable: false),
                    MaxLevel = table.Column<int>(type: "integer", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dungeons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Element = table.Column<int>(type: "integer", maxLength: 32, nullable: false),
                    UnlockOrder = table.Column<int>(type: "integer", nullable: false),
                    MinLevel = table.Column<int>(type: "integer", nullable: false),
                    MaxLevel = table.Column<int>(type: "integer", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    NameEn = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MercenaryTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_MaxHp = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Attack = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Defense = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Speed = table.Column<float>(type: "real", nullable: false),
                    BaseStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_Element = table.Column<string>(type: "text", nullable: false),
                    BaseStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                    Element = table.Column<int>(type: "integer", nullable: false),
                    BaseStats_MaxHp = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Attack = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Defense = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Speed = table.Column<float>(type: "real", nullable: false),
                    BaseStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_Element = table.Column<string>(type: "text", nullable: false),
                    BaseStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                    SoftCurrency = table.Column<long>(type: "bigint", nullable: false),
                    PremiumCurrency = table.Column<long>(type: "bigint", nullable: false),
                    ActiveMercenaryTeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    ActiveMonsterTeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsInitialized = table.Column<bool>(type: "boolean", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatScaling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AttackPerLevel = table.Column<float>(type: "real", nullable: false),
                    DefensePerLevel = table.Column<float>(type: "real", nullable: false),
                    HpPerLevel = table.Column<float>(type: "real", nullable: false),
                    SpeedPerLevel = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatScaling", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "ExpeditionStages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: false),
                    StageNumber = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    EnemyId = table.Column<Guid>(type: "uuid", nullable: false),
                    FixedLevel = table.Column<int>(type: "integer", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                    DifficultyRating = table.Column<int>(type: "integer", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                name: "PlayerMercenaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    Level = table.Column<int>(type: "integer", nullable: false),
                    Rank = table.Column<int>(type: "integer", nullable: false),
                    BonusStats_MaxHp = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Attack = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Defense = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Speed = table.Column<float>(type: "real", nullable: false),
                    BonusStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_Element = table.Column<string>(type: "text", nullable: false),
                    BonusStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                    BonusStats_MaxHp = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Attack = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Defense = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Speed = table.Column<float>(type: "real", nullable: false),
                    BonusStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_Element = table.Column<string>(type: "text", nullable: false),
                    BonusStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "ItemTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    MercenarySlot = table.Column<int>(type: "integer", nullable: true),
                    MonsterSlot = table.Column<int>(type: "integer", nullable: true),
                    BaseStats_MaxHp = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Attack = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Defense = table.Column<float>(type: "real", nullable: false),
                    BaseStats_Speed = table.Column<float>(type: "real", nullable: false),
                    BaseStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_Element = table.Column<string>(type: "text", nullable: false),
                    BaseStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BaseStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    OwnerKind = table.Column<int>(type: "integer", nullable: false),
                    BaseQuality = table.Column<int>(type: "integer", nullable: false),
                    UpgradeTarget = table.Column<int>(type: "integer", nullable: false),
                    GrantedMercenaryTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    GrantedMonsterTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    ScalingId = table.Column<Guid>(type: "uuid", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemTemplates_MercenaryTemplates_GrantedMercenaryTemplateId",
                        column: x => x.GrantedMercenaryTemplateId,
                        principalTable: "MercenaryTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemTemplates_MonsterTemplates_GrantedMonsterTemplateId",
                        column: x => x.GrantedMonsterTemplateId,
                        principalTable: "MonsterTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemTemplates_StatScaling_ScalingId",
                        column: x => x.ScalingId,
                        principalTable: "StatScaling",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
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
                name: "PlayerMercenarySlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SlotIndex = table.Column<int>(type: "integer", nullable: false),
                    MercenaryInstanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    ContractItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerMercenarySlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerMercenarySlots_PlayerMercenaries_MercenaryInstanceId",
                        column: x => x.MercenaryInstanceId,
                        principalTable: "PlayerMercenaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlayerMercenarySlots_Players_PlayerProfileId",
                        column: x => x.PlayerProfileId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlayerBeastSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    SlotIndex = table.Column<int>(type: "integer", nullable: false),
                    BeastInstanceId = table.Column<Guid>(type: "uuid", nullable: true),
                    EggItemId = table.Column<Guid>(type: "uuid", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerBeastSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerBeastSlots_PlayerMonsters_BeastInstanceId",
                        column: x => x.BeastInstanceId,
                        principalTable: "PlayerMonsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PlayerBeastSlots_Players_PlayerProfileId",
                        column: x => x.PlayerProfileId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    MinQuality = table.Column<int>(type: "integer", nullable: false),
                    ItemTemplateId = table.Column<Guid>(type: "uuid", nullable: true),
                    ItemTemplateId1 = table.Column<Guid>(type: "uuid", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemEffect", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemEffect_ItemTemplates_ItemTemplateId",
                        column: x => x.ItemTemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemEffect_ItemTemplates_ItemTemplateId1",
                        column: x => x.ItemTemplateId1,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlayerItemDiscoveries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uuid", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDiscovered = table.Column<bool>(type: "boolean", nullable: false),
                    TimesFound = table.Column<int>(type: "integer", nullable: false),
                    ItemXp = table.Column<long>(type: "bigint", nullable: false),
                    MasteryLevel = table.Column<int>(type: "integer", nullable: false),
                    FirstDiscoveredUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastFoundUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerItemDiscoveries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerItemDiscoveries_ItemTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "ItemTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerItemDiscoveries_Players_PlayerId",
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
                    BonusStats_MaxHp = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Attack = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Defense = table.Column<float>(type: "real", nullable: false),
                    BonusStats_Speed = table.Column<float>(type: "real", nullable: false),
                    BonusStats_CritChance = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_CritMultiplier = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_Element = table.Column<string>(type: "text", nullable: false),
                    BonusStats_ElementalDamageBonus = table.Column<double>(type: "double precision", nullable: false),
                    BonusStats_ElementalResistance = table.Column<double>(type: "double precision", nullable: false),
                    PrefixCode = table.Column<string>(type: "text", nullable: true),
                    SuffixCode = table.Column<string>(type: "text", nullable: true),
                    BaseLevel = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    NameEn = table.Column<string>(type: "text", nullable: false),
                    DescriptionEn = table.Column<string>(type: "text", nullable: false),
                    NameCs = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionCs = table.Column<string>(type: "text", nullable: true),
                    NameDe = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    DescriptionDe = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    ImagePromptMeta = table.Column<string>(type: "text", nullable: true)
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
                name: "BeastEquipmentSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BeastInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: false),
                    PlayerItemId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeastEquipmentSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeastEquipmentSlots_PlayerItems_PlayerItemId",
                        column: x => x.PlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BeastEquipmentSlots_PlayerMonsters_BeastInstanceId",
                        column: x => x.BeastInstanceId,
                        principalTable: "PlayerMonsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MercenaryEquipmentSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MercenaryInstanceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Slot = table.Column<int>(type: "integer", nullable: false),
                    PlayerItemId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MercenaryEquipmentSlots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MercenaryEquipmentSlots_PlayerItems_PlayerItemId",
                        column: x => x.PlayerItemId,
                        principalTable: "PlayerItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_MercenaryEquipmentSlots_PlayerMercenaries_MercenaryInstance~",
                        column: x => x.MercenaryInstanceId,
                        principalTable: "PlayerMercenaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeastEquipmentSlots_BeastInstanceId_Slot",
                table: "BeastEquipmentSlots",
                columns: new[] { "BeastInstanceId", "Slot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BeastEquipmentSlots_PlayerItemId",
                table: "BeastEquipmentSlots",
                column: "PlayerItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DungeonAchievements_DungeonId",
                table: "DungeonAchievements",
                column: "DungeonId");

            migrationBuilder.CreateIndex(
                name: "IX_Dungeons_Code",
                table: "Dungeons",
                column: "Code",
                unique: true);

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
                name: "IX_ItemEffect_ItemTemplateId1",
                table: "ItemEffect",
                column: "ItemTemplateId1");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_GrantedMercenaryTemplateId",
                table: "ItemTemplates",
                column: "GrantedMercenaryTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_GrantedMonsterTemplateId",
                table: "ItemTemplates",
                column: "GrantedMonsterTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemTemplates_ScalingId",
                table: "ItemTemplates",
                column: "ScalingId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Code",
                table: "Locations",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MercenaryEquipmentSlots_MercenaryInstanceId_Slot",
                table: "MercenaryEquipmentSlots",
                columns: new[] { "MercenaryInstanceId", "Slot" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MercenaryEquipmentSlots_PlayerItemId",
                table: "MercenaryEquipmentSlots",
                column: "PlayerItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBeastSlots_BeastInstanceId",
                table: "PlayerBeastSlots",
                column: "BeastInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerBeastSlots_PlayerProfileId",
                table: "PlayerBeastSlots",
                column: "PlayerProfileId");

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
                name: "IX_PlayerItemDiscoveries_PlayerId_TemplateId",
                table: "PlayerItemDiscoveries",
                columns: new[] { "PlayerId", "TemplateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerItemDiscoveries_TemplateId",
                table: "PlayerItemDiscoveries",
                column: "TemplateId");

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
                name: "IX_PlayerMercenarySlots_MercenaryInstanceId",
                table: "PlayerMercenarySlots",
                column: "MercenaryInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerMercenarySlots_PlayerProfileId",
                table: "PlayerMercenarySlots",
                column: "PlayerProfileId");

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
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BeastEquipmentSlots");

            migrationBuilder.DropTable(
                name: "DungeonStages");

            migrationBuilder.DropTable(
                name: "ExpeditionStages");

            migrationBuilder.DropTable(
                name: "ItemEffect");

            migrationBuilder.DropTable(
                name: "MercenaryEquipmentSlots");

            migrationBuilder.DropTable(
                name: "PlayerBeastSlots");

            migrationBuilder.DropTable(
                name: "PlayerDungeonAchievements");

            migrationBuilder.DropTable(
                name: "PlayerDungeonProgresses");

            migrationBuilder.DropTable(
                name: "PlayerExpeditionAchievements");

            migrationBuilder.DropTable(
                name: "PlayerExpeditionProgresses");

            migrationBuilder.DropTable(
                name: "PlayerItemDiscoveries");

            migrationBuilder.DropTable(
                name: "PlayerMercenarySlots");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PlayerItems");

            migrationBuilder.DropTable(
                name: "PlayerMonsters");

            migrationBuilder.DropTable(
                name: "DungeonAchievements");

            migrationBuilder.DropTable(
                name: "ExpeditionAchievements");

            migrationBuilder.DropTable(
                name: "PlayerMercenaries");

            migrationBuilder.DropTable(
                name: "ItemTemplates");

            migrationBuilder.DropTable(
                name: "Dungeons");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "MercenaryTemplates");

            migrationBuilder.DropTable(
                name: "MonsterTemplates");

            migrationBuilder.DropTable(
                name: "StatScaling");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToEnti260223 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionProgresses_PlayerExpeditionAchievementSumma~",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionProgresses_AchievementsId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "AchievementsId",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PlayerExpeditionAchievement",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Current",
                table: "PlayerExpeditionAchievement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DifficultyTier",
                table: "PlayerExpeditionAchievement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "PlayerExpeditionAchievement",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerExpeditionProgressId",
                table: "PlayerExpeditionAchievement",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Required",
                table: "PlayerExpeditionAchievement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "PlayerDungeonAchievement",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "DifficultyTier",
                table: "PlayerDungeonAchievement",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionAchievement_PlayerExpeditionProgressId",
                table: "PlayerExpeditionAchievement",
                column: "PlayerExpeditionProgressId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionAchievement_PlayerExpeditionProgresses_Play~",
                table: "PlayerExpeditionAchievement",
                column: "PlayerExpeditionProgressId",
                principalTable: "PlayerExpeditionProgresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerExpeditionAchievement_PlayerExpeditionProgresses_Play~",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropIndex(
                name: "IX_PlayerExpeditionAchievement_PlayerExpeditionProgressId",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "Current",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "DifficultyTier",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "PlayerExpeditionProgressId",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "Required",
                table: "PlayerExpeditionAchievement");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "PlayerDungeonAchievement");

            migrationBuilder.DropColumn(
                name: "DifficultyTier",
                table: "PlayerDungeonAchievement");

            migrationBuilder.AddColumn<Guid>(
                name: "AchievementsId",
                table: "PlayerExpeditionProgresses",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PlayerExpeditionProgresses_AchievementsId",
                table: "PlayerExpeditionProgresses",
                column: "AchievementsId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerExpeditionProgresses_PlayerExpeditionAchievementSumma~",
                table: "PlayerExpeditionProgresses",
                column: "AchievementsId",
                principalTable: "PlayerExpeditionAchievementSummary",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

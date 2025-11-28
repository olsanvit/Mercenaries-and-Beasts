using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ItemEffectUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PlayerMercenaries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PlayerExpeditionProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "PlayerDungeonProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MonsterTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "MercenaryTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ItemTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ItemEffect",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Dungeons",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ItemEffect");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Dungeons");
        }
    }
}

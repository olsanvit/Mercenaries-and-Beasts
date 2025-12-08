using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Players",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "PlayerMercenaries",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "PlayerExpeditionProgresses",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "PlayerDungeonProgresses",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "MonsterTemplates",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "MercenaryTemplates",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Locations",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ItemTemplates",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ItemEffect",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "ExpeditionStages",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "DungeonStages",
                newName: "ImagePromptMeta");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Dungeons",
                newName: "ImagePromptMeta");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Players",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PlayerMercenaries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PlayerExpeditionProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "PlayerDungeonProgresses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "MonsterTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "MercenaryTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ItemTemplates",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ItemEffect",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "ExpeditionStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "DungeonStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Dungeons",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PlayerMercenaries");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PlayerExpeditionProgresses");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "PlayerDungeonProgresses");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "MonsterTemplates");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "MercenaryTemplates");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ItemTemplates");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ItemEffect");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Dungeons");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "Players",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "PlayerMercenaries",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "PlayerExpeditionProgresses",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "PlayerDungeonProgresses",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "MonsterTemplates",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "MercenaryTemplates",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "Locations",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "ItemTemplates",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "ItemEffect",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "ExpeditionStages",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "DungeonStages",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImagePromptMeta",
                table: "Dungeons",
                newName: "ImageUrl");
        }
    }
}

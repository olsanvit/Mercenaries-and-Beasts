using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DungeonAiGenerationCode1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseLevel",
                table: "DungeonStages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "DungeonStages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "DungeonStages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "DungeonStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "DungeonStages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseLevel",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "DungeonStages");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "DungeonStages");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class LocationAiGenerationCode1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BaseLevel",
                table: "ExpeditionStages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "ExpeditionStages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionEn",
                table: "ExpeditionStages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "ExpeditionStages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameEn",
                table: "ExpeditionStages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseLevel",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "DescriptionEn",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "ExpeditionStages");

            migrationBuilder.DropColumn(
                name: "NameEn",
                table: "ExpeditionStages");
        }
    }
}

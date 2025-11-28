using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercenariesAndBeasts.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddElementAndUnlockOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Locations");

            migrationBuilder.AddColumn<string>(
                name: "Element",
                table: "Locations",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UnlockOrder",
                table: "Locations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Element",
                table: "Dungeons",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UnlockOrder",
                table: "Dungeons",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Element",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "UnlockOrder",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "Element",
                table: "Dungeons");

            migrationBuilder.DropColumn(
                name: "UnlockOrder",
                table: "Dungeons");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Locations",
                type: "character varying(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");
        }
    }
}

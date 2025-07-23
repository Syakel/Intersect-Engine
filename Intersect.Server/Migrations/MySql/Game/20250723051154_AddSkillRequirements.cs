using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.MySql.Game
{
    /// <inheritdoc />
    public partial class AddSkillRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillLevelRequired",
                table: "Resources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillRequired",
                table: "Resources",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "SkillLevelRequired",
                table: "Crafts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillRequired",
                table: "Crafts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillLevelRequired",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "SkillRequired",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "SkillLevelRequired",
                table: "Crafts");

            migrationBuilder.DropColumn(
                name: "SkillRequired",
                table: "Crafts");
        }
    }
}

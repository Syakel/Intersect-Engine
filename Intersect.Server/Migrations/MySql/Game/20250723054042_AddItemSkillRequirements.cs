using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.MySql.Game
{
    /// <inheritdoc />
    public partial class AddItemSkillRequirements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkillLevelRequired",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SkillRequired",
                table: "Items",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SkillLevelRequired",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "SkillRequired",
                table: "Items");
        }
    }
}

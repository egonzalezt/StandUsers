using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StandUsers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEnableAttribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "GovCarpetaVerified",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Active",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GovCarpetaVerified",
                table: "Users");
        }
    }
}

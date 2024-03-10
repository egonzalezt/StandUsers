using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StandUsers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetIdentificationNumberAsLong : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "IdentificationNumber",
                table: "Users",
                type: "bigint",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 10);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "IdentificationNumber",
                table: "Users",
                type: "integer",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldMaxLength: 10);
        }
    }
}

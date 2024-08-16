using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceSimulator3.Migrations
{
    /// <inheritdoc />
    public partial class aa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WeaponId",
                table: "WeaponDefence",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WeaponDefence",
                newName: "WeaponId");
        }
    }
}

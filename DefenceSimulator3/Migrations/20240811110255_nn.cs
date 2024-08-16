using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceSimulator3.Migrations
{
    /// <inheritdoc />
    public partial class nn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Threat_OriginId",
                table: "Threat");

            migrationBuilder.DropIndex(
                name: "IX_Threat_WeaponId",
                table: "Threat");

            migrationBuilder.CreateIndex(
                name: "IX_Threat_OriginId",
                table: "Threat",
                column: "OriginId");

            migrationBuilder.CreateIndex(
                name: "IX_Threat_WeaponId",
                table: "Threat",
                column: "WeaponId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Threat_OriginId",
                table: "Threat");

            migrationBuilder.DropIndex(
                name: "IX_Threat_WeaponId",
                table: "Threat");

            migrationBuilder.CreateIndex(
                name: "IX_Threat_OriginId",
                table: "Threat",
                column: "OriginId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Threat_WeaponId",
                table: "Threat",
                column: "WeaponId",
                unique: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DefenceSimulator3.Migrations
{
    /// <inheritdoc />
    public partial class dd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimeToImpact1",
                table: "Threat",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeToImpact1",
                table: "Threat");
        }
    }
}

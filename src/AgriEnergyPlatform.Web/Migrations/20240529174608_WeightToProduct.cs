using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriEnergyPlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class WeightToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsWeight",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsWeight",
                table: "Products");
        }
    }
}

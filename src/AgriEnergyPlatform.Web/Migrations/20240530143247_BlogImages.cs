using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriEnergyPlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class BlogImages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogImageUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Heading1Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Heading2Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Heading3Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Heading4Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Heading5Url = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogImages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogImages");
        }
    }
}

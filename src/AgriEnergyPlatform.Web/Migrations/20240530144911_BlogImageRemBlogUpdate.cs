using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgriEnergyPlatform.Web.Migrations
{
    /// <inheritdoc />
    public partial class BlogImageRemBlogUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogImages");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Blogs",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "BlogImageUrl",
                table: "Blogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content1",
                table: "Blogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Content2",
                table: "Blogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content3",
                table: "Blogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content4",
                table: "Blogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content5",
                table: "Blogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading1",
                table: "Blogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Heading1ImageUrl",
                table: "Blogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading2",
                table: "Blogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading2ImageUrl",
                table: "Blogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading3",
                table: "Blogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading3ImageUrl",
                table: "Blogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading4",
                table: "Blogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading4ImageUrl",
                table: "Blogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading5",
                table: "Blogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Heading5ImageUrl",
                table: "Blogs",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "Blogs",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subtitle",
                table: "Blogs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlogImageUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Content1",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Content2",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Content3",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Content4",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Content5",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading1",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading1ImageUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading2",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading2ImageUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading3",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading3ImageUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading4",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading4ImageUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading5",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Heading5ImageUrl",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Intro",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "Subtitle",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                table: "Blogs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Blogs",
                type: "TEXT",
                maxLength: 10000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "Blogs",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

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
    }
}

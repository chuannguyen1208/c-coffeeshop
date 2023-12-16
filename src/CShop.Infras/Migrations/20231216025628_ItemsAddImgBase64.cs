using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShop.Infras.Migrations
{
    /// <inheritdoc />
    public partial class ItemsAddImgBase64 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImgBase64",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgBase64",
                table: "Items");
        }
    }
}

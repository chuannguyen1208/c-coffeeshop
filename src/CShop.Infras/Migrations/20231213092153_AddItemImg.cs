using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShop.Infras.Migrations
{
    /// <inheritdoc />
    public partial class AddItemImg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Img",
                table: "Items",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Img",
                table: "Items");

            migrationBuilder.AddColumn<string>(
                name: "No",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShop.Infras.Migrations
{
    /// <inheritdoc />
    public partial class AddNoToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "No",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "No",
                table: "Orders");
        }
    }
}

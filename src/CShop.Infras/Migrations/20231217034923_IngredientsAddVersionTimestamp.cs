using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CShop.Infras.Migrations
{
    /// <inheritdoc />
    public partial class IngredientsAddVersionTimestamp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Version",
                table: "Ingredients",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Ingredients");
        }
    }
}

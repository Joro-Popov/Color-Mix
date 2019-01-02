using Microsoft.EntityFrameworkCore.Migrations;

namespace ColorMix.Data.Migrations
{
    public partial class AddShoppingCartItemSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "ShoppingCartItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Size",
                table: "ShoppingCartItems");
        }
    }
}

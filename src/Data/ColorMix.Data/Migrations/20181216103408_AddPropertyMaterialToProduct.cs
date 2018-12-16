using Microsoft.EntityFrameworkCore.Migrations;

namespace ColorMix.Data.Migrations
{
    public partial class AddPropertyMaterialToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material",
                table: "Products");
        }
    }
}

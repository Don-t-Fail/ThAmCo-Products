using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Products.Data.Migrations
{
    public partial class NewBrandAndCategoryObjects : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AvailableProductCount",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AvailableProductCount",
                table: "Brands",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Brands",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AvailableProductCount", "Description" },
                values: new object[] { 3, "Description 1" });

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AvailableProductCount", "Description" },
                values: new object[] { 2, "Description 2" });

            migrationBuilder.UpdateData(
                table: "Brands",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AvailableProductCount", "Description" },
                values: new object[] { 3, "Description 3" });

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 1,
                column: "AvailableProductCount",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 2,
                column: "AvailableProductCount",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Category",
                keyColumn: "Id",
                keyValue: 3,
                column: "AvailableProductCount",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableProductCount",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "AvailableProductCount",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Brands");
        }
    }
}

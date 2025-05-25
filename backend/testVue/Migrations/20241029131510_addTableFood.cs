using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testVue.Migrations
{
    /// <inheritdoc />
    public partial class addTableFood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomizableItem",
                columns: table => new
                {
                    CustomItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomItemPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomizableItem", x => x.CustomItemId);
                });

            migrationBuilder.CreateTable(
                name: "FoodCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "FoodCustomizable",
                columns: table => new
                {
                    FoodCustomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    CustomItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCustomizable", x => x.FoodCustomId);
                });

            migrationBuilder.CreateTable(
                name: "FoodItem",
                columns: table => new
                {
                    FoodItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceListed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PriceCustom = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodItem", x => x.FoodItemId);
                });

            migrationBuilder.CreateTable(
                name: "FoodPriceType",
                columns: table => new
                {
                    PriceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodItemId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodPriceType", x => x.PriceTypeId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomizableItem");

            migrationBuilder.DropTable(
                name: "FoodCategory");

            migrationBuilder.DropTable(
                name: "FoodCustomizable");

            migrationBuilder.DropTable(
                name: "FoodItem");

            migrationBuilder.DropTable(
                name: "FoodPriceType");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace testVue.Migrations
{
    /// <inheritdoc />
    public partial class UpdateColumnFoodItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "FoodItem",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "FoodItem",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "FoodItem");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "FoodItem");
        }
    }
}

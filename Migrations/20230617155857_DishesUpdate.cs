using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JinglePlanner.Migrations
{
    /// <inheritdoc />
    public partial class DishesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Recipe_RecipeId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_RecipeId",
                table: "Dish");

            migrationBuilder.AlterColumn<string>(
                name: "RecipeId",
                table: "Dish",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyName",
                table: "Dish",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "GuestName",
                table: "Dish",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddColumn<int>(
                name: "RecipeId1",
                table: "Dish",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_RecipeId1",
                table: "Dish",
                column: "RecipeId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Recipe_RecipeId1",
                table: "Dish",
                column: "RecipeId1",
                principalTable: "Recipe",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Recipe_RecipeId1",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_RecipeId1",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "RecipeId1",
                table: "Dish");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "Dish",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "PartyName",
                table: "Dish",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GuestName",
                table: "Dish",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Dish_RecipeId",
                table: "Dish",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dish_Recipe_RecipeId",
                table: "Dish",
                column: "RecipeId",
                principalTable: "Recipe",
                principalColumn: "Id");
        }
    }
}

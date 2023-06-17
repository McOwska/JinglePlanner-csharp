using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JinglePlanner.Migrations
{
    /// <inheritdoc />
    public partial class DishUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "guestName",
                table: "Dish",
                newName: "GuestName");

            migrationBuilder.AddColumn<string>(
                name: "GuestAndParty",
                table: "Dish",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PartyName",
                table: "Dish",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuestAndParty",
                table: "Dish");

            migrationBuilder.DropColumn(
                name: "PartyName",
                table: "Dish");

            migrationBuilder.RenameColumn(
                name: "GuestName",
                table: "Dish",
                newName: "guestName");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JinglePlanner.Migrations
{
    /// <inheritdoc />
    public partial class InitialGuests4Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Responsible",
                table: "Guest",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Responsible",
                table: "Guest");
        }
    }
}

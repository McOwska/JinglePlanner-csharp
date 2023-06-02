using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JinglePlanner.Migrations
{
    /// <inheritdoc />
    public partial class InitialGuests3Create : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Guest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Guest",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}

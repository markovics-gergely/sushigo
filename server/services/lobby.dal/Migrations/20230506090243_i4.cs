using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lobby.dal.Migrations
{
    /// <inheritdoc />
    public partial class i4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Lobbies",
                newName: "CreatorUserId");

            migrationBuilder.AddColumn<bool>(
                name: "Ready",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "DeckType",
                table: "Lobbies",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ready",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DeckType",
                table: "Lobbies");

            migrationBuilder.RenameColumn(
                name: "CreatorUserId",
                table: "Lobbies",
                newName: "CreatorId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace lobby.dal.Migrations
{
    /// <inheritdoc />
    public partial class i2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Lobbies_LobbyId",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Decks",
                table: "Decks");

            migrationBuilder.RenameTable(
                name: "Decks",
                newName: "Players");

            migrationBuilder.RenameIndex(
                name: "IX_Decks_LobbyId",
                table: "Players",
                newName: "IX_Players_LobbyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Players",
                table: "Players",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Lobbies_LobbyId",
                table: "Players",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Lobbies_LobbyId",
                table: "Players");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Players",
                table: "Players");

            migrationBuilder.RenameTable(
                name: "Players",
                newName: "Decks");

            migrationBuilder.RenameIndex(
                name: "IX_Players_LobbyId",
                table: "Decks",
                newName: "IX_Decks_LobbyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Decks",
                table: "Decks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Lobbies_LobbyId",
                table: "Decks",
                column: "LobbyId",
                principalTable: "Lobbies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

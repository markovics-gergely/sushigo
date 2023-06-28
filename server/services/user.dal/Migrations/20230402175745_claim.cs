using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.dal.Migrations
{
    /// <inheritdoc />
    public partial class Claim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameClaims",
                table: "AspNetUsers",
                newName: "DeckClaims");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeckClaims",
                table: "AspNetUsers",
                newName: "GameClaims");
        }
    }
}

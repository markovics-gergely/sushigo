using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace game.dal.Migrations
{
    /// <inheritdoc />
    public partial class HandFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Hands_HandId1",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_HandId1",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "HandId1",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "HandId1",
                table: "Players",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_HandId1",
                table: "Players",
                column: "HandId1",
                unique: true,
                filter: "[HandId1] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Hands_HandId1",
                table: "Players",
                column: "HandId1",
                principalTable: "Hands",
                principalColumn: "Id");
        }
    }
}

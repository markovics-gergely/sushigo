using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.dal.Migrations
{
    /// <inheritdoc />
    public partial class i5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActiveLobby",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveLobby",
                table: "AspNetUsers");
        }
    }
}

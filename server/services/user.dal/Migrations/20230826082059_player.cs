using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.dal.Migrations
{
    /// <inheritdoc />
    public partial class player : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ActiveGamePlayer",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveGamePlayer",
                table: "AspNetUsers");
        }
    }
}

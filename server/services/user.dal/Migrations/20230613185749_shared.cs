using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace user.dal.Migrations
{
    /// <inheritdoc />
    public partial class Shared : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Image",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Image");
        }
    }
}

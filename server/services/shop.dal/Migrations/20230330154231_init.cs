using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop.dal.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SushiType = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Id);
                    table.UniqueConstraint("AK_Cards_Type", x => x.Type);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DeckType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.UniqueConstraint("AK_Decks_DeckType", x => x.DeckType);
                });

            migrationBuilder.CreateTable(
                name: "DeckCards",
                columns: table => new
                {
                    CardType = table.Column<int>(type: "int", nullable: false),
                    DeckType = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeckCards", x => new { x.DeckType, x.CardType });
                    table.ForeignKey(
                        name: "FK_DeckCards_Cards_CardType",
                        column: x => x.CardType,
                        principalTable: "Cards",
                        principalColumn: "Type",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeckCards_Decks_DeckType",
                        column: x => x.DeckType,
                        principalTable: "Decks",
                        principalColumn: "DeckType",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "ImagePath", "SushiType", "Type" },
                values: new object[,]
                {
                    { new Guid("0452cd8d-50dd-4e69-b7a8-b75f79264404"), "/cards/Fruit.png", 21, 23 },
                    { new Guid("08aa6a54-4eb2-4a84-8831-a3d343502410"), "/cards/SquidNigiri.png", 0, 2 },
                    { new Guid("1753ee95-fe9f-4cb8-ad1c-48fc64d96b04"), "/cards/Dumpling.png", 5, 6 },
                    { new Guid("1c8caa7d-6e89-432e-ad59-4a775613646b"), "/cards/Wasabi.png", 21, 21 },
                    { new Guid("371ea535-e022-47d0-9f8e-d30d8f072564"), "/cards/GreenTeaIceCream.png", 21, 22 },
                    { new Guid("46cec834-0cd0-452b-a6e7-e112a1ad791c"), "/cards/SalmonNigiri.png", 0, 1 },
                    { new Guid("4a57d23b-aff6-46b2-a99e-aca738c4b091"), "/cards/Spoon.png", 13, 17 },
                    { new Guid("4f3bd105-4daf-4831-a7da-f2173454b4bb"), "/cards/SpecialOrder.png", 13, 18 },
                    { new Guid("70865471-53b2-42dc-b7fb-ba5252dfd6bd"), "/cards/Uramaki.png", 5, 5 },
                    { new Guid("7246741c-0082-431c-9872-42c64e456a49"), "/cards/Menu.png", 13, 15 },
                    { new Guid("751fa42d-9cb9-428e-b074-1ec3ea8ea483"), "/cards/SoySauce.png", 13, 16 },
                    { new Guid("7a42b750-8689-48c1-9205-a55140d66182"), "/cards/Onigiri.png", 5, 9 },
                    { new Guid("8866bdf4-10b8-443e-a3db-2c740ddb13ea"), "/cards/Eel.png", 5, 8 },
                    { new Guid("889d9cf4-41c3-49eb-a651-3d0fc432622d"), "/cards/Tea.png", 13, 20 },
                    { new Guid("8f309897-3131-4365-a6d2-7c3402a7c9ce"), "/cards/Chopsticks.png", 13, 14 },
                    { new Guid("9916f23f-58d2-4f6d-9a71-24c11f949f0f"), "/cards/Sashimi.png", 5, 11 },
                    { new Guid("9d927b2f-9a6a-46b7-b617-a94c6039a539"), "/cards/Pudding.png", 24, 24 },
                    { new Guid("a9987075-f9e7-4375-ac37-7ad2c74bccb7"), "/cards/MisoSoup.png", 5, 10 },
                    { new Guid("ab9acdf6-2fef-4d0b-a563-cbe310cb52c8"), "/cards/Tempura.png", 5, 12 },
                    { new Guid("ccfe3e2b-a7e8-4868-9643-994dba3ce500"), "/cards/Temaki.png", 3, 4 },
                    { new Guid("cf7f13b2-2940-4948-b17a-4c3887978ae5"), "/cards/MakiRoll.png", 3, 3 },
                    { new Guid("d5387ecd-c837-4397-8df7-8d98b5c40b5e"), "/cards/EggNigiri.png", 0, 0 },
                    { new Guid("eb51cd2a-3448-46c4-a9ce-f7accb2e25db"), "/cards/Edamame.png", 5, 7 },
                    { new Guid("f22cf0d5-5f78-445d-a4d2-115982f7fa50"), "/cards/TakeoutBox.png", 13, 19 },
                    { new Guid("fddda130-3cc1-49ca-b57a-29cb74abd9f5"), "/cards/Tofu.png", 13, 13 }
                });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "Id", "DeckType" },
                values: new object[,]
                {
                    { new Guid("19c5ac29-a017-40ce-8c04-183e83187189"), 1 },
                    { new Guid("2bd4c609-5f6b-4976-96f3-a6d3d9f2a0a0"), 3 },
                    { new Guid("443c1263-968c-43d7-a3e3-a2d1b6ae3ec3"), 5 },
                    { new Guid("66ac5c86-2d27-4420-b64c-94a24c27adf9"), 7 },
                    { new Guid("66b81989-410f-42d8-88f1-2cdff94748ab"), 4 },
                    { new Guid("929a7298-d727-4f91-a1d6-129da1d6139a"), 2 },
                    { new Guid("c540adef-e447-4ca7-ac6f-f1cba0fe5851"), 0 },
                    { new Guid("ff44aabc-284b-4359-8e77-d49ac0cb9a1d"), 6 }
                });

            migrationBuilder.InsertData(
                table: "DeckCards",
                columns: new[] { "CardType", "DeckType" },
                values: new object[,]
                {
                    { 0, 0 },
                    { 1, 0 },
                    { 2, 0 },
                    { 3, 0 },
                    { 10, 0 },
                    { 11, 0 },
                    { 12, 0 },
                    { 20, 0 },
                    { 21, 0 },
                    { 22, 0 },
                    { 0, 1 },
                    { 1, 1 },
                    { 2, 1 },
                    { 3, 1 },
                    { 6, 1 },
                    { 11, 1 },
                    { 12, 1 },
                    { 14, 1 },
                    { 21, 1 },
                    { 24, 1 },
                    { 0, 2 },
                    { 1, 2 },
                    { 2, 2 },
                    { 4, 2 },
                    { 6, 2 },
                    { 12, 2 },
                    { 13, 2 },
                    { 15, 2 },
                    { 21, 2 },
                    { 22, 2 },
                    { 0, 3 },
                    { 1, 3 },
                    { 2, 3 },
                    { 4, 3 },
                    { 9, 3 },
                    { 11, 3 },
                    { 13, 3 },
                    { 17, 3 },
                    { 19, 3 },
                    { 23, 3 },
                    { 0, 4 },
                    { 1, 4 },
                    { 2, 4 },
                    { 5, 4 },
                    { 6, 4 },
                    { 7, 4 },
                    { 9, 4 },
                    { 18, 4 },
                    { 20, 4 },
                    { 22, 4 },
                    { 0, 5 },
                    { 1, 5 },
                    { 2, 5 },
                    { 4, 5 },
                    { 8, 5 },
                    { 10, 5 },
                    { 13, 5 },
                    { 16, 5 },
                    { 17, 5 },
                    { 24, 5 },
                    { 0, 6 },
                    { 1, 6 },
                    { 2, 6 },
                    { 3, 6 },
                    { 6, 6 },
                    { 8, 6 },
                    { 12, 6 },
                    { 14, 6 },
                    { 17, 6 },
                    { 22, 6 },
                    { 0, 7 },
                    { 1, 7 },
                    { 2, 7 },
                    { 5, 7 },
                    { 9, 7 },
                    { 10, 7 },
                    { 13, 7 },
                    { 15, 7 },
                    { 18, 7 },
                    { 23, 7 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeckCards_CardType",
                table: "DeckCards",
                column: "CardType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeckCards");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Decks");
        }
    }
}

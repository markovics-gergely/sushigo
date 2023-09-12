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
                    DeckType = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<long>(type: "bigint", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    DeckType = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    { new Guid("0476464f-5e40-47b1-a5f1-f0b9e3903579"), "/cards/Wasabi.png", 3, 21 },
                    { new Guid("065611f2-5501-4c02-b4c4-9c61169cbc66"), "/cards/TakeoutBox.png", 3, 19 },
                    { new Guid("16e1545b-4b66-4abf-b008-2be9bd1e3fec"), "/cards/SoySauce.png", 3, 16 },
                    { new Guid("3d270093-98d6-4f6c-85e2-ec6f48790fc9"), "/cards/Onigiri.png", 2, 9 },
                    { new Guid("3f4aaf5c-ff98-40ea-8e4f-e180a138388c"), "/cards/Spoon.png", 3, 17 },
                    { new Guid("3f92b303-b023-496e-8795-217274917672"), "/cards/Edamame.png", 2, 7 },
                    { new Guid("59f7cdbd-fa48-4348-ade6-45f8923d84f3"), "/cards/SquidNigiri.png", 0, 2 },
                    { new Guid("6470ca1d-25c7-4144-9dd4-276d229a0758"), "/cards/Chopsticks.png", 3, 14 },
                    { new Guid("6c9eba45-abbc-4eef-8959-df76bb1f94b9"), "/cards/Temaki.png", 1, 4 },
                    { new Guid("72ad82f6-0a98-4c8c-8649-38032dc0721b"), "/cards/Fruit.png", 4, 23 },
                    { new Guid("7335fa88-94d4-4997-8bb1-89555cb911bb"), "/cards/Tempura.png", 2, 12 },
                    { new Guid("73527941-b903-4f2e-9469-04d13ad37e34"), "/cards/EggNigiri.png", 0, 0 },
                    { new Guid("85ebefd3-ed02-45be-982c-696b98084136"), "/cards/Dumpling.png", 2, 6 },
                    { new Guid("95ee587a-42fc-44ee-9433-4a9084148c1d"), "/cards/SpecialOrder.png", 3, 18 },
                    { new Guid("986eeaf3-fd9f-41d6-a338-6572030a3f3b"), "/cards/Menu.png", 3, 15 },
                    { new Guid("b073befa-621b-4184-b44f-5643d4d12ab2"), "/cards/Eel.png", 2, 8 },
                    { new Guid("bb062eac-fff6-495c-84df-c181925e92a9"), "/cards/GreenTeaIceCream.png", 4, 22 },
                    { new Guid("c814c368-f859-455a-b7d2-8a47debcf172"), "/cards/Tofu.png", 2, 13 },
                    { new Guid("d07f87ff-022a-4803-8742-725616004d9d"), "/cards/MisoSoup.png", 2, 10 },
                    { new Guid("d0abb279-d897-40e0-8624-4ebe12fd6e52"), "/cards/Pudding.png", 4, 24 },
                    { new Guid("d1ba5bf8-31e5-4624-a5e3-a1757bb8d6bb"), "/cards/Tea.png", 3, 20 },
                    { new Guid("d842190a-183a-4c35-a88d-8256753f1eca"), "/cards/SalmonNigiri.png", 0, 1 },
                    { new Guid("e92bf631-7312-4f34-999d-9aaf415323bc"), "/cards/Sashimi.png", 2, 11 },
                    { new Guid("ef0b02dd-944a-4359-ad36-0b5280b77ca3"), "/cards/Uramaki.png", 1, 5 },
                    { new Guid("f7d4abd9-b382-47a6-9e76-3eba0b606c6d"), "/cards/MakiRoll.png", 1, 3 }
                });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "Id", "Cost", "DeckType", "ImagePath" },
                values: new object[,]
                {
                    { new Guid("3177fa4a-f09f-4083-a6f4-d5e5d2f2fa68"), 50L, 4, "/decks/PointsPlatter.png" },
                    { new Guid("878dcfaa-738a-4c7b-bd98-5045a37a8932"), 50L, 3, "/decks/MasterMenu.png" },
                    { new Guid("9c5c6df6-2eac-487f-9f66-60a7f686e823"), 50L, 7, "/decks/DinnerForTwo.png" },
                    { new Guid("9fe2a730-f7f1-4ca7-a6fe-5d3ce7c6885c"), 50L, 6, "/decks/BigBanquet.png" },
                    { new Guid("a743fb13-aad3-4459-af8d-8c96f560b5c4"), 50L, 5, "/decks/CutThroatCombo.png" },
                    { new Guid("e6ee5f1c-f2aa-4ab2-981f-5c5544e58cec"), 50L, 2, "/decks/PartySampler.png" },
                    { new Guid("f0f67a12-5f98-4ba6-8031-c20f3193a326"), 50L, 0, "/decks/MyFirstMeal.png" },
                    { new Guid("fe484141-ee6d-4029-8498-5d3cfac9adb0"), 50L, 1, "/decks/SushiGo.png" }
                });

            migrationBuilder.InsertData(
                table: "DeckCards",
                columns: new[] { "CardType", "DeckType", "Id" },
                values: new object[,]
                {
                    { 0, 0, new Guid("b10fa3c3-e18c-4541-9b46-127ea9a2f924") },
                    { 1, 0, new Guid("e172b89e-80f1-4c23-82e8-1882e2aa48c1") },
                    { 2, 0, new Guid("89b108c9-8606-4cd6-a0a1-360ef51e4958") },
                    { 3, 0, new Guid("b3860012-917b-4091-8f3a-8c1a7a0a33ea") },
                    { 10, 0, new Guid("16101299-5b37-49a1-b410-c535d248d4bb") },
                    { 11, 0, new Guid("de55b2d1-939b-469c-b9e9-fd6c07fabb3a") },
                    { 12, 0, new Guid("25127479-6d90-40eb-867d-987fdec05451") },
                    { 20, 0, new Guid("38140c9b-6d67-486b-81e7-49622965cb97") },
                    { 21, 0, new Guid("e69c8efe-faf1-4e31-8912-261613b52524") },
                    { 22, 0, new Guid("7ebd5108-daf0-4938-9f85-319b131d8f31") },
                    { 0, 1, new Guid("7b9695dc-e405-4b3c-8dc3-53a7e934232c") },
                    { 1, 1, new Guid("bd6731e0-fb90-4af3-a661-094f095c015a") },
                    { 2, 1, new Guid("e2a37017-af7c-41ab-8c1b-59b7776c2a95") },
                    { 3, 1, new Guid("506b3f48-23c8-44d0-a743-d4b2e4f2f190") },
                    { 6, 1, new Guid("cd28da30-75b1-4275-8fa6-47f282a7d6c1") },
                    { 11, 1, new Guid("e4b96b81-ec0c-4c9a-9242-52fcb5022d7c") },
                    { 12, 1, new Guid("1bda0dba-4763-459c-b842-2eacdf14979a") },
                    { 14, 1, new Guid("ddcb803e-0cc8-409c-b95a-804f3fc18636") },
                    { 21, 1, new Guid("7876ef65-7c09-4c09-b3a2-8224a9f5bc1b") },
                    { 24, 1, new Guid("97265733-5f89-4235-ae30-2f02476842b6") },
                    { 0, 2, new Guid("29955a69-e3a2-4dc7-96b8-07c83439a086") },
                    { 1, 2, new Guid("818e8879-3abc-4c58-8a43-0def6f69c1c6") },
                    { 2, 2, new Guid("11eb04e1-5261-45aa-8f0d-07611b46643f") },
                    { 4, 2, new Guid("3fa0f61b-1b0c-4375-8134-8df2ba437a9e") },
                    { 6, 2, new Guid("dbb6de3a-c106-4744-bb2c-a0e1f9588ce7") },
                    { 12, 2, new Guid("26c98834-6e65-4571-9b15-0a782e8fb2ec") },
                    { 13, 2, new Guid("c099268a-ca13-49f8-8677-96e8628467b6") },
                    { 15, 2, new Guid("418bbf4d-c7fa-4604-acb1-9f2bf40342ad") },
                    { 21, 2, new Guid("c2459e06-d2af-4a0a-8997-62da81d457b3") },
                    { 22, 2, new Guid("76719c84-f898-4d1f-a1cc-9220bb7121ee") },
                    { 0, 3, new Guid("0e45bf7d-4fa4-464e-ab75-e8bf9084fbe4") },
                    { 1, 3, new Guid("8fd5251b-fd93-4736-b579-0656499110bb") },
                    { 2, 3, new Guid("c6588c8c-314f-4fd9-8ca0-2e2531649d72") },
                    { 4, 3, new Guid("e0e09874-5e65-43da-acc4-6a5b78156a7f") },
                    { 9, 3, new Guid("99bcf316-849c-4b57-8fa3-03720113d4d1") },
                    { 11, 3, new Guid("d80712f6-61e8-479e-b767-43a709b42972") },
                    { 13, 3, new Guid("d67ae5aa-deb1-4e91-a8cb-563e3afd03e3") },
                    { 17, 3, new Guid("68629c99-898e-44c3-a00f-129bbcd27f6e") },
                    { 19, 3, new Guid("53507d9c-00fc-4780-ab7e-1a2be413d110") },
                    { 23, 3, new Guid("67bc3585-e9a5-446c-bbc2-41e547168aea") },
                    { 0, 4, new Guid("2819e144-923e-493d-997e-ffe36c600d20") },
                    { 1, 4, new Guid("53c79d63-0082-4d67-81e6-6663425d1543") },
                    { 2, 4, new Guid("44a09e11-a43b-440d-a445-d2b04b07231a") },
                    { 5, 4, new Guid("d6975f70-ebf2-49e0-bae9-8b841b705d56") },
                    { 6, 4, new Guid("29758d4c-bedd-405d-bdf1-e2123b2ed188") },
                    { 7, 4, new Guid("51333131-38f6-4f38-bab0-007bcc687157") },
                    { 9, 4, new Guid("f033e280-4bef-4e37-a322-56feb94e10a7") },
                    { 18, 4, new Guid("a6db99bb-b698-494b-8d51-72b46ad42d38") },
                    { 20, 4, new Guid("54d558a5-2e46-4518-ba3e-5bba11328b85") },
                    { 22, 4, new Guid("a1223b5a-5d9a-4201-98bb-17ee3e4c424c") },
                    { 0, 5, new Guid("04bfee6e-4965-4bee-bf1e-9d7a8e1aa2d0") },
                    { 1, 5, new Guid("ffd98ad0-1582-4a13-b74f-fc831cb73bf4") },
                    { 2, 5, new Guid("49cbb2b3-f9f5-4f7e-b783-22b12b0ede04") },
                    { 4, 5, new Guid("fb657a6f-9f2f-4e86-b6a3-1f907c221e1d") },
                    { 8, 5, new Guid("47273c8a-b174-42a5-9deb-352070f1cd52") },
                    { 10, 5, new Guid("3a9879ba-e9c9-4138-af55-82182db4e7e1") },
                    { 13, 5, new Guid("56621d94-53a2-4988-afb1-e5bf4ad8d6f7") },
                    { 16, 5, new Guid("85ca961a-8f86-4e07-a81d-e364c95ba269") },
                    { 17, 5, new Guid("7c964677-7c22-4bf3-8c91-3b3e6845db00") },
                    { 24, 5, new Guid("24233d86-fc58-461c-9c75-5bd19428a9a0") },
                    { 0, 6, new Guid("56e6df35-37a2-449e-8392-85493bdcffe2") },
                    { 1, 6, new Guid("b38253c2-e2e6-4dd4-8438-90ff97d7d9a2") },
                    { 2, 6, new Guid("3860454b-9107-4ede-ae42-53c5c1afb4af") },
                    { 3, 6, new Guid("68511fff-4651-4c55-b9c6-ee7f1eb895c5") },
                    { 6, 6, new Guid("9d8b75c7-71c8-4623-bf51-d91f10cfb2e7") },
                    { 8, 6, new Guid("0552524f-4496-4808-b21c-2168f55e7071") },
                    { 12, 6, new Guid("2b7f6cf5-4300-4baa-92ee-5e57c51855e0") },
                    { 14, 6, new Guid("1d95e7f5-7f69-48c1-b3d9-4641ce7c1f05") },
                    { 17, 6, new Guid("4f8ed8af-69dd-40b4-bc6b-c6e88212107d") },
                    { 22, 6, new Guid("c979c17c-b914-4554-862c-4a29dc8600b6") },
                    { 0, 7, new Guid("9c12af39-58b0-4d23-8b1d-afa43f203af3") },
                    { 1, 7, new Guid("0591c3ef-7e0c-473f-ab12-5a58045bd9b0") },
                    { 2, 7, new Guid("b03da0c2-faa9-46de-84db-ab8945f24387") },
                    { 5, 7, new Guid("bfe8ecaf-ca47-465a-80c5-1f7669d4e334") },
                    { 9, 7, new Guid("2ea5c1aa-7280-4998-bf41-d4dafb264d30") },
                    { 10, 7, new Guid("4f29b96c-db3b-49c9-a165-399407b83018") },
                    { 13, 7, new Guid("b3b89ae2-c696-4669-8775-253037582079") },
                    { 15, 7, new Guid("da29d6d8-c822-470a-8e41-11ec67409d13") },
                    { 18, 7, new Guid("af6f52b2-637e-4de2-909c-6569850dbe04") },
                    { 23, 7, new Guid("7065fc50-e39e-4ddb-877e-27292a03cc66") }
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

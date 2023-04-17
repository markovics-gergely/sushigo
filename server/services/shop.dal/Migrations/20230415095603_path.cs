using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop.dal.Migrations
{
    /// <inheritdoc />
    public partial class path : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("02e9886f-045b-416f-b7a4-ab47e5ae82a5"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("0382c0c6-8057-4fe3-bbc4-fdfa06090def"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("0dce8df4-f5cc-479a-976c-2ab8d97a465a"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("1de8b64f-7f49-4b8f-a5db-4f9fd2c7eaa7"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("26b8321c-9417-4d83-b43c-76f64200e496"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("37f4948a-8183-43df-86f8-6726b546ed2c"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("4fe311d7-6c90-4a50-ac50-e20645580e98"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("5a3356f1-84d7-4fa6-9359-361bebf5f1e8"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("6affa9da-131e-486c-8bef-d66f9bf8d773"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("6b7e9cd0-8c37-446d-a925-e2c550d76d51"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("746b9923-2e9c-456d-9774-c45b9da9c930"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("8e276ae4-0111-4867-a170-ddeb4cfa8cb4"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("abadaac9-af83-4b2f-b3b6-14b9cabde229"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b3da37f3-353d-4ac9-b99e-fa0b847a21f2"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b78680d2-b767-4b09-bd45-78dfec5bc4fe"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b9e9bb44-5d49-4335-b2c1-8a6b78a711ff"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("bbbd2b6e-a4f7-4d6a-a4eb-5d444c9b46b3"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("bd329baf-9496-4385-96c4-c28e38cf9938"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("c0484eeb-f762-4d51-85a3-de3a2fde5014"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("c7375ade-58d6-4e35-b577-a975df77e560"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("cb037c96-ade7-40de-ae0c-3e777ad08d96"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("ce09447b-1d60-47bb-996a-50404e2453ba"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("d49a8542-74c5-4ac3-afce-53142500af57"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("f63a7fc1-bf86-46aa-ba0e-8171443654bd"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("fb615441-e3e0-4a0b-b0ad-2dee6d8f23e8"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("2f4052ef-896e-460a-9c24-9824ba39c16b"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("485ca349-b75c-4a03-bff3-7f7a48c85327"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("54b50c40-d4d6-464c-b030-688c1f901125"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("6183eba4-2a64-42fc-85fe-acbd61fde712"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("6a29d35d-3e8f-4800-8895-9f596d0f96d5"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("7bf5078e-6abb-4477-b8d8-3b97a4fdd060"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("abdfdf36-a01d-4e0c-addb-e92c17fb36b7"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("cf429547-d511-4836-8385-8de7d96d8193"));

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Decks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "ImagePath", "SushiType", "Type" },
                values: new object[,]
                {
                    { new Guid("000ed01f-8076-477a-b6c9-6b904fd1b72f"), "/cards/Fruit.png", 21, 23 },
                    { new Guid("0ecc53be-db3f-434b-8750-f0f7b108e869"), "/cards/Sashimi.png", 5, 11 },
                    { new Guid("1d61c7ea-faf7-47e1-8fac-b67fe846af23"), "/cards/Edamame.png", 5, 7 },
                    { new Guid("21b94c81-0278-4b6a-a066-ce5a5b195e4b"), "/cards/Wasabi.png", 21, 21 },
                    { new Guid("259241c4-10cc-4279-b520-c0eff1d04d21"), "/cards/Tofu.png", 13, 13 },
                    { new Guid("2b73a059-b15a-4eb1-b88a-1275ba85d335"), "/cards/EggNigiri.png", 0, 0 },
                    { new Guid("38705911-6b40-4a9a-9413-0f04a0d995d5"), "/cards/Menu.png", 13, 15 },
                    { new Guid("5e14949e-1ccf-47b2-8a86-888e52a0275c"), "/cards/Temaki.png", 3, 4 },
                    { new Guid("6656dc8f-1b2d-4fc2-8bbb-5d7d3cf7d5b2"), "/cards/SquidNigiri.png", 0, 2 },
                    { new Guid("7580528f-3851-4ffd-b309-9f13d510392b"), "/cards/MakiRoll.png", 3, 3 },
                    { new Guid("7ab7c652-ef5a-48e3-98ce-06b6a5bc8ef8"), "/cards/Spoon.png", 13, 17 },
                    { new Guid("85d0c512-abe6-4c32-936f-8ca635375861"), "/cards/Eel.png", 5, 8 },
                    { new Guid("9b51b684-6b00-49ab-b9b8-e9ef98aa2234"), "/cards/Onigiri.png", 5, 9 },
                    { new Guid("a73c066d-3837-4096-98b3-b7418bbd0023"), "/cards/Tea.png", 13, 20 },
                    { new Guid("a7ce3912-4ea2-4793-b6c0-d098233ed925"), "/cards/SoySauce.png", 13, 16 },
                    { new Guid("b08938ca-b3d3-42b5-ae60-e27fdec1ad7b"), "/cards/Dumpling.png", 5, 6 },
                    { new Guid("b09141ed-73ea-429d-a55e-bdbf81367a52"), "/cards/GreenTeaIceCream.png", 21, 22 },
                    { new Guid("b5aaa6fb-e8ab-492b-99d7-a8bb31a2e0d0"), "/cards/MisoSoup.png", 5, 10 },
                    { new Guid("b6e25b27-e3fa-41ca-9683-8b1ab99f4ea4"), "/cards/Uramaki.png", 5, 5 },
                    { new Guid("ca74570c-7675-4cd3-ad20-1aee3c4fcf80"), "/cards/Chopsticks.png", 13, 14 },
                    { new Guid("cd6c5a6e-3f5f-48bf-8b2e-272435b6760d"), "/cards/Pudding.png", 24, 24 },
                    { new Guid("dda78d02-3b6b-4fef-9de8-0b20d20ba72a"), "/cards/TakeoutBox.png", 13, 19 },
                    { new Guid("df9d0598-6c7b-47b5-82eb-e6ed459c6272"), "/cards/SalmonNigiri.png", 0, 1 },
                    { new Guid("e9a85b71-5077-4723-930e-4664933584cf"), "/cards/SpecialOrder.png", 13, 18 },
                    { new Guid("f64687a8-a874-4b4e-8956-96fc84b13b82"), "/cards/Tempura.png", 5, 12 }
                });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "Id", "Cost", "DeckType", "ImagePath" },
                values: new object[,]
                {
                    { new Guid("0546c1d2-5b4d-4e51-940f-8a5e93d7dd79"), 50L, 6, "/decks/BigBanquet.png" },
                    { new Guid("47998cfe-61b6-4bbf-9035-14d2aa0c5332"), 50L, 3, "/decks/MasterMenu.png" },
                    { new Guid("70ba49e2-3d0c-4f84-a559-ef7c1dd3af6b"), 50L, 4, "/decks/PointsPlatter.png" },
                    { new Guid("7c74074d-ab6c-41a3-87b2-19078174423b"), 50L, 2, "/decks/PartySampler.png" },
                    { new Guid("9583ddba-7c9b-45d2-a921-8d394f98363f"), 50L, 0, "/decks/MyFirstMeal.png" },
                    { new Guid("c8f2b614-5330-451e-a3e7-13ad7ebc98a0"), 50L, 1, "/decks/SushiGo.png" },
                    { new Guid("e265f7fa-a62f-4b32-888a-ed891f1b71b2"), 50L, 7, "/decks/DinnerForTwo.png" },
                    { new Guid("e4c9951f-f371-405b-9102-9a66fb62f96c"), 50L, 5, "/decks/CutThroatCombo.png" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("000ed01f-8076-477a-b6c9-6b904fd1b72f"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("0ecc53be-db3f-434b-8750-f0f7b108e869"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("1d61c7ea-faf7-47e1-8fac-b67fe846af23"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("21b94c81-0278-4b6a-a066-ce5a5b195e4b"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("259241c4-10cc-4279-b520-c0eff1d04d21"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("2b73a059-b15a-4eb1-b88a-1275ba85d335"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("38705911-6b40-4a9a-9413-0f04a0d995d5"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("5e14949e-1ccf-47b2-8a86-888e52a0275c"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("6656dc8f-1b2d-4fc2-8bbb-5d7d3cf7d5b2"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("7580528f-3851-4ffd-b309-9f13d510392b"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("7ab7c652-ef5a-48e3-98ce-06b6a5bc8ef8"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("85d0c512-abe6-4c32-936f-8ca635375861"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("9b51b684-6b00-49ab-b9b8-e9ef98aa2234"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("a73c066d-3837-4096-98b3-b7418bbd0023"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("a7ce3912-4ea2-4793-b6c0-d098233ed925"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b08938ca-b3d3-42b5-ae60-e27fdec1ad7b"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b09141ed-73ea-429d-a55e-bdbf81367a52"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b5aaa6fb-e8ab-492b-99d7-a8bb31a2e0d0"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("b6e25b27-e3fa-41ca-9683-8b1ab99f4ea4"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("ca74570c-7675-4cd3-ad20-1aee3c4fcf80"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("cd6c5a6e-3f5f-48bf-8b2e-272435b6760d"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("dda78d02-3b6b-4fef-9de8-0b20d20ba72a"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("df9d0598-6c7b-47b5-82eb-e6ed459c6272"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("e9a85b71-5077-4723-930e-4664933584cf"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("f64687a8-a874-4b4e-8956-96fc84b13b82"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("0546c1d2-5b4d-4e51-940f-8a5e93d7dd79"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("47998cfe-61b6-4bbf-9035-14d2aa0c5332"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("70ba49e2-3d0c-4f84-a559-ef7c1dd3af6b"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("7c74074d-ab6c-41a3-87b2-19078174423b"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("9583ddba-7c9b-45d2-a921-8d394f98363f"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("c8f2b614-5330-451e-a3e7-13ad7ebc98a0"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("e265f7fa-a62f-4b32-888a-ed891f1b71b2"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("e4c9951f-f371-405b-9102-9a66fb62f96c"));

            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Decks");

            migrationBuilder.InsertData(
                table: "Cards",
                columns: new[] { "Id", "ImagePath", "SushiType", "Type" },
                values: new object[,]
                {
                    { new Guid("02e9886f-045b-416f-b7a4-ab47e5ae82a5"), "/cards/Chopsticks.png", 13, 14 },
                    { new Guid("0382c0c6-8057-4fe3-bbc4-fdfa06090def"), "/cards/Spoon.png", 13, 17 },
                    { new Guid("0dce8df4-f5cc-479a-976c-2ab8d97a465a"), "/cards/Tofu.png", 13, 13 },
                    { new Guid("1de8b64f-7f49-4b8f-a5db-4f9fd2c7eaa7"), "/cards/SquidNigiri.png", 0, 2 },
                    { new Guid("26b8321c-9417-4d83-b43c-76f64200e496"), "/cards/Tea.png", 13, 20 },
                    { new Guid("37f4948a-8183-43df-86f8-6726b546ed2c"), "/cards/Tempura.png", 5, 12 },
                    { new Guid("4fe311d7-6c90-4a50-ac50-e20645580e98"), "/cards/Pudding.png", 24, 24 },
                    { new Guid("5a3356f1-84d7-4fa6-9359-361bebf5f1e8"), "/cards/Wasabi.png", 21, 21 },
                    { new Guid("6affa9da-131e-486c-8bef-d66f9bf8d773"), "/cards/Sashimi.png", 5, 11 },
                    { new Guid("6b7e9cd0-8c37-446d-a925-e2c550d76d51"), "/cards/SpecialOrder.png", 13, 18 },
                    { new Guid("746b9923-2e9c-456d-9774-c45b9da9c930"), "/cards/Onigiri.png", 5, 9 },
                    { new Guid("8e276ae4-0111-4867-a170-ddeb4cfa8cb4"), "/cards/MisoSoup.png", 5, 10 },
                    { new Guid("abadaac9-af83-4b2f-b3b6-14b9cabde229"), "/cards/Menu.png", 13, 15 },
                    { new Guid("b3da37f3-353d-4ac9-b99e-fa0b847a21f2"), "/cards/SoySauce.png", 13, 16 },
                    { new Guid("b78680d2-b767-4b09-bd45-78dfec5bc4fe"), "/cards/TakeoutBox.png", 13, 19 },
                    { new Guid("b9e9bb44-5d49-4335-b2c1-8a6b78a711ff"), "/cards/Uramaki.png", 5, 5 },
                    { new Guid("bbbd2b6e-a4f7-4d6a-a4eb-5d444c9b46b3"), "/cards/MakiRoll.png", 3, 3 },
                    { new Guid("bd329baf-9496-4385-96c4-c28e38cf9938"), "/cards/SalmonNigiri.png", 0, 1 },
                    { new Guid("c0484eeb-f762-4d51-85a3-de3a2fde5014"), "/cards/EggNigiri.png", 0, 0 },
                    { new Guid("c7375ade-58d6-4e35-b577-a975df77e560"), "/cards/Fruit.png", 21, 23 },
                    { new Guid("cb037c96-ade7-40de-ae0c-3e777ad08d96"), "/cards/Dumpling.png", 5, 6 },
                    { new Guid("ce09447b-1d60-47bb-996a-50404e2453ba"), "/cards/Eel.png", 5, 8 },
                    { new Guid("d49a8542-74c5-4ac3-afce-53142500af57"), "/cards/Edamame.png", 5, 7 },
                    { new Guid("f63a7fc1-bf86-46aa-ba0e-8171443654bd"), "/cards/Temaki.png", 3, 4 },
                    { new Guid("fb615441-e3e0-4a0b-b0ad-2dee6d8f23e8"), "/cards/GreenTeaIceCream.png", 21, 22 }
                });

            migrationBuilder.InsertData(
                table: "Decks",
                columns: new[] { "Id", "Cost", "DeckType" },
                values: new object[,]
                {
                    { new Guid("2f4052ef-896e-460a-9c24-9824ba39c16b"), 50L, 1 },
                    { new Guid("485ca349-b75c-4a03-bff3-7f7a48c85327"), 50L, 5 },
                    { new Guid("54b50c40-d4d6-464c-b030-688c1f901125"), 50L, 0 },
                    { new Guid("6183eba4-2a64-42fc-85fe-acbd61fde712"), 50L, 3 },
                    { new Guid("6a29d35d-3e8f-4800-8895-9f596d0f96d5"), 50L, 6 },
                    { new Guid("7bf5078e-6abb-4477-b8d8-3b97a4fdd060"), 50L, 2 },
                    { new Guid("abdfdf36-a01d-4e0c-addb-e92c17fb36b7"), 50L, 4 },
                    { new Guid("cf429547-d511-4836-8385-8de7d96d8193"), 50L, 7 }
                });
        }
    }
}

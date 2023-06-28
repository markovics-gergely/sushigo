using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shop.dal.Migrations
{
    /// <inheritdoc />
    public partial class Cost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("0452cd8d-50dd-4e69-b7a8-b75f79264404"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("08aa6a54-4eb2-4a84-8831-a3d343502410"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("1753ee95-fe9f-4cb8-ad1c-48fc64d96b04"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("1c8caa7d-6e89-432e-ad59-4a775613646b"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("371ea535-e022-47d0-9f8e-d30d8f072564"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("46cec834-0cd0-452b-a6e7-e112a1ad791c"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("4a57d23b-aff6-46b2-a99e-aca738c4b091"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("4f3bd105-4daf-4831-a7da-f2173454b4bb"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("70865471-53b2-42dc-b7fb-ba5252dfd6bd"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("7246741c-0082-431c-9872-42c64e456a49"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("751fa42d-9cb9-428e-b074-1ec3ea8ea483"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("7a42b750-8689-48c1-9205-a55140d66182"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("8866bdf4-10b8-443e-a3db-2c740ddb13ea"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("889d9cf4-41c3-49eb-a651-3d0fc432622d"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("8f309897-3131-4365-a6d2-7c3402a7c9ce"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("9916f23f-58d2-4f6d-9a71-24c11f949f0f"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("9d927b2f-9a6a-46b7-b617-a94c6039a539"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("a9987075-f9e7-4375-ac37-7ad2c74bccb7"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("ab9acdf6-2fef-4d0b-a563-cbe310cb52c8"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("ccfe3e2b-a7e8-4868-9643-994dba3ce500"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("cf7f13b2-2940-4948-b17a-4c3887978ae5"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("d5387ecd-c837-4397-8df7-8d98b5c40b5e"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("eb51cd2a-3448-46c4-a9ce-f7accb2e25db"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("f22cf0d5-5f78-445d-a4d2-115982f7fa50"));

            migrationBuilder.DeleteData(
                table: "Cards",
                keyColumn: "Id",
                keyValue: new Guid("fddda130-3cc1-49ca-b57a-29cb74abd9f5"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("19c5ac29-a017-40ce-8c04-183e83187189"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("2bd4c609-5f6b-4976-96f3-a6d3d9f2a0a0"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("443c1263-968c-43d7-a3e3-a2d1b6ae3ec3"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("66ac5c86-2d27-4420-b64c-94a24c27adf9"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("66b81989-410f-42d8-88f1-2cdff94748ab"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("929a7298-d727-4f91-a1d6-129da1d6139a"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("c540adef-e447-4ca7-ac6f-f1cba0fe5851"));

            migrationBuilder.DeleteData(
                table: "Decks",
                keyColumn: "Id",
                keyValue: new Guid("ff44aabc-284b-4359-8e77-d49ac0cb9a1d"));

            migrationBuilder.AddColumn<long>(
                name: "Cost",
                table: "Decks",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Decks");

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
        }
    }
}

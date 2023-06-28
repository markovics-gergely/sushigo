﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using shop.dal;

#nullable disable

namespace shop.dal.Migrations
{
    [DbContext(typeof(ShopDbContext))]
    [Migration("20230330154231_init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("shop.dal.Domain.Card", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SushiType")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Cards");

                    b.HasData(
                        new
                        {
                            Id = new Guid("d5387ecd-c837-4397-8df7-8d98b5c40b5e"),
                            ImagePath = "/cards/EggNigiri.png",
                            SushiType = 0,
                            Type = 0
                        },
                        new
                        {
                            Id = new Guid("46cec834-0cd0-452b-a6e7-e112a1ad791c"),
                            ImagePath = "/cards/SalmonNigiri.png",
                            SushiType = 0,
                            Type = 1
                        },
                        new
                        {
                            Id = new Guid("08aa6a54-4eb2-4a84-8831-a3d343502410"),
                            ImagePath = "/cards/SquidNigiri.png",
                            SushiType = 0,
                            Type = 2
                        },
                        new
                        {
                            Id = new Guid("cf7f13b2-2940-4948-b17a-4c3887978ae5"),
                            ImagePath = "/cards/MakiRoll.png",
                            SushiType = 3,
                            Type = 3
                        },
                        new
                        {
                            Id = new Guid("ccfe3e2b-a7e8-4868-9643-994dba3ce500"),
                            ImagePath = "/cards/Temaki.png",
                            SushiType = 3,
                            Type = 4
                        },
                        new
                        {
                            Id = new Guid("70865471-53b2-42dc-b7fb-ba5252dfd6bd"),
                            ImagePath = "/cards/Uramaki.png",
                            SushiType = 5,
                            Type = 5
                        },
                        new
                        {
                            Id = new Guid("1753ee95-fe9f-4cb8-ad1c-48fc64d96b04"),
                            ImagePath = "/cards/Dumpling.png",
                            SushiType = 5,
                            Type = 6
                        },
                        new
                        {
                            Id = new Guid("eb51cd2a-3448-46c4-a9ce-f7accb2e25db"),
                            ImagePath = "/cards/Edamame.png",
                            SushiType = 5,
                            Type = 7
                        },
                        new
                        {
                            Id = new Guid("8866bdf4-10b8-443e-a3db-2c740ddb13ea"),
                            ImagePath = "/cards/Eel.png",
                            SushiType = 5,
                            Type = 8
                        },
                        new
                        {
                            Id = new Guid("7a42b750-8689-48c1-9205-a55140d66182"),
                            ImagePath = "/cards/Onigiri.png",
                            SushiType = 5,
                            Type = 9
                        },
                        new
                        {
                            Id = new Guid("a9987075-f9e7-4375-ac37-7ad2c74bccb7"),
                            ImagePath = "/cards/MisoSoup.png",
                            SushiType = 5,
                            Type = 10
                        },
                        new
                        {
                            Id = new Guid("9916f23f-58d2-4f6d-9a71-24c11f949f0f"),
                            ImagePath = "/cards/Sashimi.png",
                            SushiType = 5,
                            Type = 11
                        },
                        new
                        {
                            Id = new Guid("ab9acdf6-2fef-4d0b-a563-cbe310cb52c8"),
                            ImagePath = "/cards/Tempura.png",
                            SushiType = 5,
                            Type = 12
                        },
                        new
                        {
                            Id = new Guid("fddda130-3cc1-49ca-b57a-29cb74abd9f5"),
                            ImagePath = "/cards/Tofu.png",
                            SushiType = 13,
                            Type = 13
                        },
                        new
                        {
                            Id = new Guid("8f309897-3131-4365-a6d2-7c3402a7c9ce"),
                            ImagePath = "/cards/Chopsticks.png",
                            SushiType = 13,
                            Type = 14
                        },
                        new
                        {
                            Id = new Guid("7246741c-0082-431c-9872-42c64e456a49"),
                            ImagePath = "/cards/Menu.png",
                            SushiType = 13,
                            Type = 15
                        },
                        new
                        {
                            Id = new Guid("751fa42d-9cb9-428e-b074-1ec3ea8ea483"),
                            ImagePath = "/cards/SoySauce.png",
                            SushiType = 13,
                            Type = 16
                        },
                        new
                        {
                            Id = new Guid("4a57d23b-aff6-46b2-a99e-aca738c4b091"),
                            ImagePath = "/cards/Spoon.png",
                            SushiType = 13,
                            Type = 17
                        },
                        new
                        {
                            Id = new Guid("4f3bd105-4daf-4831-a7da-f2173454b4bb"),
                            ImagePath = "/cards/SpecialOrder.png",
                            SushiType = 13,
                            Type = 18
                        },
                        new
                        {
                            Id = new Guid("f22cf0d5-5f78-445d-a4d2-115982f7fa50"),
                            ImagePath = "/cards/TakeoutBox.png",
                            SushiType = 13,
                            Type = 19
                        },
                        new
                        {
                            Id = new Guid("889d9cf4-41c3-49eb-a651-3d0fc432622d"),
                            ImagePath = "/cards/Tea.png",
                            SushiType = 13,
                            Type = 20
                        },
                        new
                        {
                            Id = new Guid("1c8caa7d-6e89-432e-ad59-4a775613646b"),
                            ImagePath = "/cards/Wasabi.png",
                            SushiType = 21,
                            Type = 21
                        },
                        new
                        {
                            Id = new Guid("371ea535-e022-47d0-9f8e-d30d8f072564"),
                            ImagePath = "/cards/GreenTeaIceCream.png",
                            SushiType = 21,
                            Type = 22
                        },
                        new
                        {
                            Id = new Guid("0452cd8d-50dd-4e69-b7a8-b75f79264404"),
                            ImagePath = "/cards/Fruit.png",
                            SushiType = 21,
                            Type = 23
                        },
                        new
                        {
                            Id = new Guid("9d927b2f-9a6a-46b7-b617-a94c6039a539"),
                            ImagePath = "/cards/Pudding.png",
                            SushiType = 24,
                            Type = 24
                        });
                });

            modelBuilder.Entity("shop.dal.Domain.Deck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DeckType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Decks");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c540adef-e447-4ca7-ac6f-f1cba0fe5851"),
                            DeckType = 0
                        },
                        new
                        {
                            Id = new Guid("19c5ac29-a017-40ce-8c04-183e83187189"),
                            DeckType = 1
                        },
                        new
                        {
                            Id = new Guid("929a7298-d727-4f91-a1d6-129da1d6139a"),
                            DeckType = 2
                        },
                        new
                        {
                            Id = new Guid("2bd4c609-5f6b-4976-96f3-a6d3d9f2a0a0"),
                            DeckType = 3
                        },
                        new
                        {
                            Id = new Guid("66b81989-410f-42d8-88f1-2cdff94748ab"),
                            DeckType = 4
                        },
                        new
                        {
                            Id = new Guid("443c1263-968c-43d7-a3e3-a2d1b6ae3ec3"),
                            DeckType = 5
                        },
                        new
                        {
                            Id = new Guid("ff44aabc-284b-4359-8e77-d49ac0cb9a1d"),
                            DeckType = 6
                        },
                        new
                        {
                            Id = new Guid("66ac5c86-2d27-4420-b64c-94a24c27adf9"),
                            DeckType = 7
                        });
                });

            modelBuilder.Entity("shop.dal.Domain.DeckCard", b =>
                {
                    b.Property<int>("DeckType")
                        .HasColumnType("int");

                    b.Property<int>("CardType")
                        .HasColumnType("int");

                    b.HasKey("DeckType", "CardType");

                    b.HasIndex("CardType");

                    b.ToTable("DeckCards");

                    b.HasData(
                        new
                        {
                            DeckType = 0,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 3
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 12
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 11
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 10
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 21
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 20
                        },
                        new
                        {
                            DeckType = 0,
                            CardType = 22
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 3
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 12
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 11
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 6
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 14
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 21
                        },
                        new
                        {
                            DeckType = 1,
                            CardType = 24
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 4
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 12
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 6
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 13
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 21
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 15
                        },
                        new
                        {
                            DeckType = 2,
                            CardType = 22
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 4
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 9
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 13
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 11
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 17
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 19
                        },
                        new
                        {
                            DeckType = 3,
                            CardType = 23
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 5
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 9
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 6
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 7
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 18
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 20
                        },
                        new
                        {
                            DeckType = 4,
                            CardType = 22
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 4
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 8
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 13
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 10
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 17
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 16
                        },
                        new
                        {
                            DeckType = 5,
                            CardType = 24
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 3
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 12
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 6
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 8
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 17
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 14
                        },
                        new
                        {
                            DeckType = 6,
                            CardType = 22
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 0
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 1
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 2
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 5
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 9
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 13
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 10
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 15
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 18
                        },
                        new
                        {
                            DeckType = 7,
                            CardType = 23
                        });
                });

            modelBuilder.Entity("shop.dal.Domain.DeckCard", b =>
                {
                    b.HasOne("shop.dal.Domain.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardType")
                        .HasPrincipalKey("Type")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("shop.dal.Domain.Deck", "Deck")
                        .WithMany("Cards")
                        .HasForeignKey("DeckType")
                        .HasPrincipalKey("DeckType")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Card");

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("shop.dal.Domain.Deck", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}

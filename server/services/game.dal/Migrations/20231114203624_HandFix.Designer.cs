﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using game.dal;

#nullable disable

namespace game.dal.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20231114203624_HandFix")]
    partial class HandFix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("game.dal.Domain.Board", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("game.dal.Domain.BoardCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("CardType")
                        .HasColumnType("int");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsCalculated")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("BoardId");

                    b.ToTable("BoardCards");
                });

            modelBuilder.Entity("game.dal.Domain.Deck", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cards")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("DeckType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("game.dal.Domain.Game", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ActualPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("DeckId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("DeckType")
                        .HasColumnType("int");

                    b.Property<Guid>("FirstPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Phase")
                        .HasColumnType("int");

                    b.Property<string>("PlayerIds")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DeckId")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("game.dal.Domain.Hand", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("Hands");
                });

            modelBuilder.Entity("game.dal.Domain.HandCard", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CardType")
                        .HasColumnType("int");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("HandId");

                    b.ToTable("HandCards");
                });

            modelBuilder.Entity("game.dal.Domain.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("BoardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("CanPlayAfterTurn")
                        .HasColumnType("bit");

                    b.Property<Guid>("GameId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("HandId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("NextPlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<Guid?>("SelectedCardId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("SelectedCardType")
                        .HasColumnType("int");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BoardId")
                        .IsUnique();

                    b.HasIndex("GameId");

                    b.HasIndex("HandId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("game.dal.Domain.BoardCard", b =>
                {
                    b.HasOne("game.dal.Domain.Board", "Board")
                        .WithMany("Cards")
                        .HasForeignKey("BoardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Board");
                });

            modelBuilder.Entity("game.dal.Domain.Game", b =>
                {
                    b.HasOne("game.dal.Domain.Deck", "Deck")
                        .WithOne("Game")
                        .HasForeignKey("game.dal.Domain.Game", "DeckId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Deck");
                });

            modelBuilder.Entity("game.dal.Domain.HandCard", b =>
                {
                    b.HasOne("game.dal.Domain.Hand", "Hand")
                        .WithMany("Cards")
                        .HasForeignKey("HandId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Hand");
                });

            modelBuilder.Entity("game.dal.Domain.Player", b =>
                {
                    b.HasOne("game.dal.Domain.Board", "Board")
                        .WithOne("Player")
                        .HasForeignKey("game.dal.Domain.Player", "BoardId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("game.dal.Domain.Game", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("game.dal.Domain.Hand", "Hand")
                        .WithMany()
                        .HasForeignKey("HandId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Board");

                    b.Navigation("Game");

                    b.Navigation("Hand");
                });

            modelBuilder.Entity("game.dal.Domain.Board", b =>
                {
                    b.Navigation("Cards");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("game.dal.Domain.Deck", b =>
                {
                    b.Navigation("Game")
                        .IsRequired();
                });

            modelBuilder.Entity("game.dal.Domain.Game", b =>
                {
                    b.Navigation("Players");
                });

            modelBuilder.Entity("game.dal.Domain.Hand", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}

using game.dal.Domain;
using Microsoft.EntityFrameworkCore;
using shared.dal.Comparers;
using shared.dal.Converters;
using shared.dal.Models;

namespace game.dal
{
    public class GameDbContext : DbContext
    {
        public DbSet<Board> Boards => Set<Board>();
        public DbSet<BoardCard> BoardCards => Set<BoardCard>();
        public DbSet<Deck> Decks => Set<Deck>();
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Hand> Hands => Set<Hand>();
        public DbSet<HandCard> HandCards => Set<HandCard>();
        public DbSet<Player> Players => Set<Player>();
        public GameDbContext(DbContextOptions<GameDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Player>()
                .HasOne(p => p.Hand)
                .WithOne(h => h.Player)
                .HasForeignKey<Player>(p => p.HandId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Player>()
                .HasOne(p => p.Board)
                .WithOne(b => b.Player)
                .HasForeignKey<Player>(p => p.BoardId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Player>()
                .HasOne(p => p.Game)
                .WithMany(g => g.Players)
                .HasForeignKey(p => p.GameId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Game>()
                .HasOne(g => g.Deck)
                .WithOne(d => d.Game)
                .HasForeignKey<Game>(g => g.DeckId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<BoardCard>()
                .HasOne(bc => bc.Board)
                .WithMany(b => b.Cards)
                .HasForeignKey(bc => bc.BoardId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<HandCard>()
                .HasOne(hc => hc.Hand)
                .WithMany(h => h.Cards)
                .HasForeignKey(hc => hc.HandId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<BoardCard>()
                .Property(bc => bc.AdditionalInfo)
                .HasConversion<DictionaryStringValueConverter<string>>()
                .Metadata.SetValueComparer(new DictionaryValueComparer<string>());
            builder.Entity<HandCard>()
                .Property(hc => hc.AdditionalInfo)
                .HasConversion<DictionaryStringValueConverter<string>>()
                .Metadata.SetValueComparer(new DictionaryValueComparer<string>());
            builder.Entity<Game>()
                .Property(g => g.AdditionalInfo)
                .HasConversion<DictionaryStringValueConverter<string>>()
                .Metadata.SetValueComparer(new DictionaryValueComparer<string>());
            builder.Entity<Deck>()
                .Property(g => g.AdditionalInfo)
                .HasConversion<DictionaryStringValueConverter<string>>()
                .Metadata.SetValueComparer(new DictionaryValueComparer<string>());
            builder.Entity<Deck>()
                .Property(d => d.Cards)
                .HasConversion<EnumCollectionJsonValueConverter<CardType>>()
                .Metadata.SetValueComparer(new CollectionValueComparer<CardType>());
        }
    }
}

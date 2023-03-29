using Microsoft.EntityFrameworkCore;
using shop.dal.Domain;
using shop.dal.Extensions;

namespace shop.dal
{
    public class ShopDbContext : DbContext
    {
        public DbSet<Card> Cards => Set<Card>();
        public DbSet<Deck> Decks => Set<Deck>();
        public DbSet<DeckCard> DeckCards => Set<DeckCard>();
        public ShopDbContext(DbContextOptions<ShopDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.AddRelationExtensions();
            builder.AddSeedExtensions();
        }
    }
}

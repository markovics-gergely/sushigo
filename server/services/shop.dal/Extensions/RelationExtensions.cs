using Microsoft.EntityFrameworkCore;
using shop.dal.Domain;

namespace shop.dal.Extensions
{
    public static class RelationExtensions
    {
        public static void AddRelationExtensions(this ModelBuilder builder)
        {
            builder.Entity<Card>()
                .HasAlternateKey(c => c.Type);

            builder.Entity<Deck>()
                .HasAlternateKey(d => d.DeckType);

            builder.Entity<DeckCard>()
                .HasKey(dc => new { dc.DeckType, dc.CardType });

            builder.Entity<DeckCard>()
                .HasOne(dc => dc.Deck)
                .WithMany(dc => dc.Cards)
                .HasForeignKey(dc => dc.DeckType)
                .HasPrincipalKey(d => d.DeckType);

            builder.Entity<DeckCard>()
                .HasOne(dc => dc.Card)
                .WithMany()
                .HasForeignKey(dc => dc.CardType)
                .HasPrincipalKey(c => c.Type);
        }
    }
}

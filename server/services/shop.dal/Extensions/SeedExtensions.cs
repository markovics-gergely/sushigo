using Microsoft.EntityFrameworkCore;
using shop.dal.Domain;

namespace shop.dal.Extensions
{
    public static class SeedExtensions
    {
        public static void AddSeedExtensions(this ModelBuilder builder)
        {
            var typeIdMap = Enum.GetValues(typeof(CardType)).Cast<CardType>().ToDictionary(type => type, _ => new Guid());
            foreach (var type in typeIdMap)
            {
                builder.Entity<Card>()
                    .HasData(
                        new Card
                        {
                            Id = type.Value,
                            Type = type.Key,
                            SushiType = type.Key.GetSushiType(),
                            ImagePath = $"/cards/{type.Key}.png"
                        }
                    );
            }
            var decktypeIdMap = Enum.GetValues(typeof(DeckType)).Cast<DeckType>().ToDictionary(type => type, _ => new Guid());
            foreach (var deckType in decktypeIdMap)
            {
                builder.Entity<Deck>()
                        .HasData(
                            new Deck
                            {
                                Id = deckType.Value,
                                DeckType = deckType.Key
                            }
                        );
                foreach (var cardType in deckType.Key.GetCardTypes())
                {
                    builder.Entity<DeckCard>()
                        .HasData(
                            new DeckCard
                            {
                                Id = new Guid(),
                                DeckType = deckType.Key,
                                CardType = cardType
                            }
                        );
                }
            }
        }
    }
}

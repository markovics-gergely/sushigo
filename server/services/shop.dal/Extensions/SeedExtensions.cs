using Microsoft.EntityFrameworkCore;
using shared.dal.Models;
using shop.dal.Domain;

namespace shop.dal.Extensions
{
    public static class SeedExtensions
    {
        private static IDictionary<DeckType, int[]> DeckCards { get; } = new Dictionary<DeckType, int[]>()
        {
            { DeckType.MyFirstMeal, new int[] { 3, 12, 11, 10, 21, 20, 22 } },
            { DeckType.SushiGo, new int[] { 3, 12, 11, 6, 14, 21, 24 } },
            { DeckType.PartySampler, new int[] { 4, 12, 6, 13, 21, 15, 22 } },
            { DeckType.MasterMenu, new int[] { 4, 9, 13, 11, 17, 19, 23 } },
            { DeckType.PointsPlatter, new int[] { 5, 9, 6, 7, 18, 20, 22 } },
            { DeckType.CutThroatCombo, new int[] { 4, 8, 13, 10, 17, 16, 24 } },
            { DeckType.BigBanquet, new int[] { 3, 12, 6, 8, 17, 14, 22 } },
            { DeckType.DinnerForTwo, new int[] { 5, 9, 13, 10, 15, 18, 23 } }
        };
        private static IEnumerable<CardType> NigiriList { get; } = new List<CardType> { CardType.EggNigiri, CardType.SalmonNigiri, CardType.SquidNigiri };
        public static ICollection<CardType> GetCardTypes(this DeckType deckType)
        {
            return NigiriList.Concat(DeckCards[deckType].Select(d => (CardType)d)).ToList();
        }

        public static void AddSeedExtensions(this ModelBuilder builder)
        {
            var typeIdMap = Enum.GetValues(typeof(CardType)).Cast<CardType>().ToDictionary(type => type, _ => Guid.NewGuid());
            foreach (var type in typeIdMap)
            {
                builder.Entity<Card>()
                    .HasData(
                        new Card
                        {
                            Id = type.Value,
                            Type = type.Key,
                            SushiType = type.Key.SushiType(),
                            ImagePath = $"/cards/{type.Key}.png"
                        }
                    );
            }
            var decktypeIdMap = Enum.GetValues(typeof(DeckType)).Cast<DeckType>().ToDictionary(type => type, _ => Guid.NewGuid());
            foreach (var deckType in decktypeIdMap)
            {
                builder.Entity<Deck>()
                        .HasData(
                            new Deck
                            {
                                Id = deckType.Value,
                                DeckType = deckType.Key,
                                Cost = RoleTypes.DeckExp,
                                ImagePath = $"/decks/{deckType.Key}.png"
                            }
                        );
                foreach (var cardType in deckType.Key.GetCardTypes())
                {
                    builder.Entity<DeckCard>()
                        .HasData(
                            new DeckCard
                            {
                                Id = Guid.NewGuid(),
                                DeckType = deckType.Key,
                                CardType = cardType
                            }
                        );
                }
            }
        }
    }
}

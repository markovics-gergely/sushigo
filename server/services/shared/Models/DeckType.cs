namespace shared.Models
{
    public enum DeckType
    {
        MyFirstMeal,
        SushiGo,
        PartySampler,
        MasterMenu,
        PointsPlatter,
        CutThroatCombo,
        BigBanquet,
        DinnerForTwo
    }

    public static class DeckExtensions
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

        public static int GetMinPlayer(this DeckType deckType)
        {
            return deckType switch
            {
                DeckType.BigBanquet => 6,
                DeckType.MasterMenu or DeckType.PointsPlatter or DeckType.CutThroatCombo => 3,
                _ => 2,
            };
        }

        public static int GetMaxPlayer(this DeckType deckType)
        {
            return deckType switch
            {
                DeckType.DinnerForTwo => 2,
                DeckType.MyFirstMeal or DeckType.SushiGo => 5,
                DeckType.PartySampler or DeckType.PointsPlatter => 6,
                _ => 8,
            };
        }
    }
}

namespace shop.dal.Domain
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
        private static IDictionary<DeckType, ICollection<CardType>> DeckCards { get; } = new Dictionary<DeckType, ICollection<CardType>>()
        {
            { DeckType.MyFirstMeal, new List<CardType> { CardType.EggNigiri, CardType.SalmonNigiri, CardType.SquidNigiri } },
            { DeckType.SushiGo, new List<CardType> { CardType.MakiRoll, CardType.Temaki, CardType.Uramaki } },
            { DeckType.PartySampler, new List<CardType> { CardType.Dumpling, CardType.Edamame, CardType.Eel, CardType.Onigiri, CardType.MisoSoup, CardType.Sashimi, CardType.Tempura, CardType.Tofu } },
            { DeckType.MasterMenu, new List<CardType> { CardType.Chopsticks, CardType.Menu, CardType.SoySauce, CardType.Spoon, CardType.SpecialOrder, CardType.TakeoutBox, CardType.Tea, CardType.Wasabi } },
            { DeckType.PointsPlatter, new List<CardType> { CardType.GreenTeaIceCream, CardType.Fruit, CardType.Pudding } }
        };

        public static ICollection<CardType> GetCardTypes(this DeckType deckType)
        {
            return DeckCards[deckType];
        }
    }
}

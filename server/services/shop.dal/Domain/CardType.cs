namespace shop.dal.Domain
{
    public enum CardType
    {
        EggNigiri,
        SalmonNigiri,
        SquidNigiri,
        MakiRoll,
        Temaki,
        Uramaki,
        Dumpling,
        Edamame,
        Eel,
        Onigiri,
        MisoSoup,
        Sashimi,
        Tempura,
        Tofu,
        Chopsticks,
        Menu,
        SoySauce,
        Spoon,
        SpecialOrder,
        TakeoutBox,
        Tea,
        Wasabi,
        GreenTeaIceCream,
        Fruit,
        Pudding
    }

    public static class Extensions
    {
        private static readonly int[] lastOfTypes = { 3, 5, 13, 21, 24 };
        public static SushiType GetSushiType(this CardType card)
        {
            var last = lastOfTypes.LastOrDefault(t => t <= (int)card);
            return (SushiType)last;
        }
    }
}

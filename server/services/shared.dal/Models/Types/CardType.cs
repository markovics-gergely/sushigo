using System.ComponentModel;

namespace shared.dal.Models.Types
{
    public enum CardType
    {
        [Description("Egg Nigiri")]
        EggNigiri,
        [Description("Salmon Nigiri")]
        SalmonNigiri,
        [Description("Squid Nigiri")]
        SquidNigiri,
        [Description("Maki Roll")]
        MakiRoll,
        [Description("Temaki")]
        Temaki,
        [Description("Uramaki")]
        Uramaki,
        [Description("Dumpling")]
        Dumpling,
        [Description("Edamame")]
        Edamame,
        [Description("Eel")]
        Eel,
        [Description("Onigiri")]
        Onigiri,
        [Description("Miso Soup")]
        MisoSoup,
        [Description("Sashimi")]
        Sashimi,
        [Description("Tempura")]
        Tempura,
        [Description("Tofu")]
        Tofu,
        [Description("Chopsticks")]
        Chopsticks,
        [Description("Menu")]
        Menu,
        [Description("Soy Sauce")]
        SoySauce,
        [Description("Spoon")]
        Spoon,
        [Description("Special Order")]
        SpecialOrder,
        [Description("Takeout Box")]
        TakeoutBox,
        [Description("Tea")]
        Tea,
        [Description("Wasabi")]
        Wasabi,
        [Description("Green Tea Ice Cream")]
        GreenTeaIceCream,
        [Description("Fruit")]
        Fruit,
        [Description("Pudding")]
        Pudding
    }
    public static class Extensions
    {
        public static SushiType SushiType(this CardType cardType) => cardType.GetClass().SushiType;
        public static CardTypeWrapper GetClass(this CardType card)
        {
            return card switch
            {
                CardType.EggNigiri => new EggNigiri(),
                CardType.SalmonNigiri => new SalmonNigiri(),
                CardType.SquidNigiri => new SquidNigiri(),
                CardType.MakiRoll => new MakiRoll(),
                CardType.Temaki => new Temaki(),
                CardType.Uramaki => new Uramaki(),
                CardType.Dumpling => new Dumpling(),
                CardType.Edamame => new Edamame(),
                CardType.Eel => new Eel(),
                CardType.Onigiri => new Onigiri(),
                CardType.MisoSoup => new MisoSoup(),
                CardType.Sashimi => new Sashimi(),
                CardType.Tempura => new Tempura(),
                CardType.Tofu => new Tofu(),
                CardType.Chopsticks => new Chopsticks(),
                CardType.Menu => new Menu(),
                CardType.SoySauce => new SoySauce(),
                CardType.Spoon => new Spoon(),
                CardType.SpecialOrder => new SpecialOrder(),
                CardType.TakeoutBox => new TakeoutBox(),
                CardType.Tea => new Tea(),
                CardType.Wasabi => new Wasabi(),
                CardType.GreenTeaIceCream => new GreenTeaIceCream(),
                CardType.Fruit => new Fruit(),
                CardType.Pudding => new Pudding(),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
        public static Type GetClassType(this CardType card) => card switch
        {
            CardType.EggNigiri => typeof(EggNigiri),
            CardType.SalmonNigiri => typeof(SalmonNigiri),
            CardType.SquidNigiri => typeof(SquidNigiri),
            CardType.MakiRoll => typeof(MakiRoll),
            CardType.Temaki => typeof(Temaki),
            CardType.Uramaki => typeof(Uramaki),
            CardType.Dumpling => typeof(Dumpling),
            CardType.Edamame => typeof(Edamame),
            CardType.Eel => typeof(Eel),
            CardType.Onigiri => typeof(Onigiri),
            CardType.MisoSoup => typeof(MisoSoup),
            CardType.Sashimi => typeof(Sashimi),
            CardType.Tempura => typeof(Tempura),
            CardType.Tofu => typeof(Tofu),
            CardType.Chopsticks => typeof(Chopsticks),
            CardType.Menu => typeof(Menu),
            CardType.SoySauce => typeof(SoySauce),
            CardType.Spoon => typeof(Spoon),
            CardType.SpecialOrder => typeof(SpecialOrder),
            CardType.TakeoutBox => typeof(TakeoutBox),
            CardType.Tea => typeof(Tea),
            CardType.Wasabi => typeof(Wasabi),
            CardType.GreenTeaIceCream => typeof(GreenTeaIceCream),
            CardType.Fruit => typeof(Fruit),
            CardType.Pudding => typeof(Pudding),
            _ => throw new ArgumentOutOfRangeException(nameof(card)),
        };

        public static bool HasAfterTurn(this CardType cardType)
        {
            return cardType switch
            {
                CardType.Chopsticks => true,
                CardType.Spoon => true,
                _ => false,
            };
        }

        public static bool HasAfterTurnInHand(this CardType cardType)
        {
            return cardType switch
            {
                CardType.Menu => true,
                _ => false,
            };
        }

        public static int[]? GetPoints(this CardType cardType)
        {
            return cardType switch
            {
                CardType.MakiRoll => new int[] { 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3 },
                CardType.Uramaki => new int[] { 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5 },
                CardType.Onigiri => new int[] { 0, 0, 1, 1, 2, 2, 3, 3 },
                CardType.Fruit => new int[] { 200, 200, 020, 020, 002, 002, 110, 110, 110, 011, 011, 011, 101, 101, 101 },
                _ => null,
            };
        }

        public static List<CardType> HasPointsList { get; } = new List<CardType>
        {
            CardType.MakiRoll,
            CardType.Uramaki,
            CardType.Onigiri,
            CardType.Fruit
        };

        public static bool HasPoints(this CardType type) => HasPointsList.Contains(type);

        public static Queue<int>? GetShuffledPoints(this CardType cardType)
        {
            var points = cardType.GetPoints();
            if (points == null) return null;
            return new Queue<int>(points.OrderBy(x => Guid.NewGuid()));
        }
    }
}

namespace shared.dal.Models
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

        public static bool HasAfterTurnInHand(this CardType? cardType)
        {
            return cardType switch
            {
                CardType.Menu => true,
                _ => false,
            };
        }
    }
}

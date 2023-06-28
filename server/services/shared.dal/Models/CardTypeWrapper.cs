namespace shared.dal.Models
{
    public abstract class CardTypeWrapper
    {
        public CardType CardType { get; }
        public SushiType SushiType { get; }
        public CardTypeWrapper(CardType cardType, SushiType sushiType) { CardType = cardType; SushiType = sushiType; }
    }
    public class EggNigiri : CardTypeWrapper { public EggNigiri() : base(CardType.EggNigiri, SushiType.Nigiri) { } }
    public class SalmonNigiri : CardTypeWrapper { public SalmonNigiri() : base(CardType.SalmonNigiri, SushiType.Nigiri) { } }
    public class SquidNigiri : CardTypeWrapper { public SquidNigiri() : base(CardType.SquidNigiri, SushiType.Nigiri) { } }
    public class MakiRoll : CardTypeWrapper { public MakiRoll() : base(CardType.MakiRoll, SushiType.Roll) { } }
    public class Temaki : CardTypeWrapper { public Temaki() : base(CardType.Temaki, SushiType.Roll) { } }
    public class Uramaki : CardTypeWrapper { public Uramaki() : base(CardType.Uramaki, SushiType.Roll) { } }
    public class Dumpling : CardTypeWrapper { public Dumpling() : base(CardType.Dumpling, SushiType.Appetizer) { } }
    public class Edamame : CardTypeWrapper { public Edamame() : base(CardType.Edamame, SushiType.Appetizer) { } }
    public class Eel : CardTypeWrapper { public Eel() : base(CardType.Eel, SushiType.Appetizer) { } }
    public class Onigiri : CardTypeWrapper { public Onigiri() : base(CardType.Onigiri, SushiType.Appetizer) { } }
    public class MisoSoup : CardTypeWrapper { public MisoSoup() : base(CardType.MisoSoup, SushiType.Appetizer) { } }
    public class Sashimi : CardTypeWrapper { public Sashimi() : base(CardType.Sashimi, SushiType.Appetizer) { } }
    public class Tempura : CardTypeWrapper { public Tempura() : base(CardType.Tempura, SushiType.Appetizer) { } }
    public class Tofu : CardTypeWrapper { public Tofu() : base(CardType.Tofu, SushiType.Appetizer) { } }
    public class Chopsticks : CardTypeWrapper { public Chopsticks() : base(CardType.Chopsticks, SushiType.Special) { } }
    public class Menu : CardTypeWrapper { public Menu() : base(CardType.Menu, SushiType.Special) { } }
    public class SoySauce : CardTypeWrapper { public SoySauce() : base(CardType.SoySauce, SushiType.Special) { } }
    public class Spoon : CardTypeWrapper { public Spoon() : base(CardType.Spoon, SushiType.Special) { } }
    public class SpecialOrder : CardTypeWrapper { public SpecialOrder() : base(CardType.SpecialOrder, SushiType.Special) { } }
    public class TakeoutBox : CardTypeWrapper { public TakeoutBox() : base(CardType.TakeoutBox, SushiType.Special) { } }
    public class Tea : CardTypeWrapper { public Tea() : base(CardType.Tea, SushiType.Special) { } }
    public class Wasabi : CardTypeWrapper { public Wasabi() : base(CardType.Wasabi, SushiType.Special) { } }
    public class GreenTeaIceCream : CardTypeWrapper { public GreenTeaIceCream() : base(CardType.GreenTeaIceCream, SushiType.Dessert) { } }
    public class Fruit : CardTypeWrapper { public Fruit() : base(CardType.Fruit, SushiType.Dessert) { } }
    public class Pudding : CardTypeWrapper { public Pudding() : base(CardType.Pudding, SushiType.Dessert) { } }
}

namespace shared.dal.Models
{
    public abstract class DeckTypeWrapper
    {
        public DeckType DeckType { get; }
        public DeckTypeWrapper(DeckType deckType) { DeckType = deckType; }
    }
    public class MyFirstMeal : DeckTypeWrapper { public MyFirstMeal() : base(DeckType.MyFirstMeal) { } }
    public class SushiGo : DeckTypeWrapper { public SushiGo() : base(DeckType.SushiGo) { } }
    public class PartySampler : DeckTypeWrapper { public PartySampler() : base(DeckType.PartySampler) { } }
    public class MasterMenu : DeckTypeWrapper { public MasterMenu() : base(DeckType.MasterMenu) { } }
    public class PointsPlatter : DeckTypeWrapper { public PointsPlatter() : base(DeckType.PointsPlatter) { } }
    public class CutThroatCombo : DeckTypeWrapper { public CutThroatCombo() : base(DeckType.CutThroatCombo) { } }
    public class BigBanquet : DeckTypeWrapper { public BigBanquet() : base(DeckType.BigBanquet) { } }
    public class DinnerForTwo : DeckTypeWrapper { public DinnerForTwo() : base(DeckType.DinnerForTwo) { } }
}

using shared.dal.Models.Types;

namespace game.dal.Domain
{
    public class CardInfo : ICloneable
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public CardType CardType { get; set; }

        public int? Point {  get; set; }
        public CardTagType? CustomTag { get; set; }
        public ICollection<Guid>? CardIds { get; set; }
        public string? CustomTagString { get; set; }

        public object Clone()
        {
            return new CardInfo 
            { 
                GameId = GameId, 
                CardType = CardType, 
                Point = Point, 
                CustomTag = CustomTag,
                CardIds = CardIds,
                CustomTagString = CustomTagString
            };
        }

        public void Reset()
        {
            Point = null;
            CustomTag = null;
            CardIds = null;
            CustomTagString = null;
        }
    }
}

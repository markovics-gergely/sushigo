using game.dal.Domain;
using shared.dal.Models.Types;

namespace game.bll.Infrastructure.ViewModels
{
    public class CardInfoViewModel
    {
        public CardType CardType { get; set; }

        public int? Point { get; set; }
        public CardTagType? CustomTag { get; set; }
        public string? CustomTagString { get; set; }
        public IEnumerable<Guid>? CardIds { get; set; }

        public void UpdateCardInfo(CardInfo cardInfo)
        {
            if (cardInfo == null) { return; }
            if (cardInfo.CardType != CardType)
            {
                cardInfo.CardType = CardType;
            }
            if (Point != null)
            {
                cardInfo.Point = Point;
            }
            if (CustomTag != null)
            {
                cardInfo.CustomTag = CustomTag;
            }
            if (!string.IsNullOrWhiteSpace(CustomTagString))
            {
                cardInfo.CustomTagString = CustomTagString;
            }
            if (CardIds != null)
            {
                cardInfo.CardIds = new List<Guid>(CardIds);
            }
        }
    }
}

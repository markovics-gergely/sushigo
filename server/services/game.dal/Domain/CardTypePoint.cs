using shared.dal.Models;

namespace game.dal.Domain
{
    public class CardTypePoint
    {
        CardType CardType { get; set; }
        int Point { get; set; }
        public override string ToString()
        {
            return $"{CardType}|{Point}";
        }
    }
}

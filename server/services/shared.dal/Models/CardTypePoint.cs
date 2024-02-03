namespace shared.dal.Models
{
    public class CardTypePoint
    {
        public CardType CardType { get; set; }
        public int Point { get; set; }
        public override string ToString()
        {
            return $"{CardType}|{Point}";
        }
        public static CardTypePoint FromString(string value)
        {
            var parts = value.Split('|');
            return new CardTypePoint
            {
                CardType = (CardType)Enum.Parse(typeof(CardType), parts[0]),
                Point = int.Parse(parts[1])
            };
        }
    }
}

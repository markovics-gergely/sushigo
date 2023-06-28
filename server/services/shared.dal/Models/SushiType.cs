namespace shared.dal.Models
{
    public enum SushiType
    {
        Nigiri,
        Roll,
        Appetizer,
        Special,
        Dessert
    }

    public static class SushiExtensions
    {
        public static int GetCount(this SushiType type)
        {
            return type switch
            {
                SushiType.Nigiri => 12,
                SushiType.Roll => 12,
                SushiType.Appetizer => 8,
                SushiType.Special => 3,
                SushiType.Dessert => 15,
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }
    }
}

namespace shared.dal.Models.Types
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
        public static int GetCount(this SushiType type, int round = 0, int count = 2)
        {
            return type switch
            {
                SushiType.Nigiri => 12,
                SushiType.Roll => 12,
                SushiType.Appetizer => 8,
                SushiType.Special => 3,
                SushiType.Dessert => CalculateDessert(round, count),
                _ => throw new ArgumentOutOfRangeException(nameof(type)),
            };
        }

        private static int CalculateDessert(int round = 0, int count = 2)
        {
            if (count < 6)
            {
                return round switch
                {
                    0 => 5,
                    1 => 8,
                    2 => 10,
                    _ => 0
                };
            }
            else
            {
                return round switch
                {
                    0 => 7,
                    1 => 12,
                    2 => 15,
                    _ => 0
                };
            }
        }
    }
}

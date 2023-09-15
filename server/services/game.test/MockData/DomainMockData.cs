using game.dal.Domain;

namespace game.test.MockData
{
    public static class DomainMockData
    {
        public static Player Player { get; } = new Player
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            UserName = "TestUser"
        };
        public static List<Player> Players { get; } = new List<Player> { Player };

        public static BoardCard BoardCard { get; } = new BoardCard
        {
            Id = Guid.NewGuid()
        };
        public static List<BoardCard> BoardCards { get; } = new List<BoardCard> { BoardCard };
        public static HandCard HandCard { get; } = new HandCard
        {
            Id = Guid.NewGuid()
        };
        public static List<HandCard> HandCards { get; } = new List<HandCard> { HandCard };
    }
}

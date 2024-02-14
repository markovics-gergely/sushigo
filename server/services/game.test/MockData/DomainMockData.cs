using game.dal.Domain;
using shared.dal.Models.Types;

namespace game.test.MockData
{
    public static class DomainMockData
    {
        public static Guid UserId { get; } = Guid.NewGuid();
        public static Guid PlayerId { get; } = Guid.NewGuid();
        public static Guid HandId { get; } = Guid.NewGuid();
        public static Guid BoardId { get; } = Guid.NewGuid();
        public static Guid GameId { get; } = Guid.NewGuid();
        public static Guid DeckId { get; } = Guid.NewGuid();
        public static Guid HandCardId { get; } = Guid.NewGuid();
        public static Guid BoardCardId { get; } = Guid.NewGuid();
        public static Game Game { get; } = new Game
        {
            Id = GameId,
            DeckId = DeckId,
            Name = "TestName",
            DeckType = DeckType.MyFirstMeal,
        };
        public static Player Player { get; } = new Player
        {
            Id = PlayerId,
            UserId = UserId,
            UserName = "TestUser",
            HandId = HandId,
            GameId = GameId,
            BoardId = BoardId,
            NextPlayerId = PlayerId,
            Points = 2
        };
        public static List<Player> Players { get; } = new List<Player> { Player };

        public static BoardCard BoardCard { get; } = new BoardCard
        {
            Id = BoardCardId,
            GameId = GameId,
            BoardId = BoardId,
            CardInfo = new CardInfo
            {
                CardType = CardType.Wasabi
            }
        };
        public static List<BoardCard> BoardCards { get; } = new List<BoardCard> { BoardCard };
        public static List<BoardCard> UramakiBoardCards {
            get {
                BoardCard uramakiCard = BoardCard;
                uramakiCard.CardInfo = new CardInfo()
                {
                    CardType = CardType.Uramaki,
                    Point = 10
                };
                return new List<BoardCard> {  uramakiCard, uramakiCard };
            }
        }
        public static HandCard HandCard { get; } = new HandCard
        {
            Id = HandCardId,
            GameId = GameId,
            HandId = HandId,
            CardInfo = new CardInfo { CardType = CardType.Wasabi }
        };
        public static List<HandCard> HandCards { get; } = new List<HandCard> { HandCard };
    }
}

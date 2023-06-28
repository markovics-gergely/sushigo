using shared.dal.Models;

namespace game.dal.Domain
{
    public class BoardCard
    {
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public bool IsCalculated { get; set; }
        public Guid BoardId { get; set; }
        public Board? Board { get; set; }
        public CardType CardType { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; } = new Dictionary<string, string>();
    }
}

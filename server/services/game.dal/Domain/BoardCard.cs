namespace game.dal.Domain
{
    public class BoardCard
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }
        //public bool IsCalculated { get; set; }

        public Guid BoardId { get; set; }
        public Board? Board { get; set; }

        public Guid CardInfoId { get; set; }
        public CardInfo CardInfo { get; set; } = new CardInfo();
    }
}

namespace game.dal.Domain
{
    public class HandCard
    {
        public Guid Id { get; set; }

        public Guid GameId { get; set; }
        public bool IsSelected { get; set; } = false;

        public Guid HandId { get; set; }
        public Hand? Hand { get; set; }

        public Guid CardInfoId { get; set; }
        public CardInfo CardInfo { get; set; } = new CardInfo();
    }
}

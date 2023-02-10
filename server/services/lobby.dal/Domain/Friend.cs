namespace lobby.dal.Domain
{
    public class Friend
    {
        public Guid Id { get; set; }
        public required Guid FriendA { get; set; }
        public required Guid FriendB { get; set; }
        public required bool PendingA { get; set; } = true;
        public required bool PendingB { get; set; } = true;
    }
}

using user.dal.Types;

namespace user.dal.Domain
{
    public class Friend
    {
        public Guid Id { get; set; }
        public required Guid SenderId { get; set; }
        public required ApplicationUser Sender { get; set; }
        public required Guid ReceiverId { get; set; }
        public required ApplicationUser Receiver { get; set; }
        public required bool Pending { get; set; } = true;
        public FriendTypes GetFriendTypes(Guid guid)
        {
            if (guid == SenderId && Pending) return FriendTypes.Sent;
            if (guid == ReceiverId && Pending) return FriendTypes.Received;
            if (guid == SenderId || guid == ReceiverId) return FriendTypes.Friend;
            return FriendTypes.None;
        }
    }
}
